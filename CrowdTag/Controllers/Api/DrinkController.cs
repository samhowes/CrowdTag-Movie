using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using CrowdTagMovie.DAL;
using CrowdTagMovie.DTO;
using CrowdTagMovie.Models;
using CrowdTagMovie.Services;

namespace CrowdTagMovie.Controllers.Api
{
    public class ApiRoute
    {
        public ApiRoute() {}

        public ApiRoute(string name, string route)
        {
            this.Name = name;
            this.Route = route;
        }

        public string Name {get; set;}
        public string Route {get; set;}
    }
    
    public static class DrinksApi
    {
        public const string IdConstraint = ":int:min(1)";

        public static class Drinks
        {
            public static class Main
            {
                public const string Name = "GetAllDrinks";
                public const string Route = "Drinks";
            }

            public static class SpecificItem
            {
                public const string Name = "GetDrinkById";
                public const string Route = Main.Route + "/{drinkId" + IdConstraint + "}";
            }
        }

        public static class Ingredients
        {
            public static class Main
            {
                public const string Route = Drinks.SpecificItem.Route + "/Ingredients";
            }

        }

        public static class ApplyTags
        {
            public static class Main
            {
                public const string Route = Drinks.SpecificItem.Route + "/Tags";
            }

        }

        public static class TagCategories
        {
            public static class Main
            {
                public const string Name = "GetAllTagCategories";
                public const string Route = "TagCategories";
            }

            public static class SpecificItem
            {
                public const string Name = "GetTagCategoryById";
                public const string Route = Main.Route + "/{categoryId" + IdConstraint + "}";
            }
        }

        public static class Tags
        {
            public static class Main
            {
                public const string Name = "GetAllTags";
                public const string Route = TagCategories.SpecificItem.Route + "/Tags";
            }

            public static class SpecificItem
            {
                public const string Name = "GetTagById";
                public const string Route = Main.Route + "/{tagId" + IdConstraint + "}";
            }
        }
    }

    
    [RoutePrefix("api")]
    public class DrinkController : ApiController
    {

        private UnitOfWork dataContext = new UnitOfWork();

        //===========================================================================================================
        // Drink CRUD of Basic Info
        //===========================================================================================================

        [HttpGet, Route(DrinksApi.Drinks.Main.Route, Name=DrinksApi.Drinks.Main.Name)]
        [ResponseType(typeof(List<TaggedItemDTO>))]
        public IHttpActionResult GetAllDrinks()
        {
            var entityList = dataContext.TaggedItemRepository.Get();

            var dtoList = new List<TaggedItemDTO>();
            foreach (var entity in entityList)
            {
                dtoList.Add(new TaggedItemDTO(entity));
            }

            return Ok(dtoList);
        }

        [HttpGet, Route(DrinksApi.Drinks.SpecificItem.Route, Name=DrinksApi.Drinks.SpecificItem.Name)]
        [ResponseType(typeof(TaggedItemDTO))]
        public IHttpActionResult GetDrinkById(int drinkId)
        {
            var entity = dataContext.TaggedItemRepository.GetById(drinkId);

            if (entity == null) return NotFound();

            return Ok(new TaggedItemDTO(entity));
        }


        [HttpPost, Route(DrinksApi.Drinks.Main.Route)]
        public IHttpActionResult CreateNewDrink([FromBody]TaggedItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = new TaggedItem();
            dto.UpdateEntity(ref entity);
            dataContext.TaggedItemRepository.Add(entity);
            dataContext.SaveChanges();

            return Created(Url.Link(DrinksApi.Drinks.SpecificItem.Name, new { drinkId = entity.ID }), new TaggedItemDTO(entity));
        }

        [HttpPut, Route(DrinksApi.Drinks.SpecificItem.Route)]
        public IHttpActionResult UpdateDrink(int drinkId, [FromBody]TaggedItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            TaggedItem entity = dataContext.TaggedItemRepository.Update(drinkId);
            dto.UpdateEntity(ref entity);
            dataContext.SaveChanges();

            return Ok();
        }

        //===========================================================================================================
        // Apply Tag to a Drink
        //===========================================================================================================

        [HttpPost, Route(DrinksApi.ApplyTags.Main.Route)] 
        public IHttpActionResult ApplyTagToDrink(int drinkId, [FromBody]int tagId)
        {
            if (tagId <= 0) return BadRequest("TagId must be a valid Id.");

            var entity = new TagApplication { TaggedItemID = drinkId, TagID = tagId };
            dataContext.TagApplicationRepository.Add(entity);
            dataContext.SaveChanges();

            return Created(Url.Link(DrinksApi.Drinks.SpecificItem.Name, new { drinkId = drinkId }), (TagDTO)null);
        }

        [HttpPost, Route(DrinksApi.Ingredients.Main.Route)]
        public IHttpActionResult AddIngredientToDrink(int drinkId, [FromBody]IngredientDTO newIngredient)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var drinkMixer = new DrinkMixer(dataContext);
            var entity = drinkMixer.AddIngredientToDrink(drinkId, newIngredient);

            dataContext.SaveChanges();

            return Ok();
        }
        
        //===========================================================================================================
        // Tag Category CRUD
        //===========================================================================================================

        [HttpGet, Route(DrinksApi.TagCategories.Main.Route, Name = DrinksApi.TagCategories.Main.Name)]
        [ResponseType(typeof(List<TagCategoryDTO>))]
        public IHttpActionResult GetAllTagCategories()
        {
            var entityList = dataContext.TagCategoryRepository.Get();

            var dtoList = new List<TagCategoryDTO>();
            foreach (var entity in entityList)
            {
                dtoList.Add(new TagCategoryDTO(entity));
            }

            return Ok(dtoList);
        }

        [HttpGet, Route(DrinksApi.TagCategories.SpecificItem.Route, Name = DrinksApi.TagCategories.SpecificItem.Name)]
        [ResponseType(typeof(TagCategoryDTO))]
        public IHttpActionResult GetTagCategoryById(int categoryId)
        {
            var entity = dataContext.TagCategoryRepository.GetById(categoryId);

            if (entity == null) return NotFound();

            return Ok(new TagCategoryDTO(entity));
        }


        [HttpPost, Route(DrinksApi.TagCategories.Main.Route)]
        public IHttpActionResult CreateNewTagCategory([FromBody]TagCategoryDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = new TagCategory();
            dto.UpdateEntity(ref entity);
            dataContext.TagCategoryRepository.Add(entity);
            dataContext.SaveChanges();

            return Created(Url.Link(DrinksApi.TagCategories.SpecificItem.Name, new { categoryId = entity.ID }), new TagCategoryDTO(entity));
        }

        [HttpPut, Route(DrinksApi.TagCategories.SpecificItem.Route)]
        public IHttpActionResult UpdateTagCategory(int categoryId, [FromBody]TagCategoryDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = dataContext.TagCategoryRepository.Update(categoryId);
            dto.UpdateEntity(ref entity);
            dataContext.SaveChanges();

            return Ok();
        }

        //===========================================================================================================
        // Tag CRUD
        //===========================================================================================================
        
        [HttpGet, Route(DrinksApi.Tags.Main.Route, Name = DrinksApi.Tags.Main.Name)]
        [ResponseType(typeof(List<TagDTO>))]
        public IHttpActionResult GetAllTags(int categoryId)
        {
            var entityList = dataContext.TagRepository.Get();

            var dtoList = new List<TagDTO>();
            foreach (var entity in entityList)
            {
                dtoList.Add(new TagDTO(entity));
            }

            return Ok(dtoList);
        }

        [HttpGet, Route(DrinksApi.Tags.SpecificItem.Route, Name = DrinksApi.Tags.SpecificItem.Name)]
        [ResponseType(typeof(TagDTO))]
        public IHttpActionResult GetTagById(int categoryId, int tagId)
        {
            var entity = dataContext.TagRepository.GetById(tagId);

            if (entity == null) return NotFound();

            return Ok(new TagDTO(entity));
        }

        
        [HttpPost, Route(DrinksApi.Tags.Main.Route)]
        public IHttpActionResult CreateNewTag(int categoryId, [FromBody]TagDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = new Tag();
            dto.UpdateEntity(ref entity);
            
            entity.CategoryID = categoryId;

            dataContext.TagRepository.Add(entity);
            dataContext.SaveChanges();

            return Created(Url.Link(DrinksApi.Tags.SpecificItem.Name, new { tagId = entity.ID }), new TagDTO(entity));
        }

        [HttpPut, Route(DrinksApi.Tags.SpecificItem.Route)]
        public IHttpActionResult UpdateTag(int categoryId, int tagId, [FromBody]TagDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            Tag entity = dataContext.TagRepository.Update(tagId);
            dto.UpdateEntity(ref entity);

            dataContext.SaveChanges();
            return Ok();
        }
        
    }
}
