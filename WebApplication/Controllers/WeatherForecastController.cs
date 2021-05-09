using System.Collections.Generic;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public WeatherForecastController(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<string> GetCompanies()
        {
            var employees = _repository.Employee.GetAllEmployees(trackChanges: false);
            var companies = _repository.Company.GetAllCompanies(trackChanges: false);
            
            return new []{employees, companies}
        }
    }
}