using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CrowdTagMovie.DAL
{
	public interface IRepository<TEntity>
	 where TEntity : class
	{
		void Add(TEntity newEntity);
		void Delete(TEntity entityToDelete);
		void DeleteById(object id);
		IEnumerable<TEntity> Get(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc = null, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeExpressions);
		TEntity GetById(object id);
		void Update(TEntity editedEntity);
	}
}
