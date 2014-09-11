using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace CrowdTagMovie
{
	public class CrowdTagAuthentication
	{
		private Func<string> _delegate;

		public CrowdTagAuthentication()
		{
			_delegate = HttpContext.Current.User.Identity.GetUserId;
		}

		public CrowdTagAuthentication(Func<string> method)
		{
			_delegate = method;
		}

		public string GetCurrentUserId()
		{
			return _delegate();
		}
	}
}