using System;
using CrowdTag.Model;

namespace CrowdTag.DataAccess
{
	public class UnitOfWork : IDisposable
	{
		private TagContext _context = new TagContext();
		private TaggedItemRepository _taggedItemRepository;
		private Repository<User> _userRepository;
		private TagCategoryRepository _tagCategoryRepository;
		private UserAddedItemRepository<Tag> _tagRepository;
		private UserAddedItemRepository<TagApplication> _tagApplicationRepository;

		public TaggedItemRepository TaggedItemRepository
		{
			get { return _taggedItemRepository ?? (_taggedItemRepository = new TaggedItemRepository(_context)); }
		}
		
		public TagCategoryRepository TagCategoryRepository
		{
			get { return _tagCategoryRepository ?? (_tagCategoryRepository = new TagCategoryRepository(_context)); }
		}

		public UserAddedItemRepository<Tag> TagRepository
		{
			get { return _tagRepository ?? (_tagRepository = new UserAddedItemRepository<Tag>(_context)); }
		}

		public Repository<User> UserRepository
		{
			get { return _userRepository ?? (_userRepository = new Repository<User>(_context)); }
		}

		public UserAddedItemRepository<TagApplication> TagApplicationRepository
		{
			get { return _tagApplicationRepository ?? (_tagApplicationRepository = new UserAddedItemRepository<TagApplication>(_context)); }
		}

		public void SaveChanges()
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