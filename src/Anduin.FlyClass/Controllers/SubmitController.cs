using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Anduin.FlyClass.Entities;
using Anduin.FlyClass.Models.SubmitViewModels;

namespace Anduin.FlyClass.Controllers;

[Authorize]
public class SubmitController(FlyClassDbContext dbContext) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewData["ClassTypeId"] = new SelectList(await dbContext.ClassTypes.ToListAsync(), "Id", nameof(ClassType.Name));
        ViewData["SiteId"] = new SelectList(await dbContext.Sites.ToListAsync(), "Id", nameof(Site.SiteName));
        return View(new SubmitIndexViewModel
        {
            Comments = string.Empty
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SubmitIndexViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), model);
        }

        var user = await dbContext.Users.FindAsync(GetUserId(User));

        var money = await dbContext.MoneyMaps
            .Where(m => m.ClassTypeId == model.ClassTypeId)
            .Where(m => m.LevelId == user!.LevelId)
            .SingleAsync();

        await dbContext.TeachEvents.AddAsync(new TeachEvent
        {
            Times = model.Times,
            Comments = model.Comments,
            TeacherId = GetUserId(User),
            IsApproved = false,
            ClassTypeId = model.ClassTypeId,
            SiteId = model.SiteId,
            EventTime =
                model.EventTime == TimeStatus.Today ? DateTime.UtcNow.Date :
                model.EventTime == TimeStatus.Yesterday ? DateTime.UtcNow.Date.AddDays(-1) : throw new InvalidOperationException(),
            MoneyPaid = money.Bonus * model.Times
        });
        await dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Log));
    }

    public async Task<IActionResult> Log()
    {
        var userId = GetUserId(User);
        var applicationDbContext = dbContext.TeachEvents
            .Where(e => e.TeacherId == userId)
            .Include(t => t.ClassType)
            .Include(t => t.Site)
            .Include(t => t.Teacher)
            .OrderByDescending(t => t.EventTime);
        return View(await applicationDbContext.ToListAsync());
    }

    public string GetUserId(ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier)!;
}
