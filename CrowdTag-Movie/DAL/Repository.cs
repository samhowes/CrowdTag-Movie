﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CrowdTagMovie.DAL
{
	public class Repository<TEntity> where TEntity : class
	{
		protected MovieContext _context;
		protected DbSet<TEntity> dbSet;

		public Repository()
		{
			this._context = new MovieContext();
			this.dbSet = _context.Set<TEntity>();
		}

		public Repository(MovieContext context)
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