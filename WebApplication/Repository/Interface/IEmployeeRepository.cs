using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);
        IEnumerable<Employee> GetAllEmployeesForCompany(Guid companyId, bool trackChanges);
        Employee GetEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges);
        Employee GetEmployeeById(Guid employeeId, bool trackChanges);
    }
}
