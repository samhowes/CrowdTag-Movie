﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CrowdTagMovie.Models
{
	public class Tag : UserAddedItem
	{
		[RegularExpression(ModelConstant.RegEx.TagName)]
		public string Name { get; set; }

		[Required]
		[ForeignKey("Category")]
		public int? CategoryID { get; set; }

		public virtual TagCategory Category { get; set; }
	}
}
