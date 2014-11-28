using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CrowdTag.Model;

namespace CrowdTag.DataAccess
{
	public class TagContext : DbContext//, IMovieContext
	{

		public TagContext() : base("TagContext")
		{}

		public DbSet<TaggedItem> TaggedItems { get; set; }

		public DbSet<TagApplication> TagApplications { get; set; }

		public DbSet<IngredientTagApplication> IngredientTagApplications { get; set; }

		public DbSet<TagCategory> TagCategories { get; set; }

		public DbSet<User> Users { get; set; }

		//public DbSet<UserRank> Ranks { get; set; }

		//public DbSet<Vote> Votes { get; set; }



		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
		}

		public void AttachIfUnattached(object entityToAttach)
		{
			if (this.Entry(entityToAttach).State == EntityState.Detached)
			{
				this.Set(entityToAttach.GetType()).Attach(entityToAttach);
			}
		}

		public void MarkAsModified(object editedEntity)
		{
			Entry(editedEntity).State = EntityState.Modified;
		}

	}
}