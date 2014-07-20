﻿using System;
using System.Collections.Generic;
using System.Linq;
using CrowdTagMovie.Models;

namespace CrowdTagMovie.DAL
{
	public class UserAddedItemRepository : PagedRepository<UserAddedItem>
	{
		public override void Add(UserAddedItem newEntity)
		{
			newEntity.CreatedDateTime = DateTime.Now;
			base.Add(newEntity);
		}

		public override void Update(UserAddedItem editedEntity)
		{
			editedEntity.UpdatedDateTime = DateTime.Now;
			base.Update(editedEntity);
		}
	}

	public class UserRepository : PagedRepository<User>
	{
		public override void Add(User newEntity)
		{
			newEntity.DateJoined = DateTime.Now;
			newEntity.LastActivity = DateTime.Now;
			newEntity.UserRankID = 1;
			base.Add(newEntity);
		}
	}
}