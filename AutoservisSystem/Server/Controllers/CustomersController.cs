using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoservisSystem.Server.Data;
using AutoservisSystem.Shared;
using AutoservisSystem.Shared.Models;

namespace AutoservisSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var tmp = new List<Customer>();
            try
            {
                tmp = await _context.Customers
                .Where(c => c.CustomerId != 1)
                .Include(c => c.Cars)
                .Include(c => c.Orders)
                .ThenInclude(o => o.Tasks)
                .Include(c => c.CalendarEvents)
                .OrderBy(c => c.Surname).ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return tmp;
        }

        // GET: api/Customers
        [HttpGet("event/{customers}")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetEventCustomers(List<int> customers)
        {
            List<Customer> tmp = new();
            foreach(var item in customers)
            {
                tmp.Add(await _context.Customers
                    .Where(c => c.CustomerId != 1)
                    .Where(c => c.CustomerId == item).FirstOrDefaultAsync());
            }
            return tmp;
        }

        [HttpGet("notDone")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersCarInGarage()
        {
            var tmp = await _context.Orders
                .Where(o => o.Done == false).ToListAsync();

            List<Customer> customers = new();
            Customer customer = new();
            foreach (Order order in tmp)
            {
                customer = await _context.Customers
                    .Where(c => c.CustomerId == order.CustomerId).FirstAsync();
                customers.Add(customer);
                customer = new Customer();
            }

            if (customers == null)
            {
                return NotFound();
            }

            return customers;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.Where(c => c.CustomerId == id)
                .Include(c => c.Cars)
                .Include(c => c.Orders)
                .ThenInclude(o => o.Tasks).FirstAsync();

            if (customer == null || id == 1)
            {
                return NotFound();
            }

            return customer;
        }

        // GET: api/Customers/5
        [HttpGet("last")]
        public async Task<ActionResult<Customer>> GetLastCustomer()
        {
            var customer = await _context.Customers.OrderBy(c => c.CustomerId).LastOrDefaultAsync();

            if (customer == null || customer.CustomerId == 1)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId || id == 1)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null || id == 1)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
