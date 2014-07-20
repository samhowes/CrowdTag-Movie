using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTagMovie.Models
{
	public class TagApplication : UserAddedItem
	{
		[Required]
		public int Score { get; set; }

		[Required]
		[ForeignKey("Movie")]
		public int? MovieID { get; set; }

		[Required]
		[ForeignKey("Tag")]
		public int? TagID { get; set; }

		public virtual Movie Movie { get; set; }
		public virtual Tag Tag { get; set; }
		public virtual ICollection<Vote> Votes { get; set; }

	}
}
