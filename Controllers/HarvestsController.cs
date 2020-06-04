using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrchardManagement.Data;
using OrchardManagement.Models;

namespace OrchardManagement.Controllers
{
    public class HarvestsController : Controller
    {
        private readonly OrchardContext _context;

        public HarvestsController(OrchardContext context)
        {
            _context = context;
        }

        // GET: Harvests
        public async Task<IActionResult> Index()
        {
            var orchardContext = _context.Harvest.Include(h => h.Trees);
            return View(await orchardContext.ToListAsync());
        }

        // GET: Harvests/Create
        public IActionResult Create()
        {
            ViewData["TreeID"] = new SelectList(_context.Tree, "ID", "Description");
            return View();
        }

        // POST: Harvests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Information,Date,GrossWeight,TreeID")] Harvest harvest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(harvest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TreeID"] = new SelectList(_context.Tree, "ID", "Description", harvest.TreeID);
            return View(harvest);
        }

        // GET: Harvests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var harvest = await _context.Harvest.FindAsync(id);
            if (harvest == null)
            {
                return NotFound();
            }
            ViewData["TreeID"] = new SelectList(_context.Tree, "ID", "Description", harvest.TreeID);
            return View(harvest);
        }

        // POST: Harvests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Information,Date,GrossWeight,TreeID")] Harvest harvest)
        {
            if (id != harvest.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(harvest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HarvestExists(harvest.ID))
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
            ViewData["TreeID"] = new SelectList(_context.Tree, "ID", "Description", harvest.TreeID);
            return View(harvest);
        }

        // GET: Harvests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var harvest = await _context.Harvest
                .Include(h => h.Trees)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (harvest == null)
            {
                return NotFound();
            }

            return View(harvest);
        }

        // POST: Harvests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var harvest = await _context.Harvest.FindAsync(id);
            _context.Harvest.Remove(harvest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HarvestExists(int id)
        {
            return _context.Harvest.Any(e => e.ID == id);
        }
    }
}
