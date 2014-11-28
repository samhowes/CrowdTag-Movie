namespace CrowdTag.Model
{
	public class TagApplication : UserAddedItem
	{
		//[Required]
		//public int Score { get; set; }

		[Required]
		[ForeignKey("TaggedItem")]
		public int? TaggedItemID { get; set; }

		[Required]
		[ForeignKey("Tag")]
		public int? TagID { get; set; }

		public virtual TaggedItem TaggedItem { get; set; }
		public virtual Tag Tag { get; set; }
		//public virtual ICollection<Vote> Votes { get; set; }

	}

	public enum MeasurementTypeEnum 
	{
		Ounce = 1,
		Dash = 2,
		Fill = 3
	}

	public class IngredientTagApplication : TagApplication
	{
		[Required]
		public decimal? Amount { get; set; }

		[Required]
		public MeasurementTypeEnum? MeasurementType { get; set; }


	}

}
