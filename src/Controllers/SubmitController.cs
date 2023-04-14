using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        this._context = dbContext;
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

        await _context.TeachEvents.AddAsync(new TeachEvent
        {
            Times = model.Times,
            Comments = model.Comments,
            TeacherId = GetUserId(User),
            IsApproved = false,
            ClassTypeId = model.ClassTypeId,
            SiteId = model.SiteId,
            EventTime =
                model.EventTime == TimeStatus.Today ? DateTime.UtcNow :
                model.EventTime == TimeStatus.Yesterday ? DateTime.UtcNow.AddDays(-1) : throw new InvalidOperationException(),
        });
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), "Home");
    }

    public string GetUserId(ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.NameIdentifier);
}
