using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication.Models;
using WebApplication.Repository.Interface;

namespace WebApplication.Repository.Concrete
{
    public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
    {
        public CompanyRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Company> GetAllCompanies(bool trackChanges)
        {
            return FindAll(trackChanges)
                .OrderBy(c => c.Name)
                .ToList();
        }

        public Company GetCompanyById(Guid companyId, bool trackChanges)
        {
            return GetSingleById(companyId, trackChanges);
        }

        public void CreateCompany(Company company)
        {
            Create(company);
        }

        public IEnumerable<Company> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges)
        {
            return GetMultipleByIds(companyIds, trackChanges);
        }
    }
}
