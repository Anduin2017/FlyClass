using Anduin.FlyClass.Data;
using Anduin.FlyClass.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Anduin.FlyClass.Controllers;

[Authorize(Roles = "Admin")]
public class SitesController : Controller
{
    private readonly FlyClassDbContext _context;

    public SitesController(FlyClassDbContext context)
    {
        _context = context;
    }

    // GET: Sites
    public async Task<IActionResult> Index()
    {
          return _context.Sites != null ? 
                      View(await _context.Sites.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Sites'  is null.");
    }

    // GET: Sites/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Sites == null)
        {
            return NotFound();
        }

        var site = await _context.Sites
            .FirstOrDefaultAsync(m => m.Id == id);
        if (site == null)
        {
            return NotFound();
        }

        return View(site);
    }

    // GET: Sites/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Sites/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,SiteName")] Site site)
    {
        if (ModelState.IsValid)
        {
            _context.Add(site);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(site);
    }

    // GET: Sites/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Sites == null)
        {
            return NotFound();
        }

        var site = await _context.Sites.FindAsync(id);
        if (site == null)
        {
            return NotFound();
        }
        return View(site);
    }

    // POST: Sites/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,SiteName")] Site site)
    {
        if (id != site.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(site);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(site.Id))
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
        return View(site);
    }

    // GET: Sites/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Sites == null)
        {
            return NotFound();
        }

        var site = await _context.Sites
            .FirstOrDefaultAsync(m => m.Id == id);
        if (site == null)
        {
            return NotFound();
        }

        return View(site);
    }

    // POST: Sites/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Sites == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Sites'  is null.");
        }
        var site = await _context.Sites.FindAsync(id);
        if (site != null)
        {
            _context.Sites.Remove(site);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SiteExists(int id)
    {
      return (_context.Sites?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
