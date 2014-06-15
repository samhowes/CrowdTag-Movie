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

namespace CrowdTagMovie.Controllers
{
	internal sealed class SortStrings
	{
		public const string TitleDescend	= "Title_desc";
		public const string TitleAscend		= "";
		public const string ReleaseDescend	= "Release_desc";
		public const string ReleaseAscend	= "Release";
	}

    public class MovieController : Controller
    {
		private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: /Movie/
		[AllowAnonymous]
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
			ViewBag.CurrentSort = sortOrder;
			ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? SortStrings.TitleDescend : SortStrings.TitleAscend;
			ViewBag.ReleaseSortParm = sortOrder == SortStrings.ReleaseAscend ? SortStrings.ReleaseDescend : SortStrings.ReleaseAscend;
			
			
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;


			Expression<Func<Movie,bool>> searchFunc = null;
			Func<IQueryable<Movie>, IOrderedQueryable<Movie>> orderByFunc = null;
			int pageSize = 4;
			int pageNumber = (page ?? 1);

			if (!String.IsNullOrEmpty(searchString))
			{
				/*moviesQuery = moviesQuery.Where(m => 
					m.Title.ToUpper().Contains(searchString.ToUpper()) ||
					m.Director.ToUpper().Contains(searchString.ToUpper())
					);*/

				searchFunc = m => m.Title.ToUpper().Contains(searchString.ToUpper())
												|| m.Director.ToUpper().Contains(searchString.ToUpper());
			}

			switch (sortOrder)
			{
				case SortStrings.TitleDescend:
					orderByFunc = m => m.OrderByDescending(movie => movie.Title);
					break;
				case SortStrings.ReleaseAscend:
					orderByFunc = m => m.OrderBy(movie => movie.ReleaseDate);
					break;
				case SortStrings.ReleaseDescend:
					orderByFunc = m => m.OrderByDescending(movie => movie.ReleaseDate);
					break;
				default:
					orderByFunc = m => m.OrderBy(movie => movie.Title);
					break;
			}

			/*return View(await Task.Run<IPagedList>(
				() =>
				{
					return moviesQuery.ToPagedList(pageNumber, pageSize);
				})); */

			//var movies = await unitOfWork.MovieRepository.GetAsync(searchFunc, orderByFunc);
			return View(await unitOfWork.MovieRepository.GetAsync(orderByFunc, searchFunc));
        }

        // GET: /Movie/Details/5
		[AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
			var movie = new Movie();
			/*
			if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }*/
            return View(movie);
        }
		/*
        // GET: /Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="MovieID,DateAdded,Title,ReleaseDate,Description,Director")] Movie movie)
        {
            if (ModelState.IsValid)
            {
				db.Movies.Add(movie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: /Movie/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: /Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="MovieID,DateAdded,Title,ReleaseDate,Description,Director")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: /Movie/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }
		

        // POST: /Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Movie movie = await db.Movies.FindAsync(id);
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

		 */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
				unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
