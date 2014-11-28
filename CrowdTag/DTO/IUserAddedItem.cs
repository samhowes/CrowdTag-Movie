using System;

namespace CrowdTagMovie.DTO
{
	interface IUserAddedItemDTO
	{
		DateTime? CreatedDateTime { get; set; }
		DateTime? UpdatedDateTime { get; set; }
	}
}
