﻿using System.ComponentModel.DataAnnotations;

namespace Anduin.FlyClass.Models.SubmitViewModels;

public class SubmitIndexViewModel
{
    [Display(Name = "上课日期")]
    [Required]
    public TimeStatus EventTime { get; set; }

    [Display(Name = "选择校区")]
    [Required]
    public int SiteId { get; set; } = 1;

    [Display(Name = "课程类型")]
    [Required]
    public int ClassTypeId { get; set; } = 1;

    [Display(Name = "次数（当日、该课程类型累计上课节数。）")]
    [Required]
    public int Times { get; set; } = 1;

    [Display(Name = "附属备注信息")]
    [Required(ErrorMessage = "稍微描述一下今天教了什么吧。")]
    [MaxLength(30, ErrorMessage = "最多填写 {0} 个字。")]
    public required string Comments { get; set; }
}
