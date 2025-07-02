namespace HRManagementSystem.BL.Helpers
{
    public static class DefaultUserValues
    {
        public const string Nationality = "Unknown";
        public const string NationalId = "00000000000000";
        public const string Address = "Not Provided";
        public const decimal Salary = 3000;

        public static readonly DateTime DateOfBirth = DateTime.UtcNow.AddYears(-25);
        public static readonly DateTime ContractDate = DateTime.UtcNow.Date;
        public static readonly DateTime StartTime = DateTime.Today.AddHours(9);
        public static readonly DateTime EndTime = DateTime.Today.AddHours(17);
        public const int DefaultDepartmentId = 1; 
    }
}
