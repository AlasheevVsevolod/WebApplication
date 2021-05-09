﻿using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Repository
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
    }
}