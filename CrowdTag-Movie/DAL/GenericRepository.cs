using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CrowdTagMovie.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PagedList;

namespace CrowdTagMovie.DAL
{
	public class GenericPagedRepository<TEntity> where TEntity : class
	{
		internal MovieContext context;
		internal DbSet<TEntity> dbSet;

		protected int pageSize;
		protected int pageNumber;

		// Bookmark!
		public void SetPaging(int pageSize, int pageNumber)
		{
			this.pageSize = pageSize;
			this.pageNumber = pageNumber;
		}

		public GenericPagedRepository(MovieContext context)
		{
			this.context = context;
			this.dbSet = context.Set<TEntity>();
			SetPaging(4, 1);
		}

		public virtual async Task<IPagedList> GetAsync(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc,
			Expression<Func<TEntity, bool>> filter = null,
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

			query = orderByFunc(query);

			return await Task.Run<IPagedList>(() =>
				{
					return query.ToPagedList(this.pageNumber, this.pageSize);
				});

		}
	}
}