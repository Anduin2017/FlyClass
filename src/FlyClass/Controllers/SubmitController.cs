using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FlyClass.Data;
using FlyClass.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FlyClass.Models.SubmitViewModels;
using System.Security.Claims;

namespace FlyClass.Controllers;

[Authorize]
public class SubmitController : Controller
{
    private readonly ApplicationDbContext _context;

    public SubmitController(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ClassTypeId"] = new SelectList(await _context.ClassTypes.ToListAsync(), "Id", nameof(ClassType.Name));
        ViewData["SiteId"] = new SelectList(await _context.Sites.ToListAsync(), "Id", nameof(Site.SiteName));
        return View(new SubmitIndexViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SubmitIndexViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), model);
        }

        var user = await _context.Users.FindAsync(GetUserId(User));

        var money = await _context.MoneyMaps
            .Where(m => m.ClassTypeId == model.ClassTypeId)
            .Where(m => m.LevelId == user.LevelId)
            .SingleAsync();

        await _context.TeachEvents.AddAsync(new TeachEvent
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
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Log));
    }

    public async Task<IActionResult> Log()
    {
        var userId = GetUserId(User);
        var applicationDbContext = _context.TeachEvents
            .Where(e => e.TeacherId == userId)
            .Include(t => t.ClassType)
            .Include(t => t.Site)
            .Include(t => t.Teacher)
            .OrderByDescending(t => t.EventTime);
        return View(await applicationDbContext.ToListAsync());
    }

    public string GetUserId(ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}
