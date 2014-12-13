using System;
using CrowdTag.Model;

namespace CrowdTag.DataAccess
{
	public class DataContext : IDisposable
	{
		private TagDbContext _dbContext = new TagDbContext();
		private TaggedItemRepository _taggedItemRepository;
		private Repository<User> _userRepository;
		private TagCategoryRepository _tagCategoryRepository;
		private UserAddedItemRepository<TagBase> _tagRepository;
        private UserAddedItemRepository<TagApplication> _tagApplicationRepository;

		public TaggedItemRepository TaggedItemRepository
		{
			get { return _taggedItemRepository ?? (_taggedItemRepository = new TaggedItemRepository(_dbContext)); }
		}
		
		public TagCategoryRepository TagCategoryRepository
		{
			get { return _tagCategoryRepository ?? (_tagCategoryRepository = new TagCategoryRepository(_dbContext)); }
		}

		public UserAddedItemRepository<TagBase> TagRepository
		{
			get { return _tagRepository ?? (_tagRepository = new UserAddedItemRepository<TagBase>(_dbContext)); }
		}

		public Repository<User> UserRepository
		{
			get { return _userRepository ?? (_userRepository = new Repository<User>(_dbContext)); }
		}

		public UserAddedItemRepository<TagApplication> TagApplicationRepository
		{
            get { return _tagApplicationRepository ?? (_tagApplicationRepository = new UserAddedItemRepository<TagApplication>(_dbContext)); }
		}

		public void SaveChanges()
		{
			_dbContext.SaveChanges();
		}


		/***   IDisposable Implementation   ***/
		private bool disposed = false;
		
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				_dbContext.Dispose();
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