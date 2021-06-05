using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repository.Interface
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company GetCompanyById(Guid companyId, bool trackChanges);
        IEnumerable<Company> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges);
        void CreateCompany(Company company);
    }
}
