using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrowdTagMovie.Models
{
	public abstract class UserAddedItem
	{
		[Key]
		public int ID { get; set; }


		[Required]
		[ForeignKey("Submitter")]
		[MinLength(ModelConstant.StringLength.GUID), MaxLength(ModelConstant.StringLength.GUID)]
		public String SubmitterID { get; set; }


		[Required]
		[DataType(DataType.Date), DisplayFormat(DataFormatString=ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? CreatedDateTime { get; set; }

		[Required]
		[DataType(DataType.Date), DisplayFormat(DataFormatString = ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? UpdatedDateTime { get; set; }


		public virtual User Submitter { get; set; }

	}
}