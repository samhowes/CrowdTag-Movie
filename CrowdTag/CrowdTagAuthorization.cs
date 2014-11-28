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
			return "a95d0b81-3245-4d7e-a429-64d2c07d412b";
		}
	}
}