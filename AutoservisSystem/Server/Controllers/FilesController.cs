using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoservisSystem.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoservisSystem.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public FilesController(IWebHostEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        [HttpGet("{dateFrom}to{dateTo}")]
        public IActionResult DownloadExcel(string dateFrom, string dateTo)
        {
            byte[] reportBytes;

            CultureInfo provider = CultureInfo.InvariantCulture;

            DateTime from = DateTime.ParseExact(dateFrom, "dd.MM.yyyy", provider);
            DateTime to = DateTime.ParseExact(dateTo, "dd.MM.yyyy", provider);

            var orders = _context.Orders.Where(o => o.ReceivedDate.Date >= from.Date).Where(o => o.ReceivedDate.Date <= to.Date).ToList();
            var employees = _context.Employees.ToList();

            List<bool> automat = new List<bool>();

            foreach(var order in orders)
            {
                automat.Add(_context.Cars.Where(c => c.CarEvidenceNumber == order.CarEvidenceNumber).FirstOrDefault().AutomaticTransmission);
            }

            using (var package = Utils.CreateExcel(_hostingEnvironment, orders, employees, automat))
            {
                reportBytes = package.GetAsByteArray();
            }

            return File(reportBytes, XlsxContentType, $"Test{DateTime.Now.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}.xlsx");
        }
    }
}
