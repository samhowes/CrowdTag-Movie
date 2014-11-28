using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrowdTagMovie.Models
{
	public class TagCategory : UserAddedItem
	{
		[RegularExpression(ModelConstant.RegEx.TagName)]
		public string Name { get; set; }

		public string Description { get; set; }

		public virtual ICollection<Tag> Tags { get; set; }
	}
}