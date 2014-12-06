using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CrowdTag.Model;

namespace CrowdTag.DataAccess
{
	public class UserAddedItemRepository<TEntity> : Repository<TEntity> where TEntity : UserAddedItem
	{
		//public UserAddedItemRepository() : base() { }

		public UserAddedItemRepository(TagDbContext movieDbContext) : base(movieDbContext)
		{}
		

		public override void Add(TEntity newEntity)
		{
			newEntity.SubmitterId = CrowdTagAuthorization.GetCurrentUserId();
			newEntity.CreatedDateTime = DateTime.Now;
			base.Add(newEntity);
		}

		public override TEntity Update(object id)
		{
			TEntity entity = base.Update(id);
			entity.UpdatedDateTime = DateTime.Now;
			return entity;
		}

			/*
		protected virtual UserAddedItem Update(object drinkId)
		{
			UserAddedItem entity = base.Update(drinkId)

			UserAddedItem entity = this.GetById(dto.ID);

			entity.UpdatedDateTime = DateTime.Now;

			dto.UpdateEntity(ref entity);

			return entity;
		}*/
	}

	public class TaggedItemRepository : UserAddedItemRepository<TaggedItem> 
	{
		public TaggedItemRepository(TagDbContext tagDbContext) : base(tagDbContext) { }

		public override IEnumerable<TaggedItem> Get(Func<IQueryable<TaggedItem>, IOrderedQueryable<TaggedItem>> orderByFunc = null, Expression<Func<TaggedItem, bool>> filter = null, params Expression<Func<TaggedItem, object>>[] includeExpressions)
		{
			var query = dbSet.AsQueryable()
						.Include(ti => ti.TagApplications.Select(ta => ta.Tag));

			return query.ToList();
		}

		/*public TaggedItem Update(TaggedItemDTO dto)
		{
			TaggedItem entity = (TaggedItem)base.Update(dto);

			dto.UpdateEntity(ref entity);

			return entity;
		}*/
	}
	
	public class TagCategoryRepository : UserAddedItemRepository<TagCategory>
	{
		public TagCategoryRepository(TagDbContext tagDbContext) : base(tagDbContext) { }

		public override IEnumerable<TagCategory> Get(Func<IQueryable<TagCategory>, IOrderedQueryable<TagCategory>> orderByFunc = null, Expression<Func<TagCategory, bool>> filter = null, params Expression<Func<TagCategory, object>>[] includeExpressions)
		{
			var query = dbSet.AsQueryable()
						.Include(tc => tc.Tags);

			return query.ToList();
		}
		/*
		public TagCategory Update(TagCategoryDTO dto)
		{
			TagCategory entity = (TagCategory)base.Update(dto);

			dto.UpdateEntity(ref entity);

			return entity;
		}*/
	}

	public class UserRepository : Repository<User>
	{
		public UserRepository(TagDbContext dbContext) : base(dbContext)
		{ }

		public override void Add(User newEntity)
		{
			newEntity.DateJoined = DateTime.Now;
			newEntity.LastActivity = DateTime.Now;
			//newEntity.UserRankID = 1;
			base.Add(newEntity);
		}
	}
}