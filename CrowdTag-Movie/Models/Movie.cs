using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTagMovie.Models
{
	public class Movie : UserAddedItem
	{
		
		[Required]
		[StringLength(255)]
		[RegularExpression(@"^[\'\-\:\s\w,]*$", ErrorMessage="Title must not contain special characters")]
		public string Title { get; set; }


		[Required]
		[DataType(DataType.Date), DisplayFormat(DataFormatString=ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? ReleaseDate { get; set; }


		[StringLength(ModelConstant.StringLength.FreeText, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters")]
		public string Description { get; set; }


		[StringLength(100)]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
		public string Director { get; set; }

		
		public virtual ICollection<TagApplication> TagApplications { get; set; }
		

	}
}