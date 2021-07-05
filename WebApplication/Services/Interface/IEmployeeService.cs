using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
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
        void UpdateEmployee(EmployeeForUpdateDto employee, Guid employeeId);
        void PatchEmployee(Guid employeeId, JsonPatchDocument<EmployeeForUpdateDto> patchDoc);
    }
}
