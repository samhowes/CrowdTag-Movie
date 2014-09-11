using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTagMovie.Models
{
	public class TaggedItem : UserAddedItem
	{
		
		[Required]
		[StringLength(255)]
		[RegularExpression(@"^[\'\-\:\s\w,]*$", ErrorMessage="Name must not contain special characters")]
		public string Name { get; set; }


		[StringLength(ModelConstant.StringLength.FreeText, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters")]
		public string Description { get; set; }


		public virtual ICollection<TagApplication> TagApplications { get; set; }
		
	}
}