using System.ComponentModel.DataAnnotations;

namespace Anduin.FlyClass.Models.TeachersViewModels;

public class CreateTeacherAddressModel
{
    [Display(Name = "中文姓名")]
    public required string ChineseName { get; set; }
    [EmailAddress]
    [Display(Name = "Email地址")]
    public required string Email { get; set; }
    [Display(Name = "密码")]
    public required string Password { get; set; }
}
