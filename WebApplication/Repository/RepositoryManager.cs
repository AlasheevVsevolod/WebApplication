﻿namespace WebApplication.Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private CompanyRepository _companyRepository;
        private EmployeeRepository _employeeRepository;
        
        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }

        public ICompanyRepository Company
        {
            get
            {
                if (_companyRepository == null)
                {
                    _companyRepository = new CompanyRepository(_context);
                }

                return _companyRepository;
            }
        }
        
        public IEmployeeRepository Employee
        {
            get
            {
                if (_employeeRepository == null)
                {
                    _employeeRepository = new EmployeeRepository(_context);
                }

                return _employeeRepository;
            }
        }

        public void Save() => _context.SaveChanges();
    }
}