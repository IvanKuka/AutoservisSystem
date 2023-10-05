using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoservisSystem.Server.Data;
using AutoservisSystem.Shared.Models;

namespace AutoservisSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersPartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersPartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OrdersParts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersPart>>> GetOrdersPart()
        {
            return await _context.OrdersParts.ToListAsync();
        }

        // GET: api/OrdersParts
        [HttpGet("orderId/{orderId}")]
        public async Task<ActionResult<IDictionary<int,OrdersPart>>> GetOrdersPartsByOrderId(int orderId)
        {
            Dictionary<int, OrdersPart> tmp = await _context.OrdersParts
                .Where(o => o.OrderId == orderId)
                .ToDictionaryAsync(o => o.PartId, o => o);
            var tmpList = await _context.OrdersParts.Where(o => o.OrderId == orderId).ToListAsync();
            return tmp;
        }

        // GET: api/OrdersParts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdersPart>> GetOrdersPart(int id)
        {
            var ordersPart = await _context.OrdersParts.FindAsync(id);

            if (ordersPart == null)
            {
                return NotFound();
            }

            return ordersPart;
        }

        // PUT: api/OrdersParts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrdersPart(int id, OrdersPart ordersPart)
        {
            if (id != ordersPart.OrdersPartId)
            {
                return BadRequest();
            }

            _context.Entry(ordersPart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersPartExists(id))
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

        // POST: api/OrdersParts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrdersPart>> PostOrdersPart(OrdersPart ordersPart)
        {
            _context.OrdersParts.Add(ordersPart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrdersPart", new { id = ordersPart.OrdersPartId }, ordersPart);
        }

        // DELETE: api/OrdersParts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrdersPart(int id)
        {
            var ordersPart = await _context.OrdersParts.FindAsync(id);
            if (ordersPart == null)
            {
                return NotFound();
            }

            _context.OrdersParts.Remove(ordersPart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdersPartExists(int id)
        {
            return _context.OrdersParts.Any(e => e.OrdersPartId == id);
        }
    }
}
