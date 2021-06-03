using System;
using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repository.Interface
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
        Company GetCompanyById(Guid companyId, bool trackChanges);
    }
}
