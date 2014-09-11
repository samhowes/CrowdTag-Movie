using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using PagedList;
using CrowdTagMovie.Models;
using CrowdTagMovie.DAL;
using CrowdTagMovie.DTO;

namespace CrowdTagMovie.Controllers
{
	public sealed class SortStrings
	{
		public const string TitleDescend	= "Title_desc";
		public const string TitleAscend		= "";
		public const string ReleaseDescend	= "Release_desc";
		public const string ReleaseAscend	= "Release";
	}

    public class MovieController : Controller
    {
		private UnitOfWork UoW = new UnitOfWork();

		private int pageSize = 10;

		private void SetViewBagSortParameters(string sortOrder)
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? SortStrings.TitleDescend : SortStrings.TitleAscend;
			ViewBag.ReleaseSortParm = (sortOrder == SortStrings.ReleaseAscend) ? SortStrings.ReleaseDescend : SortStrings.ReleaseAscend;

		}

        // GET: /TaggedItem/
		[AllowAnonymous]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
			SetViewBagSortParameters(sortOrder);
			
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;


			Expression<Func<TaggedItem,bool>> searchFunc = null;
			Func<IQueryable<TaggedItem>, IOrderedQueryable<TaggedItem>> orderByFunc = null;
			int pageNumber = (page ?? 1);

			if (!String.IsNullOrEmpty(searchString))
			{
				searchFunc = m => m.Name.ToUpper().Contains(searchString.ToUpper())
												|| m.Director.ToUpper().Contains(searchString.ToUpper());
			}

			switch (sortOrder)
			{
				case SortStrings.TitleDescend:
					orderByFunc = m => m.OrderByDescending(movie => movie.Name);
					break;
				case SortStrings.ReleaseAscend:
					orderByFunc = m => m.OrderBy(movie => movie.ReleaseDate);
					break;
				case SortStrings.ReleaseDescend:
					orderByFunc = m => m.OrderByDescending(movie => movie.ReleaseDate);
					break;
				default:
					orderByFunc = m => m.OrderBy(movie => movie.Name);
					break;
			}

			return View(UoW.MovieRepository.Get(orderByFunc, searchFunc));
        }

        // GET: /TaggedItem/Details/5
		[AllowAnonymous]
        public ActionResult Details(int? id)
        {
			var movie = new TaggedItem();
			
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
			movie = UoW.MovieRepository.GetById(id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }
		
        // GET: /TaggedItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /TaggedItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Name,ReleaseDate,Description,Director")] MovieDTO movie)
        {
            if (ModelState.IsValid)
            {
				var dbo = movie.ToDBO();
				UoW.MovieRepository.Add(dbo);
				UoW.Commit();
				return RedirectToAction("Details", new { id = dbo.ID });
            }

            return View(movie);
        }

        // GET: /TaggedItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
			TaggedItem movie = UoW.MovieRepository.GetById(id);

			if (movie == null)
            {
                return HttpNotFound();
            }
            
			return View(movie);
        }

        // POST: /TaggedItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Name,ReleaseDate,Description,Director")] TaggedItem movie)
        {
            if (ModelState.IsValid)
            {
                UoW.MovieRepository.Update(movie);
				return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: /TaggedItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TaggedItem movie = UoW.MovieRepository.GetById(id);

            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }
		

        // POST: /TaggedItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			UoW.MovieRepository.DeleteById(id);
			UoW.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				UoW.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
