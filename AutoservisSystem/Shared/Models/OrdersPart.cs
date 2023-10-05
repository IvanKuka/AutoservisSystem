using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoservisSystem.Shared.Models
{
    public class OrdersPart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int OrdersPartId { get; set; }
        public double Pieces { get; set; }
        public int OrderId { get; set; }
        public int PartId { get; set; }
        public double Price { get; set; }
    }
}
