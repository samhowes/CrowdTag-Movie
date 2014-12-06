using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdTag.Model
{

    public class Drink : TaggedItem
    {
        public Drink()
        {
            Recipe = new HashSet<IngredientApplication>();
        }

        public virtual ICollection<IngredientApplication> Recipe { get; set; }
    }

    public class Drink2
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<TagApplication> TagApplications { get; set; }

        public virtual ICollection<IngredientApplication> Recipe { get; set; } 
    }
}
