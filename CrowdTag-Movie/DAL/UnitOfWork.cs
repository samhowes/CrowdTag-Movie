using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrowdTagMovie.Models;

namespace CrowdTagMovie.DAL
{
	public class UnitOfWork : IDisposable
	{
		private MovieContext _context = new MovieContext();
		private Repository<Movie> _movieRepository;
		private Repository<User> _userRepository;

		public Repository<Movie> MovieRepository
		{
			get { return _movieRepository ?? (_movieRepository = new Repository<Movie>(_context)); }
		}

		public Repository<User> UserRepository
		{
			get { return _userRepository ?? (_userRepository = new Repository<User>(_context)); }
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