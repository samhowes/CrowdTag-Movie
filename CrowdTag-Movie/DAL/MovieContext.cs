using CrowdTagMovie.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CrowdTagMovie.DAL
{
	public class MovieContext : DbContext
	{

		public MovieContext() : base("MovieContext")
		{}

		public DbSet<Movie> Movies { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}

	}
}