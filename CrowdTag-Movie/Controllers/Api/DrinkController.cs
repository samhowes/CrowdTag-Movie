using CrowdTagMovie.DAL;
using CrowdTagMovie.DTO;
using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CrowdTagMovie.Controllers.Api
{
	public static class DrinksApi
	{
		public static class Main
		{
			public const string Name = "GetAllDrinks";
			public const string Route = "Drinks";
		}

		public static class SpecificItem
		{
			public const string Name = "GetDrinkById";
			public const string Route = Main.Route + "/{id:int:min(1)}";
		}

	}

	
	[RoutePrefix("api")]
    public class DrinkController : ApiController
    {

		private UnitOfWork UoW = new UnitOfWork();

		[HttpGet, Route(DrinksApi.Main.Route, Name=DrinksApi.Main.Name)]
		[ResponseType(typeof(List<TaggedItemDTO>))]
		public IHttpActionResult GetAllDrinks()
		{
			var entityList = UoW.TaggedItemRepository.Get();

			var dtoList = new List<TaggedItemDTO>();
			foreach (var entity in entityList)
			{
				dtoList.Add(new TaggedItemDTO(entity));
			}

			return Ok(dtoList);
		}

		[HttpGet, Route(DrinksApi.SpecificItem.Route, Name=DrinksApi.SpecificItem.Name)]
		[ResponseType(typeof(TaggedItemDTO))]
		public IHttpActionResult GetDrinkById(int id)
		{
			var entity = UoW.TaggedItemRepository.GetById(id);

			if (entity == null) return NotFound();

			return Ok(new TaggedItemDTO(entity));
		}


		[HttpPost, Route(DrinksApi.Main.Route)]
		public IHttpActionResult CreateNewDrink([FromBody]TaggedItemDTO dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var entity = new TaggedItem();
			dto.UpdateEntity(ref entity);
			UoW.TaggedItemRepository.Add(entity);
			UoW.Commit();

			return Created(Url.Link(DrinksApi.SpecificItem.Name, new { id = entity.ID }), new TaggedItemDTO(entity));
		}

		[HttpPut, Route(DrinksApi.SpecificItem.Route)]
		public IHttpActionResult UpdateDrink(int id, [FromBody]TaggedItemDTO dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			dto.ID = id;
			UoW.TaggedItemRepository.Update(dto);
			UoW.Commit();

			return Ok();
		}
    }
}
