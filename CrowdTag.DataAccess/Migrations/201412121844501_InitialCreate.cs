namespace CrowdTag.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaggedItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Description = c.String(nullable: false, maxLength: 1000),
                        SubmitterId = c.String(nullable: false, maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.SubmitterId)
                .Index(t => t.SubmitterId);
            
            CreateTable(
                "dbo.IngredientApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IngredientId = c.Int(nullable: false),
                        DrinkId = c.Int(nullable: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        MeasurementTypeId = c.Int(nullable: false),
                        SubmitterId = c.String(nullable: false, maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaggedItems", t => t.DrinkId)
                .ForeignKey("dbo.Tags", t => t.IngredientId)
                .ForeignKey("dbo.MeasurementTypes", t => t.MeasurementTypeId)
                .ForeignKey("dbo.Users", t => t.SubmitterId)
                .Index(t => t.IngredientId)
                .Index(t => t.DrinkId)
                .Index(t => t.MeasurementTypeId)
                .Index(t => t.SubmitterId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryId = c.Int(),
                        SubmitterId = c.String(nullable: false, maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TagCategories", t => t.CategoryId)
                .ForeignKey("dbo.Users", t => t.SubmitterId)
                .Index(t => t.CategoryId)
                .Index(t => t.SubmitterId);
            
            CreateTable(
                "dbo.TagCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        SubmitterId = c.String(nullable: false, maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.SubmitterId)
                .Index(t => t.SubmitterId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Username = c.String(nullable: false, maxLength: 160),
                        Score = c.Int(nullable: false),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(nullable: false),
                        DateJoined = c.DateTime(nullable: false),
                        LastActivity = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaggedItemId = c.Int(nullable: false),
                        TagId = c.Int(nullable: false),
                        SubmitterId = c.String(nullable: false, maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.SubmitterId)
                .ForeignKey("dbo.Tags", t => t.TagId)
                .ForeignKey("dbo.TaggedItems", t => t.TaggedItemId)
                .Index(t => t.TaggedItemId)
                .Index(t => t.TagId)
                .Index(t => t.SubmitterId);
            
            CreateTable(
                "dbo.MeasurementTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        HasAmount = c.Boolean(nullable: false),
                        SubmitterId = c.String(nullable: false, maxLength: 128),
                        CreatedDateTime = c.DateTime(nullable: false),
                        UpdatedDateTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.SubmitterId)
                .Index(t => t.SubmitterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IngredientApplications", "SubmitterId", "dbo.Users");
            DropForeignKey("dbo.IngredientApplications", "MeasurementTypeId", "dbo.MeasurementTypes");
            DropForeignKey("dbo.MeasurementTypes", "SubmitterId", "dbo.Users");
            DropForeignKey("dbo.IngredientApplications", "IngredientId", "dbo.Tags");
            DropForeignKey("dbo.TagCategories", "SubmitterId", "dbo.Users");
            DropForeignKey("dbo.TagApplications", "TaggedItemId", "dbo.TaggedItems");
            DropForeignKey("dbo.TagApplications", "TagId", "dbo.Tags");
            DropForeignKey("dbo.Tags", "SubmitterId", "dbo.Users");
            DropForeignKey("dbo.Tags", "CategoryId", "dbo.TagCategories");
            DropForeignKey("dbo.TagApplications", "SubmitterId", "dbo.Users");
            DropForeignKey("dbo.TaggedItems", "SubmitterId", "dbo.Users");
            DropForeignKey("dbo.IngredientApplications", "DrinkId", "dbo.TaggedItems");
            DropIndex("dbo.MeasurementTypes", new[] { "SubmitterId" });
            DropIndex("dbo.TagApplications", new[] { "SubmitterId" });
            DropIndex("dbo.TagApplications", new[] { "TagId" });
            DropIndex("dbo.TagApplications", new[] { "TaggedItemId" });
            DropIndex("dbo.TagCategories", new[] { "SubmitterId" });
            DropIndex("dbo.Tags", new[] { "SubmitterId" });
            DropIndex("dbo.Tags", new[] { "CategoryId" });
            DropIndex("dbo.IngredientApplications", new[] { "SubmitterId" });
            DropIndex("dbo.IngredientApplications", new[] { "MeasurementTypeId" });
            DropIndex("dbo.IngredientApplications", new[] { "DrinkId" });
            DropIndex("dbo.IngredientApplications", new[] { "IngredientId" });
            DropIndex("dbo.TaggedItems", new[] { "SubmitterId" });
            DropTable("dbo.MeasurementTypes");
            DropTable("dbo.TagApplications");
            DropTable("dbo.Users");
            DropTable("dbo.TagCategories");
            DropTable("dbo.Tags");
            DropTable("dbo.IngredientApplications");
            DropTable("dbo.TaggedItems");
        }
    }
}
