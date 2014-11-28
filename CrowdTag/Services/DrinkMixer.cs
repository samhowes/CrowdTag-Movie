using CrowdTag.DataAccess;
using CrowdTag.DTO;
using CrowdTag.Model;

namespace CrowdTag.Services
{
    public class DrinkMixer
    {
        private UnitOfWork _dataContext;

        public DrinkMixer(UnitOfWork dataContext)
        {
            this._dataContext = dataContext;
        }

        public IngredientTagApplication AddIngredientToDrink(int drinkId, IngredientDTO ingredient)
        {
            var drink = _dataContext.TaggedItemRepository.GetById(drinkId);
            var tag = _dataContext.TagRepository.GetById(ingredient.Id);

            if (drink == null) throw new DbItemNotFoundException(typeof (TaggedItem), drinkId);

            var newEntity = new IngredientTagApplication {TaggedItemID = drinkId,TagID = ingredient.Id};
            ingredient.UpdateEntity(ref newEntity);
            _dataContext.TagApplicationRepository.Add(newEntity);

            return newEntity;
        }
    }
}