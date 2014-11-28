using System.ComponentModel.DataAnnotations;
using CrowdTag.Model;

namespace CrowdTag.DTO
{
	public class IngredientDTO : TagDTO
	{
		[Required]
		public decimal? Amount { get; set; }

		[Required]
		public MeasurementTypeEnum? MeasurementType { get; set; }


		public IngredientDTO(IngredientTagApplication entity) : base(entity)
		{
			this.Amount = entity.Amount;
			this.MeasurementType = entity.MeasurementType;
		}

		public IngredientDTO() : base() { }

		public void UpdateEntity(ref IngredientTagApplication entity) 
		{
			entity.Amount = this.Amount;
			entity.MeasurementType = this.MeasurementType;
		}

	}
}
