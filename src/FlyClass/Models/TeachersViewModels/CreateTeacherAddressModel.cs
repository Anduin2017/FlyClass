using System.ComponentModel.DataAnnotations;

namespace FlyClass.Models.TeachersViewModels;

public class CreateTeacherAddressModel
{
    [Display(Name = "中文姓名")]
    public string ChineseName { get; set; }
    [EmailAddress]
    [Display(Name = "Email地址")]
    public string Email { get; set; }
    [Display(Name = "密码")]
    public string Password { get; set; }
}
