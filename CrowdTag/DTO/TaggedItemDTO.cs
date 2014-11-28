using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CrowdTag.Model;

namespace CrowdTag.DTO
{
    public class TaggedItemDTO : UserAddedItemDTO
    {
        
        [Required]
        [StringLength(255)]
        [RegularExpression(ModelConstant.RegEx.ItemName, ErrorMessage = "Name must not contain special characters")]
        public string Name { get; set; }


        [StringLength(ModelConstant.StringLength.FreeText, MinimumLength = 3, ErrorMessage = "Description must be at least 3 characters")]
        public string Description { get; set; }

        public List<TagDTO> Tags { get; set; }

        public List<IngredientDTO> Recipe { get; set; }



        public TaggedItemDTO() : base() { }

        public TaggedItemDTO(TaggedItem entity)
            : base(entity)
        {
            this.Name = entity.Name;
            this.Description = entity.Description;

            if (entity.TagApplications == null) return;
            
            this.Tags = new List<TagDTO>();
            this.Recipe = new List<IngredientDTO>();
            foreach (var tagApp in entity.TagApplications)
            {
                if (tagApp is IngredientTagApplication)
                {
                    this.Recipe.Add(new IngredientDTO(tagApp as IngredientTagApplication));
                }
                else
                {
                    this.Tags.Add(new TagDTO(tagApp));
                }

            }
        }


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