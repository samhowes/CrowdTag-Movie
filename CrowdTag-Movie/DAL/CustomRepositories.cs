using System;
using System.Collections.Generic;
using System.Linq;
using CrowdTagMovie.Models;

namespace CrowdTagMovie.DAL
{
	public class UserAddedItemRepository<TEntity> : Repository<TEntity> where TEntity : UserAddedItem
	{
		//public UserAddedItemRepository() : base() { }

		public UserAddedItemRepository(IMovieContext movieContext) : base(movieContext)
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

	public class UserRepository : Repository<User>
	{
		public UserRepository(IMovieContext context) : base(context)
		{ }

		public override void Add(User newEntity)
		{
			newEntity.DateJoined = DateTime.Now;
			newEntity.LastActivity = DateTime.Now;
			newEntity.UserRankID = 1;
			base.Add(newEntity);
		}
	}
}