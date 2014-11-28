using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;

namespace CrowdTag.DataAccess
{
    public class TagContextProvider : EFContextProvider<TagDbContext>
    {
        public TagContextProvider() : base() {}

        protected override bool BeforeSaveEntity(EntityInfo entityInfo)
        {
            return base.BeforeSaveEntity(entityInfo);
        }

        protected override Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            // return a map of those entities we want saved.
            return saveMap;
        }
    }
}
