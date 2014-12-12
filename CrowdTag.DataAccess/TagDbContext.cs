using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CrowdTag.Model;

namespace CrowdTag.DataAccess
{
    public class TagDbContext : DbContext//, IMovieContext
    {
        private const string _contextName = "TagContext";
        public static string ContextName { get { return _contextName; } }
        public TagDbContext() : base(ContextName)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Drink> Drinks { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<IngredientApplication> IngredientApplications { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<TagCategory> TagCategories { get; set; }

        public DbSet<TagApplication> TagApplications { get; set; }

        public DbSet<MeasurementType> MeasurementTypes { get; set; }

        
        
        //Future improvement:
        //public DbSet<TaggedItem> TaggedItems { get; set; }

        //public DbSet<UserRank> Ranks { get; set; }

        //public DbSet<Vote> Votes { get; set; }

        //public DbSet<IngredientTagApplication> IngredientTagApplications { get; set; }
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