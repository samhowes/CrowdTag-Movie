using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CrowdTag.DataAccess
{
	public class Repository<TEntity> /*: IRepository<TEntity>*/ where TEntity : class
	{
		protected TagDbContext _dbContext;
		protected DbSet<TEntity> dbSet;

		/*
		public Repository()
		{
			this._dbContext = new TagDbContext();
			this.dbSet = _dbContext.Set<TEntity>();
		}*/

		public Repository(TagDbContext dbContext)
		{
			this._dbContext = dbContext;
			this.dbSet = dbContext.Set<TEntity>();
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
			_dbContext.AttachIfUnattached(entityToDelete);
			dbSet.Remove(entityToDelete);
		}

		public virtual void Add(TEntity newEntity)
		{
			dbSet.Add(newEntity);
		}

		public virtual TEntity Update(object id)
		{
			TEntity entity = GetById(id);
			return entity;
		}
	}
}