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
using Microsoft.AspNetCore.Identity;
using FlyClass.Models.TeachersViewModels;

namespace FlyClass.Controllers;

[Authorize]
public class TeachersController : Controller
{
    private readonly UserManager<Teacher> userManager;
    private readonly SignInManager<Teacher> signInManager;
    private readonly ApplicationDbContext _context;

    public TeachersController(
        UserManager<Teacher> userManager,
        SignInManager<Teacher> signInManager,
        ApplicationDbContext context)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
          return _context.Teachers != null ? 
                      View(await _context.Teachers.Include(t => t.Level).ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Teachers'  is null.");
    }

    public async Task<IActionResult> Details(string id)
    {
        if (id == null || _context.Teachers == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTeacherAddressModel newTeacher)
    {
        if (ModelState.IsValid)
        {
            var defaultLevel = await this._context.Levels.FirstAsync();
            var user = new Teacher
            {
                ChineseName = newTeacher.ChineseName,
                UserName = newTeacher.Email,
                Email = newTeacher.Email,
                LevelId = defaultLevel.Id,
            };
            var result = await userManager.CreateAsync(user, newTeacher.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(newTeacher);
            }

            return RedirectToAction(nameof(Index));
        }
        return View(newTeacher);
    }

    // GET: Teachers/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || _context.Teachers == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }

        ViewData["LevelId"] = new SelectList(_context.Levels, "Id", nameof(Level.Name));
        return View(teacher);
    }

    // POST: Teachers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, Teacher model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var teacherInDb = await _context.Teachers.FindAsync(id);
                teacherInDb.ChineseName = model.ChineseName;
                teacherInDb.LevelId = model.LevelId;
                _context.Update(teacherInDb);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(model.Id))
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
        ViewData["LevelId"] = new SelectList(_context.Levels, "Id", nameof(Level.Name));
        return View(model);
    }

    // GET: Teachers/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || _context.Teachers == null)
        {
            return NotFound();
        }

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teacher == null)
        {
            return NotFound();
        }

        return View(teacher);
    }

    // POST: Teachers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (_context.Teachers == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Teachers'  is null.");
        }
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher != null)
        {
            _context.Teachers.Remove(teacher);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(string id)
    {
      return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}