using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        private MovieContext db = new MovieContext();

        // GET: /Movie/
		[AllowAnonymous]
        public async Task<ActionResult> Index(string sortOrder)
        {
			ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? SortStrings.TitleDescend : SortStrings.TitleAscend;
			ViewBag.ReleaseSortParm = sortOrder == SortStrings.ReleaseAscend ? SortStrings.ReleaseDescend : SortStrings.ReleaseAscend;

			var moviesQuery = from m in db.Movies
							  select m;
			
			switch (sortOrder)
			{
				case SortStrings.TitleDescend:
					moviesQuery = moviesQuery.OrderByDescending(m => m.Title);
					break;
				case SortStrings.ReleaseAscend:
					moviesQuery = moviesQuery.OrderBy(m => m.ReleaseDate);
					break;
				case SortStrings.ReleaseDescend:
					moviesQuery = moviesQuery.OrderByDescending(m => m.ReleaseDate);
					break;
				default:
					moviesQuery = moviesQuery.OrderBy(m => m.Title);
					break;
			}
            return View(await moviesQuery.ToListAsync());
        }

        // GET: /Movie/Details/5
		[AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
