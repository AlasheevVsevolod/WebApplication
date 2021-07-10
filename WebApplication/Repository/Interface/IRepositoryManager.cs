using System.Threading.Tasks;

namespace WebApplication.Repository.Interface
{
    public interface IRepositoryManager
    {
        ICompanyRepository Company { get; }
        IEmployeeRepository Employee { get; }
        Task Save();
    }
}
