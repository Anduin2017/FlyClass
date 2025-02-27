using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Anduin.FlyClass.Entities;

public class Teacher : IdentityUser
{
    [Display(Name = "中文姓名")]
    [MaxLength(100)]
    public required string ChineseName { get; set; }

    [Display(Name = "教师等级")]
    public required int LevelId { get; set; }
    [ForeignKey(nameof(LevelId))]
    [Display(Name = "教师等级")]
    public Level? Level { get; init; }

    [InverseProperty(nameof(TeachEvent.Teacher))]
    public IEnumerable<TeachEvent> TeachEvents { get; init; } = new List<TeachEvent>();
}
