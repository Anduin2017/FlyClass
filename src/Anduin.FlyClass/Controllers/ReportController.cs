using Anduin.FlyClass.Data;
using Anduin.FlyClass.Models.ReportViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Anduin.FlyClass.Controllers;

[Authorize(Roles = "Admin")]
public class ReportController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportController(ApplicationDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<IActionResult> Index([FromQuery]DateTime? start = null, [FromQuery]DateTime? end = null)
    {
        start ??= DateTime.MinValue;
        end ??= DateTime.MaxValue;

        var allEvents = await _context.TeachEvents
            .Include(t => t.Teacher)
            .Include(t => t.Site)
            .Where(t => t.EventTime >= start)
            .Where(t => t.EventTime <= end)
            .ToListAsync();

        return View(new ReportViewModel 
        {
            PaidByPerson = allEvents.GroupBy(t => t.TeacherId).Where(t => t.Any()).ToList(),
            PaidBySite = allEvents.GroupBy(t => t.SiteId).Where(t => t.Any()).ToList(),
        });
    }
}
