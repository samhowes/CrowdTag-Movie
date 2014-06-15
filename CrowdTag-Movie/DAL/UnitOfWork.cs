using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CrowdTagMovie.Models;

namespace CrowdTagMovie.DAL
{
	public class UnitOfWork : IDisposable
	{
		private MovieContext dbContext = new MovieContext();
		private GenericRepository<Movie> movieRepository;

		public GenericRepository<Movie> MovieRepository
		{
			get
			{
				if (this.movieRepository == null)
				{
					this.movieRepository = new GenericRepository<Movie>(dbContext);
				}
				return this.movieRepository;
			}
		}

		public void Save()
		{
			dbContext.SaveChanges();
		}


		/*** IDisposable Implementation ***/
		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				dbContext.Dispose();
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