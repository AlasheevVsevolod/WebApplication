using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication.Models;

namespace WebApplication.Services.Interface
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesForCompanyAsync(Guid companyId, bool trackChanges);
        Task<EmployeeDto> GetEmployeeForCompanyAsync(Guid companyId, Guid employeeId, bool trackChanges);
        Task<EmployeeDto> GetEmployeeByIdAsync(Guid employeeId, bool trackChanges);
        EmployeeDto GetEmployeeById(Employee existingEmployee);
        Task<IEnumerable<EmployeeDto>> GetEmployeesByIdsAsync(IEnumerable<Guid> employeeIds, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeForCreationDto employee, Guid companyId);
        Task DeleteEmployeeAsync(Employee existingEmployee);
        Task UpdateEmployeeAsync(EmployeeForUpdateDto employee, Employee existingEmployee);
        Task PatchEmployeeAsync(Employee existingEmployee, JsonPatchDocument<EmployeeForUpdateDto> patchDoc);
    }
}
