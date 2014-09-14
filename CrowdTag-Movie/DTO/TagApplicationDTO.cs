using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrowdTagMovie.DTO
{
	public class TagApplicationDTO : UserAddedItemDTO
	{
		public TagApplicationDTO() : base() { }

		public TagApplicationDTO(TagApplication entity) : base(entity)
		{
			throw new NotImplementedException();
		}



	}
}
