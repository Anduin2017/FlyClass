﻿using Anduin.FlyClass.Entities;
using Anduin.FlyClass.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Anduin.FlyClass.Controllers;

[Authorize(Roles = "Admin")]
public class LevelsController(FlyClassDbContext context) : Controller
{
    // GET: Levels
    public async Task<IActionResult> Index()
    {
          return context.Levels != null ? 
                      View(await context.Levels.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Levels'  is null.");
    }

    // GET: Levels/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || context.Levels == null)
        {
            return NotFound();
        }

        var level = await context.Levels
            .FirstOrDefaultAsync(m => m.Id == id);
        if (level == null)
        {
            return NotFound();
        }

        return View(level);
    }

    // GET: Levels/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Levels/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name")] Level level)
    {
        if (ModelState.IsValid)
        {
            context.Add(level);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(level);
    }

    // GET: Levels/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || context.Levels == null)
        {
            return NotFound();
        }

        var level = await context.Levels.FindAsync(id);
        if (level == null)
        {
            return NotFound();
        }
        return View(level);
    }

    // POST: Levels/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Level level)
    {
        if (id != level.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(level);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LevelExists(level.Id))
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
        return View(level);
    }

    // GET: Levels/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || context.Levels == null)
        {
            return NotFound();
        }

        var level = await context.Levels
            .FirstOrDefaultAsync(m => m.Id == id);
        if (level == null)
        {
            return NotFound();
        }

        return View(level);
    }

    // POST: Levels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (context.Levels == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Levels'  is null.");
        }
        var level = await context.Levels.FindAsync(id);
        if (level != null)
        {
            context.Levels.Remove(level);
        }
        
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LevelExists(int id)
    {
      return (context.Levels?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
