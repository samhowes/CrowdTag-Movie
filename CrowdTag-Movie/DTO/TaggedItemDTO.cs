using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrowdTagMovie.DTO
{
	public class TaggedItemDTO : UserAddedItemDTO
	{
		public TaggedItemDTO() : base() { }

		public TaggedItemDTO(TaggedItem entity) : base(entity)
		{
			this.Name = entity.Name;
			this.Description = entity.Description;
			
			if (entity.TagApplications != null)
			{
				this.Tags = new List<TagDTO>();
				foreach (var tagApp in entity.TagApplications)
				{
					this.Tags.Add(new TagDTO(tagApp.Tag));
				}
			}
			
		}


		[Required]
		[StringLength(255)]
		[RegularExpression(ModelConstant.RegEx.ItemName, ErrorMessage = "Name must not contain special characters")]
		public string Name { get; set; }


		[StringLength(ModelConstant.StringLength.FreeText, MinimumLength = 3, ErrorMessage = "Description must be at least 3 characters")]
		public string Description { get; set; }

		public List<TagDTO> Tags { get; set; }

		public TaggedItem ToEntity()
		{
			return new TaggedItem
			{
				Name = this.Name,
				Description = this.Description
			};
		}

		public void UpdateEntity(ref TaggedItem entity)
		{
			entity.Name = this.Name;
			entity.Description = this.Description;
		}
	}
}