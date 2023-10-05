using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoservisSystem.Shared.Models
{
    public class CalendarDay
    {
        //Generovanie Databazy -- vsetky properties nizsie
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CalendarDayId { get; set; }
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }
        public bool IsEmpty { get; set; }
        public List<CalendarEvent> Events { get; set; }
    }
}
