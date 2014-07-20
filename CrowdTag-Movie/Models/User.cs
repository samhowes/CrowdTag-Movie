using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;


namespace CrowdTagMovie.Models
{
	public class User
	{
		[MinLength(ModelConstant.StringLength.GUID), MaxLength(ModelConstant.StringLength.GUID)]
		public string ID { get; set; }

		[StringLength(160)]
		public string Username { get; set; }

		[ForeignKey("Rank")]
		public int UserRankID { get; set; }

		[Required]
		public int Score { get; set; }

		[StringLength(100)]
		public string FirstName { get; set; }

		[StringLength(100)]
		public string LastName { get; set; }

		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
			ErrorMessage = "Email is is not valid.")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }


		[Required]
		public DateTime? LastActivity { get; set; }


		[Required]
		public DateTime? DateJoined { get; set; }


		public virtual UserRank Rank { get; set; }
		public virtual ICollection<TagApplication> TagApplications { get; set; }
		public virtual ICollection<Movie> Movies { get; set; }
		public virtual ICollection<Tag> Tags { get; set; }
		public virtual ICollection<Vote> Votes { get; set; }
		
	}
}