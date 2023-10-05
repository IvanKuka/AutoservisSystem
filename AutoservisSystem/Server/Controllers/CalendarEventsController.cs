using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoservisSystem.Server.Data;
using AutoservisSystem.Server.Controllers;
using AutoservisSystem.Shared.Models;
using static System.Net.WebRequestMethods;

namespace AutoservisSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CalendarEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CalendarEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetCalendarEvents()
        {
            return await _context.CalendarEvents.ToListAsync();
        }

        // GET: api/CalendarEvents/5

        [HttpGet("{carId}/{customerId}/{description}/{years}")]
        public async Task<ActionResult<CalendarEvent>> GetCalendarSpecificEvent(int carId, int customerId, string description, int years)
        {
            var calendarEvent = await _context.CalendarEvents.Where(c => c.CarId == carId)
                .Where(c => c.Description.ToUpper() == description.ToUpper()).FirstOrDefaultAsync();


            var tmp = await _context.CalendarDays.Where(c => c.Date.Date == DateTime.Now.Date.AddYears(years)).FirstOrDefaultAsync();
            if(tmp == null)
            {
                tmp = new CalendarDay()
                {
                    Date = DateTime.Now.Date.AddYears(years),
                    DayNumber = DateTime.Now.AddYears(years).Day,
                    Events = new List<CalendarEvent>(),
                    IsEmpty = false
                };
                _context.CalendarDays.Add(tmp);
                await _context.SaveChangesAsync();
                tmp = await _context.CalendarDays.Where(c => c.Date.Date == DateTime.Now.Date.AddYears(years)).FirstOrDefaultAsync();
            }

            if (calendarEvent == null)
            {
                calendarEvent = new CalendarEvent()
                {
                    CalendarDayId = tmp.CalendarDayId,
                    CarId = carId,
                    Holiday = false,
                    Description = description,
                    DateFrom = DateTime.Today,
                    DateTo = DateTime.Parse("1.1.0001")
                };
                _context.CalendarEvents.Add(calendarEvent);
                await _context.SaveChangesAsync();
            }
            else
            {
                calendarEvent.CalendarDayId = tmp.CalendarDayId;
            }

            return calendarEvent;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEvent>> GetCalendarEvent(int id)
        {
            var calendarEvent = await _context.CalendarEvents.Where(c => c.CalendarEventId == id).FirstAsync();

            if (calendarEvent == null)
            {
                return NotFound();
            }

            return calendarEvent;
        }

        // PUT: api/CalendarEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarEvent(int id, CalendarEvent calendarEvent)
        {
            if (id != calendarEvent.CalendarEventId)
            {
                return BadRequest();
            }

            _context.Entry(calendarEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarEventExists(id))
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

        // POST: api/CalendarEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> PostCalendarEvent(CalendarEvent calendarEvent)
        {
            CalendarDay day = _context.CalendarDays.Where(c => c.Date.Date == calendarEvent.DateFrom.Date).FirstOrDefault();
            if(day == null)
            {
                day = new CalendarDay
                {
                    Date = calendarEvent.DateFrom.Date,
                    DayNumber = calendarEvent.DateFrom.Date.Day,
                    IsEmpty = true
                };
                _context.CalendarDays.Add(day);
                await _context.SaveChangesAsync();
                day = _context.CalendarDays.Where(c => c.Date.Date == calendarEvent.DateFrom.Date).FirstOrDefault();
            }
            calendarEvent.CalendarDayId = day.CalendarDayId;

            _context.CalendarEvents.Add(calendarEvent);
            await _context.SaveChangesAsync();
            if (calendarEvent.Holiday)
            {
                List<CalendarDay> days = new List<CalendarDay>();
                var date = calendarEvent.DateFrom.Date;
                while(date < calendarEvent.DateTo)
                {
                    date = date.AddDays(1);
                    day = _context.CalendarDays.Where(c => c.Date.Date == date.Date).FirstOrDefault();
                    if(day == null)
                    {
                        day = new CalendarDay
                        {
                            Date = date,
                            DayNumber = date.Day,
                            IsEmpty = true
                        };
                        _context.CalendarDays.Add(day);
                        await _context.SaveChangesAsync();
                        day = _context.CalendarDays.Where(c => c.Date.Date == date.Date).FirstOrDefault();
                        days.Add(day);
                    }
                    else
                    {
                        days.Add(day);
                    }
                }
                foreach(var tmpDay in days)
                {
                    var newEvent = new CalendarEvent();
                    newEvent = calendarEvent;
                    newEvent.CalendarDayId = tmpDay.CalendarDayId;
                    newEvent.CalendarEventId = 0;
                    _context.CalendarEvents.Add(newEvent);
                    await _context.SaveChangesAsync();
                }

            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.CalendarEventId }, calendarEvent);
        }

        // DELETE: api/CalendarEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarEvent(int id)
        {
            var calendarEvent = await _context.CalendarEvents.FindAsync(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }

            _context.CalendarEvents.Remove(calendarEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CalendarEventExists(int id)
        {
            return _context.CalendarEvents.Any(e => e.CalendarEventId == id);
        }
    }
}
