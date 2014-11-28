using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CrowdTagMovie.Models;

namespace CrowdTagMovie.DTO
{
	public class TagCategoryDTO : UserAddedItemDTO
	{
		public TagCategoryDTO() : base() { }


		[RegularExpression(ModelConstant.RegEx.TagName)]
		public string Name { get; set; }

		public string Description { get; set; }

		public List<TagDTO> Tags { get; set; }

		public TagCategoryDTO(TagCategory entity) : base(entity)
		{
			this.Name = entity.Name;
			this.Description = entity.Description;
			
			if (entity.Tags != null)
			{
				this.Tags = new List<TagDTO>();
			
				foreach (var tag in entity.Tags)
				{
					this.Tags.Add(new TagDTO(tag));
				}
			}
			
		}

		public void UpdateEntity(ref TagCategory entity)
		{
			entity.Name = this.Name;
			entity.Description = this.Description;
			
		}
	}
}
