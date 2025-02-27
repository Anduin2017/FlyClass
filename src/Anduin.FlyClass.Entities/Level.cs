using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anduin.FlyClass.Entities;

public class Level
{
    public int Id { get; init; }
    [Display(Name = "教师等级名称")]
    [MaxLength(100)]
    public required string Name { get; init; }

    [InverseProperty(nameof(Teacher.Level))]
    public IEnumerable<Teacher> Teachers { get; init; } = new List<Teacher>();
}