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
		[Required]
		[StringLength(255)]
		[RegularExpression(@"^[\'\-\:\s\w,]*$", ErrorMessage = "Name must not contain special characters")]
		public string Name { get; set; }


		[StringLength(ModelConstant.StringLength.FreeText, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters")]
		public string Description { get; set; }


		public TaggedItem ToEntity()
		{
			return new TaggedItem
			{
				Name = Name,
				//ReleaseDate = ReleaseDate,
				Description = Description,
				//Director = Director
			};
		}

		public static TaggedItemDTO CreateFromEntity(TaggedItem entity)
		{
			TaggedItemDTO dto = UserAddedItemDTO.CreateFromEntity(entity) as TaggedItemDTO;
			
			dto.Name = entity.Name;
			dto.Description = entity.Description;

			return dto;
		}
	}
}