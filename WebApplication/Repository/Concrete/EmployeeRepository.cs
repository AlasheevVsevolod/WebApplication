using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid employeeId, bool trackChanges)
        {
            return await GetSingleById(employeeId, trackChanges);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesForCompanyAsync(Guid companyId, bool trackChanges)
        {
            return await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
                .OrderBy(e => e.Name)
                .ToListAsync();
        }

        public async Task<Employee> GetEmployeeForCompanyAsync(Guid companyId, Guid employeeId, bool trackChanges)
        {
            return await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges)
                .SingleOrDefaultAsync();
        }

        public void CreateEmployee(Employee employee)
        {
            Create(employee);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByIdsAsync(IEnumerable<Guid> employeeIds, bool trackChanges)
        {
            return await GetMultipleByIds(employeeIds, trackChanges);
        }

        public void DeleteEmployee(Employee employee)
        {
            Delete(employee);
        }
    }
}
