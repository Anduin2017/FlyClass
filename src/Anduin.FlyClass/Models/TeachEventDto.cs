using Aiursoft.CSTools.Attributes;

namespace Anduin.FlyClass.Models;

public class TeachEventDto
{
    [CsvProperty("上课时间")] public DateTime EventTime { get; set; }
    [CsvProperty("上课次数")] public int Times { get; set; }
    [CsvProperty("备注")] public string Comments { get; set; }
    [CsvProperty("任课教师")] public string TeacherName { get; set; }
    [CsvProperty("校区")] public string SiteName { get; set; }
    [CsvProperty("课程类型")] public string ClassTypeName { get; set; }
    [CsvProperty("已经审批通过")] public bool IsApproved { get; set; }
    [CsvProperty("课时费")] public int MoneyPaid { get; set; }
}