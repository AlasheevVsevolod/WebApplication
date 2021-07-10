using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using WebApplication.Models;

namespace WebApplication.Services.Interface
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
        Task<CompanyDto> GetCompanyByIdAsync(Guid companyId, bool trackChanges);
        Task<IEnumerable<CompanyDto>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds, bool trackChanges);
        Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
        Task DeleteCompanyAsync(Company existingCompany);
        Task UpdateCompanyAsync(CompanyForUpdateDto company, Company existingCompany);
        Task PatchCompanyAsync(Company existingCompany, JsonPatchDocument<CompanyForUpdateDto> patchDoc);
    }
}
