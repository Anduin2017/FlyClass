using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Anduin.FlyClass.Entities;

public class MoneyMap
{
    public int Id { get; init; }

    [Display(Name = "教师等级")]
    public int LevelId { get; init; }
    [ForeignKey(nameof(LevelId))]
    [Display(Name = "教师等级")]
    public Level? Level { get; init; }

    [Display(Name = "课程类型")]
    public int ClassTypeId { get; init; }
    [ForeignKey(nameof(ClassTypeId))]
    [Display(Name = "课程类型")]
    public ClassType? ClassType { get; init; }

    [Display(Name = "课时费")]
    public int Bonus { get; init; }
}
