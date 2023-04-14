using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlyClass.Data;
using FlyClass.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlyClass.Controllers;

[Authorize(Roles = "Admin")]
public class TeachEventsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TeachEventsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TeachEvents
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.TeachEvents
            .Include(t => t.ClassType)
            .Include(t => t.Site)
            .Include(t => t.Teacher)
            .OrderByDescending(t => t.EventTime);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: TeachEvents/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.TeachEvents == null)
        {
            return NotFound();
        }

        var teachEvent = await _context.TeachEvents
            .Include(t => t.ClassType)
            .Include(t => t.Site)
            .Include(t => t.Teacher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teachEvent == null)
        {
            return NotFound();
        }

        return View(teachEvent);
    }

    // GET: TeachEvents/Create
    public async Task<IActionResult> Create()
    {
        ViewData["ClassTypeId"] = new SelectList(await _context.ClassTypes.ToListAsync(), "Id", nameof(ClassType.Name));
        ViewData["SiteId"] = new SelectList(await _context.Sites.ToListAsync(), "Id", nameof(Site.SiteName));
        ViewData["TeacherId"] = new SelectList(await _context.Teachers.ToListAsync(), "Id", nameof(Teacher.ChineseName));
        return View(model: new TeachEvent
        {
            EventTime = DateTime.UtcNow
        });
    }

    // POST: TeachEvents/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeachEvent teachEvent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teachEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClassTypeId"] = new SelectList(_context.ClassTypes, "Id", nameof(ClassType.Name), teachEvent.ClassTypeId);
        ViewData["SiteId"] = new SelectList(_context.Sites, "Id", nameof(Site.SiteName), teachEvent.SiteId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", nameof(Teacher.ChineseName), teachEvent.TeacherId);
        return View(teachEvent);
    }

    // GET: TeachEvents/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.TeachEvents == null)
        {
            return NotFound();
        }

        var teachEvent = await _context.TeachEvents.FindAsync(id);
        if (teachEvent == null)
        {
            return NotFound();
        }
        ViewData["ClassTypeId"] = new SelectList(_context.ClassTypes, "Id", nameof(ClassType.Name), teachEvent.ClassTypeId);
        ViewData["SiteId"] = new SelectList(_context.Sites, "Id", nameof(Site.SiteName), teachEvent.SiteId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", nameof(Teacher.ChineseName), teachEvent.TeacherId);
        return View(teachEvent);
    }

    // POST: TeachEvents/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TeachEvent teachEvent)
    {
        if (id != teachEvent.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teachEvent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeachEventExists(teachEvent.Id))
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
        ViewData["ClassTypeId"] = new SelectList(_context.ClassTypes, "Id", nameof(ClassType.Name), teachEvent.ClassTypeId);
        ViewData["SiteId"] = new SelectList(_context.Sites, "Id", nameof(Site.SiteName), teachEvent.SiteId);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", nameof(Teacher.ChineseName), teachEvent.TeacherId);
        return View(teachEvent);
    }

    // GET: TeachEvents/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.TeachEvents == null)
        {
            return NotFound();
        }

        var teachEvent = await _context.TeachEvents
            .Include(t => t.ClassType)
            .Include(t => t.Site)
            .Include(t => t.Teacher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teachEvent == null)
        {
            return NotFound();
        }

        return View(teachEvent);
    }

    // POST: TeachEvents/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.TeachEvents == null)
        {
            return Problem("Entity set 'ApplicationDbContext.TeachEvents'  is null.");
        }
        var teachEvent = await _context.TeachEvents.FindAsync(id);
        if (teachEvent != null)
        {
            _context.TeachEvents.Remove(teachEvent);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeachEventExists(int id)
    {
        return (_context.TeachEvents?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
