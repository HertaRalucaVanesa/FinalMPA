using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalMPA.Data;
using FinalMPA.Models;
using FinalMPA.Models.StoreViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FinalMPA.Controllers
{
    [Authorize(Policy = "OnlySales")]

    public class PublishersController : Controller
    {
        private readonly StoreContext _context;

        public PublishersController(StoreContext context)
        {
            _context = context;
        }

        // GET: Publishers
        public async Task<IActionResult> Index(int? id, int? magazineID)
        {
            var viewModel = new PublisherIndexData();
            viewModel.Publishers = await _context.Publishers
            .Include(i => i.PublishedMagazines)
            .ThenInclude(i => i.Magazine)
            .ThenInclude(i => i.Orders)
            .ThenInclude(i => i.Customer)
            .AsNoTracking()
            .OrderBy(i => i.PublisherName)
            .ToListAsync();
            if (id != null)
            {
                ViewData["PublisherID"] = id.Value;
                Publisher publisher = viewModel.Publishers.Where(
                i => i.ID == id.Value).Single();
                viewModel.Magazines = publisher.PublishedMagazines.Select(s => s.Magazine);
            }
            if (magazineID != null)
            {
                ViewData["MagazineID"] = magazineID.Value;
                viewModel.Orders = viewModel.Magazines.Where(
                x => x.ID == magazineID).Single().Orders;
            }
            return View(viewModel);
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PublisherName,Adress")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(publisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publisher = await _context.Publishers.Include(i => i.PublishedMagazines).ThenInclude(i => i.Magazine).AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }
            PopulatePublishedMagazineData(publisher);
            return View(publisher);

        }
        private void PopulatePublishedMagazineData(Publisher publisher)
        {
            var allMagazines = _context.Magazines;
            var publisherMagazines = new HashSet<int>(publisher.PublishedMagazines.Select(c => c.MagazineID));
            var viewModel = new List<PublishedMagazineData>();
            foreach (var magazine in allMagazines)
            {
                viewModel.Add(new PublishedMagazineData
                {
                    MagazineID = magazine.ID,
                    Title = magazine.Title,
                    IsPublished = publisherMagazines.Contains(magazine.ID)
                });
            }
            ViewData["Magazines"] = viewModel;
        }

        // POST: Publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedMagazines)
        {
            if (id == null)
            {
                return NotFound();
            }
            var publisherToUpdate = await _context.Publishers
            .Include(i => i.PublishedMagazines)
            .ThenInclude(i => i.Magazine)
            .FirstOrDefaultAsync(m => m.ID == id);
            if (await TryUpdateModelAsync<Publisher>(publisherToUpdate,"", i => i.PublisherName, i => i.Adress))
            {
                UpdatePublishedMagazines(selectedMagazines, publisherToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {

                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, ");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdatePublishedMagazines(selectedMagazines, publisherToUpdate);
            PopulatePublishedMagazineData(publisherToUpdate);
            return View(publisherToUpdate);
        }
        private void UpdatePublishedMagazines(string[] selectedMagazines, Publisher publisherToUpdate)
        {
            if (selectedMagazines == null)
            {
                publisherToUpdate.PublishedMagazines = new List<PublishedMagazine>();
                return;
            }
            var selectedMagazinesHS = new HashSet<string>(selectedMagazines);
            var publishedMagazines = new HashSet<int>
            (publisherToUpdate.PublishedMagazines.Select(c => c.Magazine.ID));
            foreach (var magazine in _context.Magazines)
            {
                if (selectedMagazinesHS.Contains(magazine.ID.ToString()))
                {
                    if (!publishedMagazines.Contains(magazine.ID))
                    {
                        publisherToUpdate.PublishedMagazines.Add(new PublishedMagazine
                        {
                            PublisherID =
                       publisherToUpdate.ID,
                            MagazineID = magazine.ID
                        });
                    }
                }
                else
                {
                    if (publishedMagazines.Contains(magazine.ID))
                    {
                        PublishedMagazine magazineToRemove = publisherToUpdate.PublishedMagazines.FirstOrDefault(i
                       => i.MagazineID == magazine.ID);
                        _context.Remove(magazineToRemove);
                    }
                }
            }
        }

        // GET: Publishers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publishers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.ID == id);
        }
    }
}
