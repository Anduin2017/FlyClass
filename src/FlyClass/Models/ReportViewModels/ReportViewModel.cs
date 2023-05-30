using System.ComponentModel.DataAnnotations;

namespace FlyClass.Models.ReportViewModels;

public class ReportViewModel
{
    [Display(Name = "查询开始时间")]
    public DateTime? Start { get; set; } = null;
    [Display(Name = "查询结束时间")]
    public DateTime? End { get; set; } = null;

    public List<IGrouping<string, TeachEvent>> PaidByPerson { get; set; }
    public List<IGrouping<int, TeachEvent>> PaidBySite { get; set; }
}
