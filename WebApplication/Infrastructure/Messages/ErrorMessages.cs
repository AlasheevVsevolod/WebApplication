namespace WebApplication.Infrastructure.Messages
{
    public static class ErrorMessages
    {
        public const string CompanyNotFound = "Company with id: {0} doesn't exist in the database.";
        public const string EmployeeNotFound = "Employee with id: {0} doesn't exist in the database.";
        public const string CompanyDontHaveEmployee = "Employee with id: {0} wasn't found in the company with id: {1}.";
        public const string CompanyIsNull = "Company is null.";
        public const string EmployeeIsNull = "Employee is null.";
        public const string CompaniesNotFound = "None of the companies with the specified ids were found. Ids are: {0}";
        public const string EmployeesNotFound = "None of the employees with the specified ids were found. Ids are: {0}";
        public const string ParametersAreNullOrEmpty = "Specified collection of parameters is null or empty";
        public const string PatchDocIsNull = "Patch document is null";
        public const string InvalidModelState = "Invalid model state for the object";
    }
}
