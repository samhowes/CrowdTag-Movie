using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrowdTagMovie.Models
{
	public class UserRank
	{
		public int UserRankID { get; set; }

		public int? Level { get; set; }

		public int MinScore { get; set; }

		public int MaxScore { get; set; }

		public String Name { get; set; }

		public String UserDescription { get; set; }

		public String ImageURL { get; set; }

		public virtual ICollection<User> Users { get; set; }
	}
}