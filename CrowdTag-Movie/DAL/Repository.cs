using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CrowdTagMovie.DAL
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		protected TagContext _context;
		protected DbSet<TEntity> dbSet;

		/*
		public Repository()
		{
			this._context = new TagContext();
			this.dbSet = _context.Set<TEntity>();
		}*/

		public Repository(TagContext context)
		{
			this._context = context;
			this.dbSet = context.Set<TEntity>();
		}

		public virtual IEnumerable<TEntity> Get(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc = null,
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

			if (orderByFunc != null)
			{
				query = orderByFunc(query);
			}


			return query.ToList();
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
			_context.AttachIfUnattached(entityToDelete);
			dbSet.Remove(entityToDelete);
		}

		public virtual void Add(TEntity newEntity)
		{
			dbSet.Add(newEntity);
		}

		public virtual void Update(TEntity editedEntity)
		{
			_context.MarkAsModified(editedEntity);
		}
	}
}