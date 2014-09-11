using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CrowdTagMovie.Models;

namespace CrowdTagMovie.DAL
{
	public class TagInitializer : DropCreateDatabaseIfModelChanges<TagContext>
	{
		protected override void Seed(TagContext context)
		{
			var now = DateTime.Now;

			/*var ranks = new List<UserRank>
			{
				new UserRank
				{
					 Level=0,
				},
			};

			ranks.ForEach(r => context.Ranks.Add(r));
			context.SaveChanges();
			*/

			var crowdTagBot = new User
			{
				ID = Guid.NewGuid().ToString(),
				Username="CrowdTagBot",
				Score=0,
				Email="Noreply@CrowdTag.com",
				DateJoined=now,
				LastActivity=now,
				//UserRankID=ranks[0].UserRankID,
			}; 

			context.Users.Add(crowdTagBot);
			context.SaveChanges();

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



			for (int ii = 0; ii < movies.Count; ii++)
			{
				movies[ii].SubmitterID = crowdTagBot.ID;
				context.TaggedItems.Add(movies[ii]);
				context.SaveChanges();
			}
			//_context.SaveChanges();
		}
	}
}

/*
 * new TaggedItem{Name="",Director="", ReleaseDate=DateTime.Parse(""),CreatedDateTime=DateTime.Parse(""), Description=""}
 */


