using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrowdTagMovie.Models
{
	public static class ModelConstant
	{
		public static class StringLength
		{
			public const int GUID = 128;
			public const int UserID = GUID;
			public const int FreeText = 1000;
		}

		public static class FormatString
		{
			public const string Date = "{0:MM/dd/yyyy}";
		}

		public static class RegEx
		{
			public const string TagName = @"^[\S]{3,}$";
			public const string ItemName = @"^.{3,}$";
		}
	}
}