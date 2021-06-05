using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Services.Interface
{
    public interface ICompanyService
    {
        IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
        CompanyDto GetCompanyById(Guid companyId, bool trackChanges);
        IEnumerable<CompanyDto> GetCompaniesByIds(IEnumerable<Guid> companyIds, bool trackChanges);
        CompanyDto CreateCompany(CompanyForCreationDto company);
    }
}
