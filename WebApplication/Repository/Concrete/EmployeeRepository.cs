using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;
using WebApplication.Repository.Interface;

namespace WebApplication.Repository.Concrete
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Employee> GetAllEmployees(bool trackChanges)
        {
            return FindAll(trackChanges)
                .OrderBy(e => e.Name)
                .ToList();
        }

        public Employee GetEmployeeById(Guid employeeId, bool trackChanges)
        {
            return GetSingleById(employeeId, trackChanges);
        }

        public IEnumerable<Employee> GetAllEmployeesForCompany(Guid companyId, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .OrderBy(e => e.Name);
        }

        public Employee GetEmployeeForCompany(Guid companyId, Guid employeeId, bool trackChanges)
        {
            return FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges)
                .SingleOrDefault();
        }
    }
}
