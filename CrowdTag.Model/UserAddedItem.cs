using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTag.Model
{
    
	public interface IUserAddedItemDTO
	{
		DateTime? CreatedDateTime { get; set; }
		DateTime? UpdatedDateTime { get; set; }
	}


	public abstract class UserAddedItem : IUserAddedItemDTO
	{
		[Key]
		public int ID { get; set; }


		[Required]
		[ForeignKey("Submitter")]
		[MaxLength(ModelConstant.StringLength.GUID)]
		public String SubmitterID { get; set; }


		[Required]
		[DataType(DataType.Date), DisplayFormat(DataFormatString=ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? CreatedDateTime { get; set; }

		
		[DataType(DataType.Date), DisplayFormat(DataFormatString = ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? UpdatedDateTime { get; set; }


		public virtual User Submitter { get; set; }

	}
}