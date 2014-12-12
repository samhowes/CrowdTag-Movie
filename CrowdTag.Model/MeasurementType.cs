using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdTag.Model
{
    public class MeasurementType : UserAddedItem
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public bool? HasAmount { get; set; } 
    }
}
