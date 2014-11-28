using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTagMovie.Models
{
	public class Vote : UserAddedItem
	{
		[Required]
		public bool IsUpvote { get; set; }

		[Required]
		[ForeignKey("TagApplication")]
		public int? TagApplicationID { get; set; }

		public virtual TagApplication TagApplication { get; set; }
	}
}
