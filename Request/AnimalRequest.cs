using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Request
{
    public class AnimalRequest
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public string admissionDate { get; set; }
        [Required]
        public int idOwner { get; set; }
    }
}
