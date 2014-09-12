using CrowdTagMovie.DAL;
using CrowdTagMovie.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace CrowdTagMovie.Controllers.Api
{
	public static class ApiRoutes
	{
		public const string ApiPrefix = "api";
		public static class Drinks
		{
			public const string Main = "Drinks";
			public const string SpecificItem = Main + "/{drinkId:int:min(1)}";
		}
	}
	
	[RoutePrefix(ApiRoutes.ApiPrefix)]
    public class DrinkController : ApiController
    {

		private UnitOfWork UoW = new UnitOfWork();

		[Route(ApiRoutes.Drinks.Main)]
		[ResponseType(typeof(TaggedItemDTO))]
		public IHttpActionResult GetAllDrinks()
		{
			var entityList = UoW.TaggedItemRepository.Get();

			var dtoList = new List<TaggedItemDTO>();
			foreach (var entity in entityList)
			{
				dtoList.Add(TaggedItemDTO.CreateFromEntity(entity));
			}

			return Ok(dtoList);
		}

    }
}
