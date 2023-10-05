using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoservisSystem.Shared.Models
{
    public class Car
    {
        //Generovanie Databazy -- vsetky properties nizsie
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int CarId { get; set; }
        public string CarEvidenceNumber { get; set; }
        public string VIN { get; set; }
        public bool Van { get; set; } = false;
        public string EngineNumber { get; set; }
        public string Brand { get; set; }
        public string ModelType { get; set; }
        public int EngigePowerInKW { get; set; }
        public int EngineVolume { get; set; }
        public int YearOfProduction { get; set; }
        public DateTime LastOilChange { get; set; }
        public DateTime LastSTK { get; set; }
        public bool AutomaticTransmission { get; set; } = false;
        public int CustomerId { get; set; }
        public virtual List<CalendarEvent> CalendarEvents { get; set; }


    }
}
