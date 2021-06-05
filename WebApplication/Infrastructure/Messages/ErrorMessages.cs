namespace WebApplication.Infrastructure.Messages
{
    public static class ErrorMessages
    {
        public const string CompanyNotFound = "Company with id: {0} doesn't exist in the database.";
        public const string EmployeeNotFound = "Employee with id: {0} doesn't exist in the database.";
        public const string CompanyDontHaveEmployee = "Employee with id: {0} wasn't found in the company with id: {1}.";
        public const string CompanyIsNull = "Company is null.";
        public const string EmployeeIsNull = "Employee is null.";
    }
}
