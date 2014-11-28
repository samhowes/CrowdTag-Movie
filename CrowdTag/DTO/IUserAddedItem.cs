using System;

namespace CrowdTag.DTO
{
	interface IUserAddedItemDTO
	{
		DateTime? CreatedDateTime { get; set; }
		DateTime? UpdatedDateTime { get; set; }
	}
}
