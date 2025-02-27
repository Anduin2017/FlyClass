using System.ComponentModel.DataAnnotations;

namespace Anduin.FlyClass.Models.AccountViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name ="Email")]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name ="密码")]
    public required  string Password { get; set; }
}
