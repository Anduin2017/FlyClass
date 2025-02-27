using Aiursoft.CSTools.Tools;
using Anduin.FlyClass.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Anduin.FlyClass.Controllers;

[Authorize(Roles = "Admin,Reviewer")]
public class TeachEventsController(FlyClassDbContext context) : Controller
{
    // GET: TeachEvents
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = context.TeachEvents
            .Include(t => t.ClassType)
            .Include(t => t.Site)
            .Include(t => t.Teacher)
            .OrderByDescending(t => t.EventTime);
        return View(await applicationDbContext.ToListAsync());
    }

    public async Task<IActionResult> Csv()
    {
        var applicationDbContext = await context.TeachEvents
            .Include(t => t.ClassType)
            .Include(t => t.Site)
            .Include(t => t.Teacher)
            .OrderByDescending(t => t.EventTime)
            .ToListAsync();
        var csv = applicationDbContext.Select(t => t.ToDto()).ToList().ToCsv();
        return File(csv, "text/csv", "TeachEvents.csv");
    }

    // GET: TeachEvents/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teachEvent = await context.TeachEvents
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
        ViewData["ClassTypeId"] = new SelectList(await context.ClassTypes.ToListAsync(), "Id", nameof(ClassType.Name));
        ViewData["SiteId"] = new SelectList(await context.Sites.ToListAsync(), "Id", nameof(Site.SiteName));
        ViewData["TeacherId"] = new SelectList(await context.Teachers.ToListAsync(), "Id", nameof(Teacher.ChineseName));
        return View(model: new TeachEvent
        {
            EventTime = DateTime.UtcNow,
            Comments = string.Empty,
            TeacherId = string.Empty,
            SiteId = 0,
            ClassTypeId = 0
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
            context.Add(teachEvent);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClassTypeId"] = new SelectList(context.ClassTypes, "Id", nameof(ClassType.Name), teachEvent.ClassTypeId);
        ViewData["SiteId"] = new SelectList(context.Sites, "Id", nameof(Site.SiteName), teachEvent.SiteId);
        ViewData["TeacherId"] = new SelectList(context.Teachers, "Id", nameof(Teacher.ChineseName), teachEvent.TeacherId);
        return View(teachEvent);
    }

    // GET: TeachEvents/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teachEvent = await context.TeachEvents.FindAsync(id);
        if (teachEvent == null)
        {
            return NotFound();
        }
        ViewData["ClassTypeId"] = new SelectList(context.ClassTypes, "Id", nameof(ClassType.Name), teachEvent.ClassTypeId);
        ViewData["SiteId"] = new SelectList(context.Sites, "Id", nameof(Site.SiteName), teachEvent.SiteId);
        ViewData["TeacherId"] = new SelectList(context.Teachers, "Id", nameof(Teacher.ChineseName), teachEvent.TeacherId);
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
                context.Update(teachEvent);
                await context.SaveChangesAsync();
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
        ViewData["ClassTypeId"] = new SelectList(context.ClassTypes, "Id", nameof(ClassType.Name), teachEvent.ClassTypeId);
        ViewData["SiteId"] = new SelectList(context.Sites, "Id", nameof(Site.SiteName), teachEvent.SiteId);
        ViewData["TeacherId"] = new SelectList(context.Teachers, "Id", nameof(Teacher.ChineseName), teachEvent.TeacherId);
        return View(teachEvent);
    }

    // GET: TeachEvents/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teachEvent = await context.TeachEvents
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
        var teachEvent = await context.TeachEvents.FindAsync(id);
        if (teachEvent != null)
        {
            context.TeachEvents.Remove(teachEvent);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeachEventExists(int id)
    {
        return context.TeachEvents.Any(e => e.Id == id);
    }
}
