using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTagMovie.Models
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
}
