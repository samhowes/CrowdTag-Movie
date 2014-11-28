using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using CrowdTag.DataAccess;
using CrowdTag.Model;
using Newtonsoft.Json.Linq;

namespace CrowdTag.Controllers.Api
{
    [BreezeController]
    [RoutePrefix("api/breeze")]
    public class BreezeDrinksController : ApiController
    {
        //TODO: Dependency Injection
        readonly TagContextProvider _contextProvider = new TagContextProvider();

        [HttpGet]
        public string MetaData()
        {
            return _contextProvider.Metadata();
        }

        [HttpGet, Route("Drinks")]
        public IQueryable<TaggedItem> Drinks()
        {
            return _contextProvider.Context.Drinks;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

    }
}