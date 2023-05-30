using Microsoft.AspNetCore.Mvc;
using FlyClass.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using FlyClass.Models.ReportViewModels;

namespace FlyClass.Controllers;

[Authorize(Roles = "Admin")]
public class ReportController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportController(ApplicationDbContext dbContext)
    {
        this._context = dbContext;
    }

    public async Task<IActionResult> Index([FromQuery]DateTime? start = null, [FromQuery]DateTime? end = null)
    {
        if (start == null)
        {
            start = DateTime.MinValue;
        }
        if (end == null)
        {
            end = DateTime.MaxValue;
        }

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
