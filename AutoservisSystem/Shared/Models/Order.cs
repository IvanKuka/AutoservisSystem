using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoservisSystem.Shared.Models;

namespace AutoservisSystem.Shared.Models
{
    public class Order
    {
        //Generovanie Databazy -- vsetky properties nizsie
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int OrderId { get; set; }
        public string CarEvidenceNumber { get; set; }
        public double Price { get; set; }
        public int InvoiceNumber { get; set; }
        public double Hours { get; set; }
        public bool Done { get; set; } = false;
        public bool Paid { get; set; } = false;
        public DateTime ReceivedDate { get; set; }
        public DateTime DoneDate { get; set; }
        public int Kilometres { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public virtual List<WorkTask> Tasks { get; set; }
        public virtual List<OrdersPart> OrdersParts { get; set; }
    }
}
