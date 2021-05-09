namespace WebApplication.Repository
{
    public interface IRepositoryManager
    {
        WebApplication.Repository.ICompanyRepository Company { get; }
        WebApplication.Repository.IEmployeeRepository Employee { get; }
        void Save();
    }
}