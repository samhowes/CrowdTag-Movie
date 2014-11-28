using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTagMovie.Models
{
	public class Movie
	{
		public int MovieID { get; set; }

		[Required]
		[DataType(DataType.Date), DisplayFormat(DataFormatString="{0:MM/dd/yyyy}", ApplyFormatInEditMode=true)]
		public DateTime DateAdded { get; set; }

		[Required, StringLength(255, MinimumLength = 3)]
		[RegularExpression(@"^[a-zA-Z''-':\s\w,]*$", ErrorMessage="Title must not contain special characters")]
		public string Title { get; set; }

		[Required]
		[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime ReleaseDate { get; set; }

		[StringLength(1000, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters")]
		[RegularExpression(@"^[a-zA-Z'\-:;\.,\\ \( \)\s\w]*$", ErrorMessage = "Description must not contain special characters")]
		public string Description { get; set; }

		[StringLength(100)]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
		public string Director { get; set; }

		/*
		[Required]
		[StringLength(128), ForeignKey("Submitter")]
		public string SubmitterID { get; set; }

		public virtual Profile Submitter { get; set; }
		public virtual ICollection<TagApplication> TagApplications { get; set; }
		 */

	}
}