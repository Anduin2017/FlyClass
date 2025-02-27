using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Anduin.FlyClass.Entities;

public class TeachEvent
{
    public int Id { get; init; }
    [Display(Name = "上课时间")]
    public DateTime EventTime { get; init; } = DateTime.UtcNow;
    [Display(Name = "上课次数")]
    public int Times { get; init; }
    [Display(Name = "备注")]
    [MaxLength(100)]
    public required string Comments { get; init; }

    [Display(Name = "任课教师")]
    [MaxLength(100)]
    public required string TeacherId { get; init; }
    [Display(Name = "任课教师")]
    [ForeignKey(nameof(TeacherId))]
    [NotNull]
    public Teacher? Teacher { get; init; }

    [Display(Name = "校区")]
    public required int SiteId { get; init; }
    [Display(Name = "校区")]
    [ForeignKey(nameof(SiteId))]
    [NotNull]
    public Site? Site { get; init; }

    [Display(Name = "课程类型")]
    public required int ClassTypeId { get; init; }
    [ForeignKey(nameof(ClassTypeId))]
    [Display(Name = "课程类型")]
    [NotNull]
    public ClassType? ClassType { get; init; }

    [Display(Name = "已经审批通过")]
    public bool IsApproved { get; init; }

    [Display(Name = "课时费")]
    public int MoneyPaid { get; init; }

    public TeachEventDto ToDto()
    {
        return new TeachEventDto
        {
            EventTime = EventTime,
            Times = Times,
            Comments = Comments,
            TeacherName = Teacher.ChineseName,
            SiteName = Site.SiteName,
            ClassTypeName = ClassType.Name,
            IsApproved = IsApproved,
            MoneyPaid = MoneyPaid
        };
    }
}
