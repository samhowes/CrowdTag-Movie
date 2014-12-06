using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTag.Model
{
    [Table("TagApplications")]
    public class TagApplication : UserAddedItem
    {
        //[Required]
        //public int Score { get; set; }

        [Required]
        [ForeignKey("TaggedItem")]
        public int? TaggedItemId { get; set; }

        [Required]
        [ForeignKey("Tag")]
        public int? TagId { get; set; }

        public virtual TaggedItem TaggedItem { get; set; }
        public virtual Tag Tag { get; set; }
        //public virtual ICollection<Vote> Votes { get; set; }
    }
}
