using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTag.Model
{
    [Table("Tags")]
	public abstract class TagBase : UserAddedItem
	{
		
		public virtual string Name { get; set; }

		[ForeignKey("Category")]
		public int? CategoryId { get; set; }

		public virtual TagCategory Category { get; set; }
	}

    public class Tag : TagBase
    {
        [RegularExpression(ModelConstant.RegEx.TagName)]
        public override string Name { get; set; }
    }
}
