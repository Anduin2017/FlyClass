using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anduin.FlyClass.Models;

public class Teacher : IdentityUser
{
    [Display(Name = "中文姓名")]
    public string ChineseName { get; set; }

    [Display(Name = "教师等级")]
    public int LevelId { get; set; }
    [ForeignKey(nameof(LevelId))]
    [Display(Name = "教师等级")]
    public Level Level { get; set; }

    [InverseProperty(nameof(TeachEvent.Teacher))]
    public IEnumerable<TeachEvent> TeachEvents { get; set; } = new List<TeachEvent>();
}

public class Level
{
    public int Id { get; set; }
    [Display(Name = "教师等级名称")]
    public string Name { get; set; }

    [InverseProperty(nameof(Teacher.Level))]
    public IEnumerable<Teacher> Teachers { get; set; } = new List<Teacher>();
}

public class TeachEvent
{
    public int Id { get; set; }
    [Display(Name = "上课时间")]
    public DateTime EventTime { get; set; } = DateTime.UtcNow;
    [Display(Name = "上课次数")]
    public int Times { get; set; }
    [Display(Name = "备注")]
    public string Comments { get; set; }

    [Display(Name = "任课教师")]
    public string TeacherId { get; set; }
    [Display(Name = "任课教师")]
    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; }

    [Display(Name = "校区")]
    public int SiteId { get; set; }
    [Display(Name = "校区")]
    [ForeignKey(nameof(SiteId))]
    public Site Site { get; set; }

    [Display(Name = "课程类型")]
    public int ClassTypeId { get; set; }
    [ForeignKey(nameof(ClassTypeId))]
    [Display(Name = "课程类型")]
    public ClassType ClassType { get; set; }

    [Display(Name = "已经审批通过")]
    public bool IsApproved { get; set; }

    [Display(Name = "课时费")]
    public int MoneyPaid { get; set; }
}

public class Site
{
    public int Id { get; set; }
    [Display(Name = "校区名称")]
    public string SiteName { get; set; }

    [InverseProperty(nameof(TeachEvent.Site))]
    public IEnumerable<TeachEvent> TeachEvents { get; set; } = new List<TeachEvent>();
}

public class ClassType
{
    public int Id { get; set; }
    [Display(Name = "课程类型")]
    public string Name { get; set; }


    [InverseProperty(nameof(TeachEvent.ClassType))]
    public IEnumerable<TeachEvent> TeachEvents { get; set; } = new List<TeachEvent>();
}

public class MoneyMap
{
    public int Id { get; set; }

    [Display(Name = "教师等级")]
    public int LevelId { get; set; }
    [ForeignKey(nameof(LevelId))]
    [Display(Name = "教师等级")]
    public Level Level { get; set; }

    [Display(Name = "课程类型")]
    public int ClassTypeId { get; set; }
    [ForeignKey(nameof(ClassTypeId))]
    [Display(Name = "课程类型")]
    public ClassType ClassType { get; set; }

    [Display(Name = "课时费")]
    public int Bonus { get; set; }
}