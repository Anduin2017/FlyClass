using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlyClass.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name ="Email")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name ="密码")]
    public string Password { get; set; }
}
