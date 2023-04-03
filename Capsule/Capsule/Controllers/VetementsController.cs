using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Capsule.Data;
using Capsule.Models;

namespace Capsule.Controllers
{
    public class VetementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VetementsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: Vetements
        public async Task<IActionResult> Index(string vetementGenre, string searchString)
        {
            IQueryable<string> genreQuery = from m in _context.Vetement
                                            orderby m.Genre
                                            select m.Genre;
            var vetements = from m in _context.Vetement
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                vetements = vetements.Where(s => s.Title!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(vetementGenre))
            {
                vetements = vetements.Where(x => x.Genre == vetementGenre);
            }

            var vetementGenreVM = new VetementGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Vetements = await vetements.ToListAsync()
            };

            return View(vetementGenreVM);
        }

        // GET: Vetements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vetement == null)
            {
                return NotFound();
            }

            var vetement = await _context.Vetement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vetement == null)
            {
                return NotFound();
            }

            return View(vetement);
        }

        // GET: Vetements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vetements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Vetement vetement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vetement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vetement);
        }

        // GET: Vetements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vetement == null)
            {
                return NotFound();
            }

            var vetement = await _context.Vetement.FindAsync(id);
            if (vetement == null)
            {
                return NotFound();
            }
            return View(vetement);
        }

        // POST: Vetements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Vetement vetement)
        {
            if (id != vetement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vetement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VetementExists(vetement.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vetement);
        }

        // GET: Vetements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vetement == null)
            {
                return NotFound();
            }

            var vetement = await _context.Vetement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vetement == null)
            {
                return NotFound();
            }

            return View(vetement);
        }

        // POST: Vetements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vetement == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vetement'  is null.");
            }
            var vetement = await _context.Vetement.FindAsync(id);
            if (vetement != null)
            {
                _context.Vetement.Remove(vetement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VetementExists(int id)
        {
          return (_context.Vetement?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
