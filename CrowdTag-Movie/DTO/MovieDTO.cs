using CrowdTagMovie.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrowdTagMovie.DTO
{
	public class MovieDTO
	{
		[Required]
		[StringLength(255)]
		[RegularExpression(@"^[\'\-\:\s\w,]*$", ErrorMessage = "Title must not contain special characters")]
		public string Title { get; set; }


		[Required]
		[DataType(DataType.Date)]
		public DateTime? ReleaseDate { get; set; }


		[StringLength(ModelConstant.StringLength.FreeText, MinimumLength = 20, ErrorMessage = "Description must be at least 20 characters")]
		public string Description { get; set; }


		[StringLength(100)]
		[RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
		public string Director { get; set; }

		public Movie ToDBO()
		{
			return new Movie
			{
				Title = Title,
				ReleaseDate = ReleaseDate,
				Description = Description,
				Director = Director
			};
		}

		public static MovieDTO FromDBO(Movie dbo)
		{
			return new MovieDTO
			{
				Title = dbo.Title,
				ReleaseDate = dbo.ReleaseDate,
				Description = dbo.Description,
				Director = dbo.Director
			};
		}
	}
}