using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository.Interface
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges);
        Task<IEnumerable<Employee>> GetAllEmployeesForCompanyAsync(Guid companyId, bool trackChanges);
        Task<Employee> GetEmployeeForCompanyAsync(Guid companyId, Guid employeeId, bool trackChanges);
        Task<Employee> GetEmployeeByIdAsync(Guid employeeId, bool trackChanges);
        Task<IEnumerable<Employee>> GetEmployeesByIdsAsync(IEnumerable<Guid> employeeId, bool trackChanges);
        void CreateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
