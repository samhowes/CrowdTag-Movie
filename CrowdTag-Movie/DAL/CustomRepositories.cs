using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using CrowdTagMovie.Models;
using System.Linq.Expressions;

namespace CrowdTagMovie.DAL
{
	public class UserAddedItemRepository<TEntity> : Repository<TEntity> where TEntity : UserAddedItem
	{
		//public UserAddedItemRepository() : base() { }

		public UserAddedItemRepository(TagContext movieContext) : base(movieContext)
		{}
		

		public override void Add(TEntity newEntity)
		{
			newEntity.CreatedDateTime = DateTime.Now;
			base.Add(newEntity);
		}

		public override void Update(TEntity editedEntity)
		{
			editedEntity.UpdatedDateTime = DateTime.Now;
			base.Update(editedEntity);
		}
	}

	public class TaggedItemRepository : UserAddedItemRepository<TaggedItem> 
	{
		public TaggedItemRepository(TagContext tagContext) : base(tagContext) { }

		public override IEnumerable<TaggedItem> Get(Func<IQueryable<TaggedItem>, IOrderedQueryable<TaggedItem>> orderByFunc = null, Expression<Func<TaggedItem, bool>> filter = null, params Expression<Func<TaggedItem, object>>[] includeExpressions)
		{
			var query = dbSet.AsQueryable()
						.Include(ti => ti.TagApplications.Select(ta => ta.Tag));

			return query.ToList();
		}
	}

	public class UserRepository : Repository<User>
	{
		public UserRepository(TagContext context) : base(context)
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