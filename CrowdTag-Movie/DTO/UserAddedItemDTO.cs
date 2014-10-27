using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrowdTagMovie.DTO
{
	public class UserAddedItemDTO
	{
		public UserAddedItemDTO() { }

		public UserAddedItemDTO(UserAddedItem entity)
		{
			this.ID = entity.ID;
			this.CreatedDateTime = entity.CreatedDateTime;
			this.UpdatedDateTime = entity.UpdatedDateTime;

		}

		public int ID { get; set; }

		[DataType(DataType.Date), DisplayFormat(DataFormatString = ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? CreatedDateTime { get; set; }


		[DataType(DataType.Date), DisplayFormat(DataFormatString = ModelConstant.FormatString.Date, ApplyFormatInEditMode = true)]
		public DateTime? UpdatedDateTime { get; set; }


	}
}