namespace SchoolSystem.Web.Models.EnumsClasses;

public static class StaffType
{
    public const string Teacher = "Teacher";
    public const string Coordinator = "Staff";
    public const string Principal = "Principal";

    public static List<string> GetStaffTypes()
    {
        return new List<string>
        {
            Teacher,
            Coordinator,
            Principal
        };
    }
}
