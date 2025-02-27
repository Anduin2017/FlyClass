using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anduin.FlyClass.Entities;

public class Site
{
    public int Id { get; init; }
    [Display(Name = "校区名称")]
    [MaxLength(100)]
    public required string SiteName { get; init; }

    [InverseProperty(nameof(TeachEvent.Site))]
    public IEnumerable<TeachEvent> TeachEvents { get; init; } = new List<TeachEvent>();
}