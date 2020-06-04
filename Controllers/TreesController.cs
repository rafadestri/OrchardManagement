using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrchardManagement.Data;
using OrchardManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrchardManagement.Controllers
{
    public class TreesController : Controller
    {
        private readonly OrchardContext _context;

        public TreesController(OrchardContext context)
        {
            _context = context;
        }

        // GET: Trees
        public async Task<IActionResult> Index()
        {
            var viewModel = new TreeIndexData();
            viewModel.Trees = await _context.Tree
                  .Include(i => i.Species)
                  .Include(i => i.TreeGroupTrees)
                    .ThenInclude(i => i.TreeGroups)
                  .OrderBy(i => i.Description)
                  .ToListAsync();

            return View(viewModel);
        }

        // GET: Trees/Create
        public IActionResult Create()
        {
            ViewData["SpeciesID"] = new SelectList(_context.Species.OrderBy(s => s.Description), "ID", "Description");

            var tree = new Tree();
            PopulateAssignedGroupData(tree);
            return View();
        }

        // POST: Trees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Description,Age,SpeciesID")] Tree tree, string[] selectedTreeGroups)
        {
            ViewData["SpeciesID"] = new SelectList(_context.Species, "ID", "Description", tree.SpeciesID);

            if (selectedTreeGroups != null)
            {
                foreach (var treeGroup in selectedTreeGroups)
                {
                    var treeGroupToAdd = new TreeGroupTree { TreeID = tree.ID, TreeGroupID = int.Parse(treeGroup) };

                    tree.TreeGroupTrees.Add(treeGroupToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(tree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedGroupData(tree);

            return View(tree);
        }

        // GET: Trees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tree = await _context.Tree
                .Include(i => i.Species)
                .Include(i => i.TreeGroupTrees).ThenInclude(i => i.TreeGroups)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (tree == null)
            {
                return NotFound();
            }

            PopulateAssignedGroupData(tree);

            ViewData["SpeciesID"] = new SelectList(_context.Species.OrderBy(s => s.Description), "ID", "Description");

            return View(tree);
        }

        private void PopulateAssignedGroupData(Tree tree)
        {
            var treeGroups = _context.TreeGroup;
            var treeGroupTrees = new HashSet<int>(tree.TreeGroupTrees.Select(c => c.TreeGroupID));
            var viewModel = new List<AssignedGroupData>();
            foreach (var group in treeGroups)
            {
                viewModel.Add(new AssignedGroupData
                {
                    TreeGroupID = group.ID,
                    Name = group.Name,
                    Assigned = treeGroupTrees.Contains(group.ID)
                });
            }

            ViewData["TreeGroups"] = viewModel;
        }

        // POST: Trees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedTreeGroups)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treeToUpdate = await _context.Tree
                .Include(i => i.Species)
                .Include(i => i.TreeGroupTrees)
                    .ThenInclude(i => i.TreeGroups)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Tree>(
                treeToUpdate,
                "",
                t => t.Description, t => t.Age, t => t.SpeciesID))
            {
                UpdateTreeGroups(selectedTreeGroups, treeToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateTreeGroups(selectedTreeGroups, treeToUpdate);
            PopulateAssignedGroupData(treeToUpdate);

            return View(treeToUpdate);
        }

        private void UpdateTreeGroups(string[] selectedTreeGroups, Tree treeToUpdate)
        {
            if (selectedTreeGroups == null)
            {
                treeToUpdate.TreeGroupTrees = new List<TreeGroupTree>();
                return;
            }

            var selectedGroupsHS = new HashSet<string>(selectedTreeGroups);
            var treeGroupTree = new HashSet<int>(treeToUpdate.TreeGroupTrees.Select(c => c.TreeGroups.ID));
            foreach (var group in _context.TreeGroup)
            {
                if (selectedGroupsHS.Contains(group.ID.ToString()))
                {
                    if (!treeGroupTree.Contains(group.ID))
                    {
                        treeToUpdate.TreeGroupTrees.Add(new TreeGroupTree { TreeID = treeToUpdate.ID, TreeGroupID = group.ID });
                    }
                }
                else
                {
                    if (treeGroupTree.Contains(group.ID))
                    {
                        TreeGroupTree groupsToRemove = treeToUpdate.TreeGroupTrees.FirstOrDefault(i => i.TreeGroupID == group.ID);
                        _context.Remove(groupsToRemove);
                    }
                }
            }
        }



        // GET: Trees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tree = await _context.Tree
                .Include(t => t.Species)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tree == null)
            {
                return NotFound();
            }

            return View(tree);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Tree tree = await _context.Tree
                .Include(i => i.TreeGroupTrees)
                .SingleAsync(i => i.ID == id);

            _context.Tree.Remove(tree);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreeExists(int id)
        {
            return _context.Tree.Any(e => e.ID == id);
        }
    }
}
