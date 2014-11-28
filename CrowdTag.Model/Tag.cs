namespace CrowdTag.Model
{
	public class Tag : UserAddedItem
	{
		[RegularExpression(ModelConstant.RegEx.TagName)]
		public string Name { get; set; }

		[ForeignKey("Category")]
		public int? CategoryID { get; set; }

		public virtual TagCategory Category { get; set; }
	}
}
