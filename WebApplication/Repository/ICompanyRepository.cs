using System.Collections.Generic;
using WebApplication.Models;

namespace WebApplication.Repository
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAllCompanies(bool trackChanges);
    }
}