using CrowdTagMovie.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CrowdTagMovie.DAL
{
	public class MovieContext : DbContext
	{

		public MovieContext() : base("MovieContext2")
		{}

		public DbSet<Movie> Movies { get; set; }
		public DbSet<User> Users { get; set; }

		public DbSet<UserRank> Ranks { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
		}

	}
}