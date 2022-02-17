using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalMPA.Data;
using FinalMPA.Models;
using Microsoft.AspNetCore.Authorization;


namespace FinalMPA.Controllers
{
    [Authorize(Roles = "Employee")]

    public class MagazinesController : Controller
    {
        private readonly StoreContext _context;

        public MagazinesController(StoreContext context)
        {
            _context = context;
        }

        // GET: Magazines
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var magazines = from b in _context.Magazines
                        select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                magazines = magazines.Where(s => s.Title.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    magazines = magazines.OrderByDescending(b => b.Title);
                    break;
                case "Price":
                    magazines = magazines.OrderBy(b => b.Price);
                    break;
                case "price_desc":
                    magazines = magazines.OrderByDescending(b => b.Price);
                    break;
                default:
                    magazines = magazines.OrderBy(b => b.Title);
                    break;
            }
            int pageSize = 2;
            return View(await PaginatedList<Magazine>.CreateAsync(magazines.AsNoTracking(), pageNumber ??
           1, pageSize));
        }

        // GET: Magazines/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var magazine = await _context.Magazines
             .Include(s => s.Orders)
             .ThenInclude(e => e.Customer)
             .AsNoTracking()
             .FirstOrDefaultAsync(m => m.ID == id);
            if (magazine == null)
            {
                return NotFound();
            }

            return View(magazine);
        }

        // GET: Magazines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Magazines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Author,Price")] Magazine magazine)
        {
            try
            {
                if (ModelState.IsValid)
            {
                _context.Add(magazine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            }
            catch (DbUpdateException /* ex*/)
            {

                ModelState.AddModelError("", "Oops! Unable to save changes." + "Please try again. ");
            }
            return View(magazine);
        }

        // GET: Magazines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magazine = await _context.Magazines.FindAsync(id);
            if (magazine == null)
            {
                return NotFound();
            }
            return View(magazine);
        }

        // POST: Magazines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var studentToUpdate = await _context.Magazines.FirstOrDefaultAsync(s => s.ID == id);
            if (await TryUpdateModelAsync<Magazine>(
            studentToUpdate,
            "",
            s => s.Author, s => s.Title, s => s.Price))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists");
                }
            }
            return View(studentToUpdate);
        }

        // GET: Magazines/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
    {
        if (id == null)
            {
                return NotFound();
            }

            var magazine = await _context.Magazines
            .AsNoTracking()
            .FirstOrDefaultAsync(m=>m.ID==id);
            if (magazine == null)
            {
                return NotFound();
            }
        if (saveChangesError.GetValueOrDefault())
        {
            ViewData["ErrorMessage"] = "Delete failed. Try again";
        }
        return View(magazine);
        }

        // POST: Magazines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var magazine = await _context.Magazines.FindAsync(id);
        if (magazine == null)
        {
            return RedirectToAction(nameof(Index));
        }
        try
        {
            _context.Magazines.Remove(magazine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException /* ex */)
        {

            return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
        }
    }

        private bool MagazineExists(int id)
        {
            return _context.Magazines.Any(e => e.ID == id);
        }
        
        
    }
}
