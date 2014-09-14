using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrowdTagMovie.DTO
{
	public class TagDTO : UserAddedItemDTO
	{
		public TagDTO() : base() { }

		public TagDTO(Tag entity) : base(entity)
		{
			this.Name = entity.Name;
		}

		public string Name { get; set; }

		public TagCategoryDTO Category {get; set;}


	}

}