using Anduin.FlyClass.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Anduin.FlyClass.Controllers;

[Authorize(Roles = "Admin")]
public class ClassTypesController(FlyClassDbContext context) : Controller
{
    // GET: ClassTypes
    public async Task<IActionResult> Index()
    {
        return View(await context.ClassTypes.ToListAsync());
    }

    // GET: ClassTypes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var classType = await context.ClassTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (classType == null)
        {
            return NotFound();
        }

        return View(classType);
    }

    // GET: ClassTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ClassTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] ClassType classType)
    {
        if (ModelState.IsValid)
        {
            context.Add(classType);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(classType);
    }

    // GET: ClassTypes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var classType = await context.ClassTypes.FindAsync(id);
        if (classType == null)
        {
            return NotFound();
        }
        return View(classType);
    }

    // POST: ClassTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ClassType classType)
    {
        if (id != classType.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(classType);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassTypeExists(classType.Id))
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
        return View(classType);
    }

    // GET: ClassTypes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var classType = await context.ClassTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (classType == null)
        {
            return NotFound();
        }

        return View(classType);
    }

    // POST: ClassTypes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var classType = await context.ClassTypes.FindAsync(id);
        if (classType != null)
        {
            context.ClassTypes.Remove(classType);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClassTypeExists(int id)
    {
      return context.ClassTypes.Any(e => e.Id == id);
    }
}
