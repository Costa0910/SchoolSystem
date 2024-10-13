namespace SchoolSystem.Web.Areas.Staff.Helpers;

public class ViewStudentGradeModel
{
  public string SubjectName { get; set; }
  public double Grade { get; set; }
  public string Status { get; set; }
  public int Absents { get; set; }
  public double AttendancePercentage { get; set; }
  public bool IsExcluded { get; set; }
  public int ExpectedHours { get; set; }
}
