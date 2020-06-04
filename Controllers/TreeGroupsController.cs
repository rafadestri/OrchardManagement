using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrchardManagement.Data;
using OrchardManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OrchardManagement.Controllers
{
    public class TreeGroupsController : Controller
    {
        private readonly OrchardContext _context;

        public TreeGroupsController(OrchardContext context)
        {
            _context = context;
        }

        // GET: TreeGroupTrees
        public async Task<IActionResult> Index()
        {
            return View(await _context.TreeGroup.OrderBy(tg => tg.Name).ToListAsync());
        }

        // GET: TreeGroupTrees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeGroup = await _context.TreeGroup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (treeGroup == null)
            {
                return NotFound();
            }

            return View(treeGroup);
        }

        // GET: TreeGroupTrees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TreeGroupTrees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] TreeGroup treeGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treeGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(treeGroup);
        }

        // GET: TreeGroupTrees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeGroup = await _context.TreeGroup.FindAsync(id);
            if (treeGroup == null)
            {
                return NotFound();
            }
            return View(treeGroup);
        }

        // POST: TreeGroupTrees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] TreeGroup treeGroup)
        {
            if (id != treeGroup.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treeGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreeGroupExists(treeGroup.ID))
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
            return View(treeGroup);
        }

        // GET: TreeGroupTrees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeGroup = await _context.TreeGroup
                .FirstOrDefaultAsync(m => m.ID == id);
            if (treeGroup == null)
            {
                return NotFound();
            }

            return View(treeGroup);
        }

        // POST: TreeGroupTrees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treeGroup = await _context.TreeGroup.FindAsync(id);
            _context.TreeGroup.Remove(treeGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreeGroupExists(int id)
        {
            return _context.TreeGroup.Any(e => e.ID == id);
        }
    }
}
