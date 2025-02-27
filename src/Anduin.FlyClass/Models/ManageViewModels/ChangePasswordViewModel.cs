using System.ComponentModel.DataAnnotations;

namespace Anduin.FlyClass.Models.ManageViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "当前密码")]
    public required string OldPassword { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "新密码")]
    public required string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "确认新密码")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }
}
