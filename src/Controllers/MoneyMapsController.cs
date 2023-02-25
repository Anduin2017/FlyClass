﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlyClass.Data;
using FlyClass.Models;
using Microsoft.AspNetCore.Authorization;

namespace FlyClass.Controllers;

[Authorize]
public class MoneyMapsController : Controller
{
    private readonly ApplicationDbContext _context;

    public MoneyMapsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: MoneyMaps
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.MoneyMaps.Include(m => m.ClassType).Include(m => m.Level);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: MoneyMaps/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.MoneyMaps == null)
        {
            return NotFound();
        }

        var moneyMap = await _context.MoneyMaps
            .Include(m => m.ClassType)
            .Include(m => m.Level)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moneyMap == null)
        {
            return NotFound();
        }

        return View(moneyMap);
    }

    // GET: MoneyMaps/Create
    public IActionResult Create()
    {
        ViewData["ClassTypeId"] = new SelectList(_context.ClassTypes, "Id", nameof(ClassType.Name));
        ViewData["LevelId"] = new SelectList(_context.Levels, "Id", nameof(Level.Name));
        return View();
    }

    // POST: MoneyMaps/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,LevelId,ClassTypeId,Bonus")] MoneyMap moneyMap)
    {
        if (ModelState.IsValid)
        {
            _context.Add(moneyMap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["ClassTypeId"] = new SelectList(_context.ClassTypes, "Id", nameof(ClassType.Name), moneyMap.ClassTypeId);
        ViewData["LevelId"] = new SelectList(_context.Levels, "Id", nameof(Level.Name), moneyMap.LevelId);
        return View(moneyMap);
    }

    // GET: MoneyMaps/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.MoneyMaps == null)
        {
            return NotFound();
        }

        var moneyMap = await _context.MoneyMaps.FindAsync(id);
        if (moneyMap == null)
        {
            return NotFound();
        }
        ViewData["ClassTypeId"] = new SelectList(_context.ClassTypes, "Id", nameof(ClassType.Name), moneyMap.ClassTypeId);
        ViewData["LevelId"] = new SelectList(_context.Levels, "Id", nameof(Level.Name), moneyMap.LevelId);
        return View(moneyMap);
    }

    // POST: MoneyMaps/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,LevelId,ClassTypeId,Bonus")] MoneyMap moneyMap)
    {
        if (id != moneyMap.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(moneyMap);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoneyMapExists(moneyMap.Id))
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
        ViewData["ClassTypeId"] = new SelectList(_context.ClassTypes, "Id", nameof(ClassType.Name), moneyMap.ClassTypeId);
        ViewData["LevelId"] = new SelectList(_context.Levels, "Id", nameof(Level.Name), moneyMap.LevelId);
        return View(moneyMap);
    }

    // GET: MoneyMaps/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.MoneyMaps == null)
        {
            return NotFound();
        }

        var moneyMap = await _context.MoneyMaps
            .Include(m => m.ClassType)
            .Include(m => m.Level)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (moneyMap == null)
        {
            return NotFound();
        }

        return View(moneyMap);
    }

    // POST: MoneyMaps/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.MoneyMaps == null)
        {
            return Problem("Entity set 'ApplicationDbContext.MoneyMaps'  is null.");
        }
        var moneyMap = await _context.MoneyMaps.FindAsync(id);
        if (moneyMap != null)
        {
            _context.MoneyMaps.Remove(moneyMap);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MoneyMapExists(int id)
    {
      return _context.MoneyMaps.Any(e => e.Id == id);
    }
}
