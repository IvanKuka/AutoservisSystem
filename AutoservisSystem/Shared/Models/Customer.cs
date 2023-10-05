using AutoservisSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoservisSystem.Shared.Models
{
    public class Customer
    {
    
        //Generovanie Databazy -- vsetky properties nizsie
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public virtual List<Order> Orders { get; set; }
        public virtual List<Car> Cars { get; set; }
        public virtual List<CalendarEvent> CalendarEvents { get; set; }
    }
}
