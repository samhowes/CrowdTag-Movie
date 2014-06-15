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
			this.dbSet = context.Set<TEntity>();
		}

		public virtual async Task<IEnumerable<TEntity>> GetAsync(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			IQueryable<TEntity> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			/* from: http://www.appetere.com/Blogs/SteveM/May-2012/Passing-Include-statements-into-a-Repository
			public IQueryable<Order> GetAll(params Expression<Func<Order, object>>[] includeExpressions)
			{
			  return includeExpressions.Aggregate<Expression<Func<Movie, object>>, IQueryable<Movie>>
			   (_context.Orders, (current, expression) => current.Include(expression));
			}

			*/

			// TODO: Test this section!
			if (includeExpressions.Any())
			{
				query = includeExpressions.Aggregate(query, (current, expression) => current.Include(expression));
			}
 
			if (orderBy != null)
			{
				return await orderBy(query).ToListAsync();
			}
			else
			{
				return await query.ToListAsync();
			}

		}
	}
}