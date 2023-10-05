using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoservisSystem.Shared.Models;

namespace AutoservisSystem.Server
{
    public class Utils
    {
        public static ExcelPackage CreateExcel(IWebHostEnvironment _hostEnvironment, List<Order> orders, List<Employee> employees, List<bool> automat)
        {
            var package = new ExcelPackage();
            package.Workbook.Properties.Title = "Test Report";
            package.Workbook.Properties.Author = "Test";
            package.Workbook.Properties.Subject = "Test Report";
            package.Workbook.Properties.Keywords = "Testing";

            var worksheet = package.Workbook.Worksheets.Add("Order");

            //Hlavicka
            worksheet.Cells[1, 1].Value = "Číslo zákazky";
            worksheet.Cells[1, 2].Value = "Dátum prijatia";
            worksheet.Cells[1, 3].Value = "EČV";
            worksheet.Cells[1, 4].Value = "Číslo faktúry";
            worksheet.Cells[1, 5].Value = "cena";
            worksheet.Cells[1, 6].Value = "odpracované hodiny";
            worksheet.Cells[1, 7].Value = "pracovník";
            worksheet.Cells[1, 8].Value = "prevodovka";

            var numberFormat = "#0.00";
            var dataCellStyleName = "TableNumber";
            var numStyle = package.Workbook.Styles.CreateNamedStyle(dataCellStyleName);
            numStyle.Style.Numberformat.Format = numberFormat;

            int cursor = 2;
            foreach(var order in orders)
            {
                worksheet.Cells[cursor, 1].Value = order.OrderId;
                worksheet.Cells[cursor, 2].Value = order.ReceivedDate.Date.ToString("dd.MM.yyyy");
                worksheet.Cells[cursor, 3].Value = order.CarEvidenceNumber;
                worksheet.Cells[cursor, 4].Value = order.InvoiceNumber;
                worksheet.Cells[cursor, 5].Style.Numberformat.Format = numberFormat;
                worksheet.Cells[cursor, 5].Value = order.Price;
                worksheet.Cells[cursor, 6].Style.Numberformat.Format = "#0.0";
                worksheet.Cells[cursor, 6].Value = order.Hours;
                worksheet.Cells[cursor, 7].Value = employees.Where(e => e.EmployeeId == order.EmployeeId).FirstOrDefault().Name;
                worksheet.Cells[cursor, 8].Value = automat.First() == true ? "automaticka" : "";
                automat.RemoveAt(0);
                cursor++;
            }

            var table = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: orders.Count+1, toColumn: 8), "Data");
            table.ShowHeader = true;
            table.ShowTotal = true;
            table.Columns[5].DataCellStyleName = dataCellStyleName;
            table.Columns[5].TotalsRowFunction = RowFunctions.Sum;
            worksheet.Cells[orders.Count + 2, 5].Style.Numberformat.Format = numberFormat;
            table.Columns[4].DataCellStyleName = dataCellStyleName;
            table.Columns[4].TotalsRowFunction = RowFunctions.Sum;
            worksheet.Cells[orders.Count + 2, 6].Style.Numberformat.Format = numberFormat;

            worksheet.Cells[1, 1, orders.Count + 1, 8].AutoFitColumns();

            return package;

        }
    }
}
