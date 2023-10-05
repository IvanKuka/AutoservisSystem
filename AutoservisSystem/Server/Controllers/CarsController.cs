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
    public class CarsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {
            return await _context.Cars.Where(c => c.CarId != 1).ToListAsync();
        }

        // GET: api/Cars
        [HttpGet("event/{cars}")]
        public async Task<ActionResult<IEnumerable<Car>>> GetEventCars(List<int> cars)
        {
            List<Car> tmp = new();
            foreach(var item in cars)
            {
                tmp.Add(await _context.Cars.Where(c => c.CarId != 1)
                    .Where(c => c.CarId == item)
                    .FirstOrDefaultAsync());
            }
            return await _context.Cars.ToListAsync();
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null || car.CarId == 1)
            {
                return NotFound();
            }

            return car;
        }

        // GET: api/Cars/5
        [HttpGet("last")]
        public async Task<ActionResult<Car>> GetLastCar()
        {
            var car = await _context.Cars.OrderBy(c => c.CarId).LastOrDefaultAsync();

            if (car == null || car.CarId == 1)
            {
                return NotFound();
            }

            return car;
        }

        [HttpGet("notDone")]
        public async Task<ActionResult<IEnumerable<Car>>> GetCarInGarage()
        {
            var tmp = await _context.Orders.Where(o => o.Done == false).ToListAsync();

            List<Car> cars = new();
            Car car = new();
            foreach (Order order in tmp)
            {
                car = await _context.Cars.Where(c => c.CarId != 1)
                    .Where(c => c.CarEvidenceNumber == order.CarEvidenceNumber)
                    .FirstAsync();
                cars.Add(car);
                car = new Car();
            }

            if (cars == null)
            {
                return NotFound();
            }

            return cars;
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.CarId || id == 1)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
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

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.CarId }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null || car.CarId == 1)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
