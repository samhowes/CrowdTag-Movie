using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdTag.Model
{
    public class IngredientApplication : UserAddedItem, IValidatableObject
    {
        public int IngredientId { get; set; }
        public virtual Ingredient Ingredient { get; set; }


        public int DrinkId { get; set; }
        public virtual Drink Drink { get; set; }

        public decimal? Amount { get; set; }

        [Required]
        public int MeasurementTypeId { get; set; }
        public virtual MeasurementType MeasurementType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MeasurementType != null && MeasurementType.HasAmount.Value && !Amount.HasValue)
            {
                yield return new ValidationResult(String.Format("MeasurementType {0} requires an amount that is not null", MeasurementType.Name), new [] {"Amount"});
            }
        }
    }
}
