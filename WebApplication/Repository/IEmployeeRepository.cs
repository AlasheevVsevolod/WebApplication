using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);
    }
}