using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace CrowdTagMovie.DTO
{
	public class TagCategoryDTO : UserAddedItemDTO
	{
		public TagCategoryDTO() : base() { }

		public TagCategoryDTO(TagCategory entity) : base(entity)
		{
			this.Name = entity.Name;
			this.Description = entity.Description;
		}

		[RegularExpression(ModelConstant.RegEx.TagName)]
		public string Name { get; set; }

		public string Description { get; set; }
	}
}
