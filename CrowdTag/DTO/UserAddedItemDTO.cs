using System;
using System.ComponentModel.DataAnnotations;
using CrowdTag.Model;

namespace CrowdTag.DTO
{
	public class UserAddedItemDTO
	{
		public UserAddedItemDTO() { }

		public UserAddedItemDTO(UserAddedItem entity)
		{
			this.Id = entity.ID;
			this.CreatedDateTime = entity.CreatedDateTime;
			this.UpdatedDateTime = entity.UpdatedDateTime;

		}

		public int Id { get; set; }

		[DataType(DataType.Date), DisplayFormat(DataFormatString = ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? CreatedDateTime { get; set; }


		[DataType(DataType.Date), DisplayFormat(DataFormatString = ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? UpdatedDateTime { get; set; }


	}
}