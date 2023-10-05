using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoservisSystem.Shared.Models
{
    public class WorkTask

    {
        //Generovanie Databazy -- vsetky properties nizsie
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int TaskId { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; } = false;
        public int OrderId { get; set; }
    }
}
