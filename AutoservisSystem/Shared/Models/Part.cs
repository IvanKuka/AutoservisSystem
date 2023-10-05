using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoservisSystem.Shared.Models
{
    public class Part
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int PartId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Supplier { get; set; }
        public string Brand { get; set; }
        public string PartNumber { get; set; }
        public virtual List<OrdersPart> OrdersParts { get; set; }
        public string Unit { get; set; }
    }
}
