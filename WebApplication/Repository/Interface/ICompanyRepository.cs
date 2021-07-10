using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Repository.Interface
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
        Task<Company> GetCompanyByIdAsync(Guid companyId, bool trackChanges);
        Task<IEnumerable<Company>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges);
        void CreateCompany(Company company);
        void DeleteCompany(Company company);
    }
}
