using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace CrowdTagMovie
{
	public static class CrowdTagAuthorization
	{
		//private Func<string> _delegate;

		/*public CrowdTagAuthorization()
		{
			//_delegate = HttpContext.Current.User.Identity.GetUserId;
		}

		public CrowdTagAuthorization(Func<string> method)
		{
			_delegate = method;
		}*/

		public static string GetCurrentUserId()
		{
			//return _delegate();
			return "0aa44960-863a-453c-8b52-04ac08289f66";
		}
	}
}