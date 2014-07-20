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
	public class PagedRepository<TEntity> : IPagedRepository<TEntity> where TEntity : class
	{
		internal MovieContext _context;
		internal DbSet<TEntity> dbSet;

		protected static int pageSize = 20;
		protected static int pageNumber = 20;

		public void SetPaging(int pageNumber, int pageSize)
		{
			PagedRepository<TEntity>.pageSize = pageSize;
			PagedRepository<TEntity>.pageNumber = pageNumber;
		}

		public PagedRepository(MovieContext context, int pageNumber = 1, int pageSize = 20)
		{
			this._context = context;
			this.dbSet = context.Set<TEntity>();
			SetPaging(pageNumber, pageSize);
		}

		private IPagedList QueryToPagedList(IQueryable<TEntity> query)
		{
			return query.ToPagedList(PagedRepository<TEntity>.pageNumber, PagedRepository<TEntity>.pageSize);
		}

		public virtual IPagedList Get(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc,
			Expression<Func<TEntity, bool>> filter = null,
			params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			IQueryable<TEntity> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			if (includeExpressions.Any())
			{
				query = includeExpressions.Aggregate(query, (current, expression) => current.Include(expression));
			}

			query = orderByFunc(query);

			return QueryToPagedList(query);
		}

		public virtual TEntity GetById(object id)
		{
			return dbSet.Find(id);
		}

		public virtual void DeleteById(object id)
		{
			TEntity entityToDelete = dbSet.Find(id);
			Delete(entityToDelete);
		}

		public virtual void Delete(TEntity entityToDelete)
		{
			if (_context.Entry(entityToDelete).State == EntityState.Detached)
			{
				dbSet.Attach(entityToDelete);
			}
			dbSet.Remove(entityToDelete);
		}

		public virtual void Add(TEntity newEntity)
		{
			dbSet.Add(newEntity);
		}

		public virtual void Update(TEntity editedEntity)
		{
			_context.Entry(editedEntity).State = EntityState.Modified;
		}

		
	}
}