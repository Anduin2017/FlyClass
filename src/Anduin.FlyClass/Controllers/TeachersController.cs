using Anduin.FlyClass.Entities;
using Anduin.FlyClass.Models.TeachersViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Anduin.FlyClass.Controllers;

[Authorize(Roles = "Admin")]
public class TeachersController(
    UserManager<Teacher> userManager,
    FlyClassDbContext context)
    : Controller
{
    public async Task<IActionResult> Index()
    {
          return View(await context.Teachers.Include(t => t.Level).ToListAsync());
    }

    public async Task<IActionResult> Details(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teacher = await context.Teachers
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
            var defaultLevel = await context.Levels.FirstAsync();
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
    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teacher = await context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return NotFound();
        }

        ViewData["LevelId"] = new SelectList(context.Levels, "Id", nameof(Level.Name));
        return View(new EditTeacherViewModel
        {
            Id = id,
            ChineseName = teacher.ChineseName,
            Email = teacher.Email!,
            LevelId = teacher.LevelId,
            IsAdmin = await userManager.IsInRoleAsync(teacher, "Admin"),
            IsReviewer = await userManager.IsInRoleAsync(teacher, "Reviewer"),
            Password = "you-cant-read-it"
        });
    }

    // POST: Teachers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EditTeacherViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var teacherInDb = await context.Teachers.FindAsync(id);
                if (teacherInDb == null)
                {
                    return NotFound();
                }

                teacherInDb.ChineseName = model.ChineseName;
                teacherInDb.LevelId = model.LevelId;
                teacherInDb.Email = model.Email;
                if (model.IsAdmin)
                {
                    await userManager.AddToRoleAsync(teacherInDb, "Admin");
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(teacherInDb, "Admin");
                }

                if (model.IsReviewer)
                {
                    await userManager.AddToRoleAsync(teacherInDb, "Reviewer");
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(teacherInDb, "Reviewer");
                }

                context.Update(teacherInDb);
                await context.SaveChangesAsync();

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(teacherInDb);
                    await userManager.ResetPasswordAsync(teacherInDb, token, model.Password);
                }
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
        ViewData["LevelId"] = new SelectList(context.Levels, "Id", nameof(Level.Name));
        return View(model);
    }

    // GET: Teachers/Delete/5
    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var teacher = await context.Teachers
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
        var teacher = await context.Teachers.FindAsync(id);
        if (teacher == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Teachers'  is null.");
        }

        await userManager.RemoveFromRoleAsync(teacher, "Admin");
        context.Teachers.Remove(teacher);

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(string id)
    {
        return context.Teachers.Any(e => e.Id == id);
    }
}
