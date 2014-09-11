using CrowdTagMovie.Models;
using System;
using System.Data.Entity;
namespace CrowdTagMovie.DAL
{
	public interface IMovieContext
	{
		DbSet<Movie> Movies { get; set; }
		
		
		
		DbSet<User> Users { get; set; }

		//DbSet<UserRank> Ranks { get; set; }

		int SaveChanges();

		DbSet<TEntity> Set<TEntity>() where TEntity : class;

		void AttachIfUnattached(object entityToDelete);

		void MarkAsModified(object editedEntity);
	}
}
