using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrowdTagMovie.DTO
{
    public class TagDTO : UserAddedItemDTO
    {
        public string Name { get; set; }

        public int? TaggedItemId { get; set; }

        public int? CategoryId { get; set; }

        public TagDTO() : base() { }

        public TagDTO(TagApplication entity) : this(entity.Tag)
        {
            this.TaggedItemId = entity.TaggedItemID;
            this.Id = entity.ID;
        }

        public TagDTO(Tag entity)
            : base(entity)
        {
            this.Name = entity.Name;
            this.CategoryId = entity.CategoryID;
        }



        public new void UpdateEntity(ref Tag entity)
        {
            entity.Name = this.Name;
        }

    }
}