using System.Collections.Generic;

namespace CrowdTag.Model
{
	public class TaggedItem : UserAddedItem
	{
		
		[Required]
		[StringLength(255)]
		[RegularExpression(ModelConstant.RegEx.ItemName, ErrorMessage="Name must not contain special characters")]
		public string Name { get; set; }


		[Required,StringLength(ModelConstant.StringLength.FreeText, ErrorMessage = "Description must be at least 3 characters")]
		public string Description { get; set; }


		public virtual ICollection<TagApplication> TagApplications { get; set; }
		
	}
}