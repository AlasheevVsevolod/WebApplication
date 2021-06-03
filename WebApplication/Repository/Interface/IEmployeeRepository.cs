using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repository.Interface
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);
        Employee GetEmployeeById(Guid employeeId, bool trackChanges);
    }
}
