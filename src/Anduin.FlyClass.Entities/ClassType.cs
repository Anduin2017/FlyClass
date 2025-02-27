using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anduin.FlyClass.Entities;

public class ClassType
{
    public int Id { get; init; }
    [Display(Name = "课程类型")]
    [MaxLength(100)]
    public required string Name { get; init; }


    [InverseProperty(nameof(TeachEvent.ClassType))]
    public IEnumerable<TeachEvent> TeachEvents { get; init; } = new List<TeachEvent>();
}