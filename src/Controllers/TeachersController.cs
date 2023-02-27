using System;
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

    // GET: Teachers
    public async Task<IActionResult> Index()
    {
          return _context.Teachers != null ? 
                      View(await _context.Teachers.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Teachers'  is null.");
    }

    // GET: Teachers/Details/5
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

    // GET: Teachers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Teachers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Teacher teacher)
    {
        if (ModelState.IsValid)
        {
            var defaultLevel = await this._context.Levels.FirstAsync();
            var user = new Teacher
            {
                ChineseName = teacher.ChineseName,
                UserName = teacher.Email,
                Email = teacher.Email,
                LevelId = defaultLevel.Id,
            };
            var result = await userManager.CreateAsync(user, teacher.PasswordHash);
            if (result.Errors.Any())
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(teacher);
            }

            return RedirectToAction(nameof(Index));
        }
        return View(teacher);
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
