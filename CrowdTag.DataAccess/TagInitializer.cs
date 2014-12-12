using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CrowdTag.Model;

namespace CrowdTag.DataAccess
{
    public class TagInitializer : 
        //CreateDatabaseIfNotExists<TagDbContext> 
        //DropCreateDatabaseIfModelChanges<TagDbContext>
        DropCreateDatabaseAlways<TagDbContext>
    {
        private User crowdTagBot;
        private DateTime now = DateTime.Now;

        protected override void Seed(TagDbContext tagContext)
        {
            AddUsers(tagContext);
            
            List<string> genres;
            List<string> classifications;
            List<string> usages;
            AddTagsAndCategories(tagContext, out genres, out classifications, out usages);

            var measurementTypes = AddMeasurementTypes(tagContext);

            var ingredientEntities = AddIngredientsAndCategories(tagContext);

            var drinks = AddDrinks(tagContext, ingredientEntities, measurementTypes);
        }

        private List<MeasurementType> AddMeasurementTypes(TagDbContext tagContext)
        {
            var measurementNames = new List<Tuple<string,bool>>
            {
                Tuple.Create("Fill", false),
                Tuple.Create("Ounce", true),
                Tuple.Create("Dash", true)
            };

            var measurementTypes = measurementNames.Select(mt => new MeasurementType()
            {
                Name = mt.Item1,
                HasAmount = mt.Item2
            }).ToList();

            ApplyUserProperties(measurementTypes);

            tagContext.MeasurementTypes.AddRange(measurementTypes);
            return measurementTypes;
        }

        private List<Drink> AddDrinks(TagDbContext tagContext, List<Ingredient> ingredientEntities, List<MeasurementType> measurementTypes)
        {
            var drinks = new List<Drink>
            {
                new Drink
                {
                    Name = "Gin & Tonic",
                    CreatedDateTime = DateTime.Parse("15 December 1995"),
                    Description = "An excellent classic drink"
                },
                new Drink
                {
                    Name = "Bloody Mary",
                    CreatedDateTime = DateTime.Parse("26 June 2013"),
                    Description = "A classic disgusting tomatoey mix"
                },
                new Drink
                {
                    Name = "Whiskey and Gingerale",
                    CreatedDateTime = DateTime.Parse("22 July 2012"),
                    Description = "An excellent classic drink"
                },
                new Drink
                {
                    Name = "Spys Demise",
                    CreatedDateTime = DateTime.Parse("11 August 2011"),
                    Description = "A potent concoction"
                },
                new Drink
                {
                    Name = "Malibu Bay Breeze",
                    CreatedDateTime = DateTime.Parse("16 November 2010"),
                    Description = "Coconutty goodness"
                },
                new Drink
                {
                    Name = "Dark and Stormy",
                    CreatedDateTime = DateTime.Parse("31 December 2010"),
                    Description = "A classic drink by Gosling"
                }
            };

            var ginAndTonic = drinks[0];
            var gin = ingredientEntities[0];
            var tonic = ingredientEntities[1];

            var fillWith = measurementTypes[0];
            var ounces = measurementTypes[1];

            ginAndTonic.Recipe.Add(new IngredientApplication()
            {
                Amount = (decimal?)1.5,
                MeasurementTypeId = ounces.Id,
                IngredientId = gin.Id
            });

            ginAndTonic.Recipe.Add(new IngredientApplication()
            {
                MeasurementTypeId = fillWith.Id,
                IngredientId = tonic.Id
            });

            ApplyUserProperties(ginAndTonic.Recipe);

            ApplyUserProperties(drinks, crowdTagBot);

            tagContext.Drinks.AddRange(drinks);
            tagContext.SaveChanges();
            return drinks;
        }

        private List<Ingredient> AddIngredientsAndCategories(TagDbContext tagContext)
        {
            var ingredients = new List<string>
            {
                "Gin",
                "Tonic",
                "Vodka",
                "Whiskey",
                "Gingerale",
                "Burbon",
                "Scotch",
                "CranberryJuice"
            };
            var ingredientEntities = ingredients.Select(name =>
                new Ingredient
                {
                    Name = name
                }).ToList();

            ApplyUserProperties(ingredientEntities);
            tagContext.Ingredients.AddRange(ingredientEntities);
            tagContext.SaveChanges();
            return ingredientEntities;
        }

        private void AddTagsAndCategories(TagDbContext tagContext, out List<string> genres, out List<string> classifications, out List<string> usages)
        {
            genres = new List<string> {"Fruity", "Manly", "Girly", "Stiff", "Classic"};
            classifications = new List<string> {"Martini", "Shooter", "Shot", "Margarita"};
            usages = new List<string> {"OnTheRocks", "Frozen", "Flaming"};

            var tagCategories = new List<TagCategory>
            {
                //new TagCategory
                //{
                //    Name = "Ingredient",Description = "An item used to make a drink", Tags = ingredients.Select(i => new Tag{Name = i}).ToList()
                //},
                new TagCategory
                {
                    Name = "Genre",
                    Description = "Fruity or Manly?",
                    Tags = genres.Select(i => new Tag {Name = i}).ToList()
                },
                new TagCategory
                {
                    Name = "Classification",
                    Description = "The type of the drink, Martini or Shooter?",
                    Tags = classifications.Select(i => new Tag {Name = i}).ToList()
                },
                new TagCategory
                {
                    Name = "Usage",
                    Description = "A possible way to consume the drink - on the rocks, or frozen?",
                    Tags = usages.Select(i => new Tag {Name = i}).ToList()
                }
            };

            foreach (var category in tagCategories)
            {
                ApplyUserProperties(category.Tags);
                tagContext.TagCategories.Add(category);
            }
            ApplyUserProperties(tagCategories);
            tagContext.SaveChanges();
        }

        private void AddUsers(TagDbContext tagContext)
        {
            crowdTagBot = new User
            {
                Id = "a95d0b81-3245-4d7e-a429-64d2c07d412b",
                Username = "CrowdTagBot",
                Score = 0,
                Email = "Noreply@CrowdTag.com",
                DateJoined = now,
                LastActivity = now,
                //UserRankID=ranks[0].UserRankID,
            };

            tagContext.Users.Add(crowdTagBot);
            tagContext.SaveChanges();
        }

        private void ApplyUserProperties(IEnumerable<UserAddedItem> entities, User user = null)
        {
            foreach (var entity in entities)
            {
                ApplyUserProperties(entity, user, entity.CreatedDateTime);
            }
        }

        private void ApplyUserProperties(UserAddedItem entity, User user = null, DateTime? date = null)
        {
            user = user ?? crowdTagBot;
            entity.SubmitterId = user.Id;
            entity.CreatedDateTime = date ?? DateTime.Now;
        }
    }
}
/*
 * var ranks = new List<UserRank>
            {
                new UserRank
                {
                     Level=0,
                },
            };

            ranks.ForEach(r => tagContext.Ranks.Add(r));
            tagContext.SaveChanges();
            
private void AddMovies()
{
    
            var movies = new List<TaggedItem>
            {
                new TaggedItem{Name="The Shawshank Redemption",CreatedDateTime=DateTime.Parse("15 December 1995"), Description="Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency."},
                new TaggedItem{Name="The Godfather",CreatedDateTime=DateTime.Parse("26 June 2013"), Description="The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son."},
                new TaggedItem{Name="The Godfather: Part II",CreatedDateTime=DateTime.Parse("22 July 2012"), Description="The early life and career of Vito Corleone in 1920s New York is portrayed while his son, Michael, expands and tightens his grip on his crime syndicate stretching from Lake Tahoe, Nevada to pre-revolution 1958 Cuba."},
                new TaggedItem{Name="The Dark Knight",CreatedDateTime=DateTime.Parse("11 August 2011"), Description="When Batman, Gordon and Harvey Dent launch an assault on the mob, they let the clown out of the box, the Joker, bent on turning Gotham on itself and bringing any heroes down to his level"},
                new TaggedItem{Name="Pulp Fiction",CreatedDateTime=DateTime.Parse("16 November 2010"), Description="The lives of two mob hit men, a boxer, a gangster's wife, and a pair of diner bandits intertwine in four tales of violence and redemption."},
                new TaggedItem{Name="The Good, the Bad and the Ugly",CreatedDateTime=DateTime.Parse("31 December 2010"), Description="A bounty hunting scam joins two men in an uneasy alliance against a third in a race to find a fortune in gold buried in a remote cemetery."},
                new TaggedItem{Name="Schindler's List",CreatedDateTime=DateTime.Parse("17 January 2011"), Description="In Poland during World War II, Oskar Schindler gradually becomes concerned for his Jewish workforce after witnessing their persecution by the Nazis."},
                new TaggedItem{Name="12 Angry Men",CreatedDateTime=DateTime.Parse("01 May 2014"), Description="A dissenting juror in a murder trial slowly manages to convince the others that the case is not as obviously clear as it seemed in court."}
            };
}

 * new TaggedItem{Name="",Director="", ReleaseDate=DateTime.Parse(""),CreatedDateTime=DateTime.Parse(""), Description=""}
 */


