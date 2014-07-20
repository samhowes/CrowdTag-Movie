using System;

namespace CrowdTagMovie.DAL
{
	interface IPagedRepository<TEntity>
	 where TEntity : class
	{
		void Add(TEntity newEntity);
		void Delete(TEntity entityToDelete);
		void DeleteById(object id);
		PagedList.IPagedList Get(Func<System.Linq.IQueryable<TEntity>, System.Linq.IOrderedQueryable<TEntity>> orderByFunc, System.Linq.Expressions.Expression<Func<TEntity, bool>> filter = null, params System.Linq.Expressions.Expression<Func<TEntity, object>>[] includeExpressions);
		TEntity GetById(object id);
		void SetPaging(int pageNumber, int pageSize);
		void Update(TEntity editedEntity);
	}
}
