using System;
using System.Web;
using Microsoft.AspNet.Identity;
using CrowdTagMovie.Models;

namespace CrowdTagMovie.DAL
{
	public class UnitOfWork : IDisposable
	{
		private TagContext _context = new TagContext();
		private UserAddedItemRepository<TaggedItem> _movieRepository;
		private Repository<User> _userRepository;

		public UserAddedItemRepository<TaggedItem> MovieRepository
		{
			get { return _movieRepository ?? (_movieRepository = new UserAddedItemRepository<TaggedItem>(_context)); }
		}

		public Repository<User> UserRepository
		{
			get { return _userRepository ?? (_userRepository = new Repository<User>(_context)); }
		}

		public string GetCurrentUserID()
		{
			return HttpContext.Current.User.Identity.GetUserId();
		}

		public void Commit()
		{
			_context.SaveChanges();
		}


		/***   IDisposable Implementation   ***/
		private bool disposed = false;
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				_context.Dispose();
			}
			this.disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}