using System.ComponentModel.DataAnnotations;

namespace FlyClass.Models.TeachersViewModels;

public class EditTeacherViewModel
{
    [Display(Name = "中文姓名")]
    public string ChineseName { get; set; }

    [Display(Name = "教师等级")]
    public int LevelId { get; set; }

    public string Id { get; set; }
    [Display(Name = "具有管理员权限（能够修改数据库和审批课程）")]
    public bool IsAdmin { get; set; }

    [Display(Name = "具有审批员权限（能够审批课程记录）")]
    public bool IsReviewer { get; set; }
}
