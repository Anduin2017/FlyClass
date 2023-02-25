using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlyClass.Models;

public class Teacher : IdentityUser
{
    public int LevelId { get; set; }
    [ForeignKey(nameof(LevelId))]
    public Level Level { get; set; }

    [InverseProperty(nameof(TeachEvent.Teacher))]
    public IEnumerable<TeachEvent> TeachEvents { get; set; } = new List<TeachEvent>();
}

public class Level
{
    public int Id { get; set; }
    public string Name { get; set; }

    [InverseProperty(nameof(Teacher.Level))]
    public IEnumerable<Teacher> Teachers { get; set; } = new List<Teacher>();
}

public class TeachEvent
{
    public int Id { get; set; }

    public DateTime EventTime { get; set; } = DateTime.UtcNow;
    public int Times { get; set; }
    public string Comments { get; set; }

    public string TeacherId { get; set; }
    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; }

    public int SiteId { get; set; }
    [ForeignKey(nameof(SiteId))]
    public Site Site { get; set; }

    public int ClassTypeId { get; set; }
    [ForeignKey(nameof(ClassTypeId))]
    public ClassType ClassType { get; set; }
}

public class Site
{
    public int Id { get; set; }
    public string SiteName { get; set; }

    [InverseProperty(nameof(TeachEvent.Site))]
    public IEnumerable<TeachEvent> TeachEvents { get; set; } = new List<TeachEvent>();
}

public class ClassType
{
    public int Id { get; set; }
    public string Name { get; set; }


    [InverseProperty(nameof(TeachEvent.ClassType))]
    public IEnumerable<TeachEvent> TeachEvents { get; set; } = new List<TeachEvent>();
}

public class MoneyMap
{
    public int Id { get; set; }

    public int LevelId { get; set; }
    [ForeignKey(nameof(LevelId))]
    public Level Level { get; set; }

    public int ClassTypeId { get; set; }
    [ForeignKey(nameof(ClassTypeId))]
    public ClassType ClassType { get; set; }

    public int Bonus { get; set; }
}