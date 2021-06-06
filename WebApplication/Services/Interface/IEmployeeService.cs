using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Services.Interface
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployees(bool trackChanges);
        IEnumerable<EmployeeDto> GetAllEmployeesForCompany(Guid companyId, bool trackChanges);
        EmployeeDto GetEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges);
        EmployeeDto GetEmployeeById(Guid employeeId, bool trackChanges);
        IEnumerable<EmployeeDto> GetEmployeesByIds(IEnumerable<Guid> employeeIds, bool trackChanges);
        EmployeeDto CreateEmployee(EmployeeForCreationDto employee, Guid companyId);
        void DeleteEmployee(Guid employeeId);
    }
}
