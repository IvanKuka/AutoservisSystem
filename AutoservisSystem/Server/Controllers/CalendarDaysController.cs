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
    public class CalendarDaysController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalendarDaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CalendarDays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarDay>>> GetCalendarDays()
        {
            return await _context.CalendarDays.Include(c => c.Events).ToListAsync();
        }

        [HttpGet("specificDay/{years}")]
        public async Task<ActionResult<CalendarDay>> GetCalendarSpecificDay(int years)
        {
            var tmp = await _context.CalendarDays.Where(c => c.Date.Date == DateTime.Now.Date.AddYears(years))
                .Include(c => c.Events).FirstOrDefaultAsync();
            if(tmp == null)
            {
                tmp = new CalendarDay
                {
                    Date = DateTime.Now.AddYears(years),
                    IsEmpty = true,
                    DayNumber = DateTime.Now.AddYears(years).Day,
                    Events = new List<CalendarEvent>()
                };
                await PostCalendarDay(tmp);
            }
            return tmp;
        }

        [HttpGet("{year}/{month}")]
        public async Task<ActionResult<IEnumerable<CalendarDay>>> GetCalendarDaysByMonth(int year,int month)
        {
            var tmp = await _context.CalendarDays.Where(c => c.Date.Year == year)
                .Where(c => c.Date.Month == month)
                .Include(c => c.Events)
                .Include(c => c.Events).ToListAsync();
            if (tmp == null)
            {
                return NotFound();
            }

            return tmp;
        }

        // GET: api/CalendarDays/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarDay>> GetCalendarDay(int id)
        {
            var calendarDay = await _context.CalendarDays.Where(c => c.CalendarDayId == id).Include(c => c.Events).FirstAsync();

            if (calendarDay == null)
            {
                return NotFound();
            }

            return calendarDay;
        }

        // PUT: api/CalendarDays/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarDay(int id, CalendarDay calendarDay)
        {
            if (id != calendarDay.CalendarDayId)
            {
                return BadRequest();
            }

            _context.Entry(calendarDay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarDayExists(id))
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

        // POST: api/CalendarDays
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CalendarDay>> PostCalendarDay(CalendarDay calendarDay)
        {
            _context.CalendarDays.Add(calendarDay);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalendarDay", new { id = calendarDay.CalendarDayId }, calendarDay);
        }

        // DELETE: api/CalendarDays/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarDay(int id)
        {
            var calendarDay = await _context.CalendarDays.FindAsync(id);
            if (calendarDay == null)
            {
                return NotFound();
            }

            _context.CalendarDays.Remove(calendarDay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CalendarDayExists(int id)
        {
            return _context.CalendarDays.Any(e => e.CalendarDayId == id);
        }
    }
}
