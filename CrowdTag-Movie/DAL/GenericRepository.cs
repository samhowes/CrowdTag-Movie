using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CrowdTagMovie.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CrowdTagMovie.DAL
{
	public class GenericRepository<TEntity> where TEntity : class
	{
		internal MovieContext context;
		internal DbSet<TEntity> dbSet;

		public GenericRepository(MovieContext context)
		{
			this.context = context;
		}
		//BOOKMARK
		public virtual async Task<IEnumerable<TEntity>> GetAsync(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = "")
		{

		}
	}
}