using System;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Infrastructure.Messages;
using WebApplication.Logger;
using WebApplication.Services.Interface;

namespace WebApplication.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;


        public CompaniesController(ILoggerManager logger, ICompanyService companyService,  IEmployeeService employeeService)
        {
            _logger = logger;
            _companyService = companyService;
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companiesDto = _companyService.GetAllCompanies(trackChanges: false);

            return Ok(companiesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetCompanyById(Guid id)
        {
            var company = _companyService.GetCompanyById(id, trackChanges: false);

            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, id);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            return Ok(company);
        }

        [HttpGet("{companyId}/employees")]
        public IActionResult GetEmployeesByCompanyId(Guid companyId)
        {
            var company = _companyService.GetCompanyById(companyId, trackChanges: false);
            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, companyId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var employees = _employeeService.GetAllEmployeesForCompany(companyId, trackChanges: false);

            return Ok(employees);
        }

        [HttpGet("{companyId}/employees/{employeeId}")]
        public IActionResult GetEmployeesByCompanyId(Guid companyId, Guid employeeId)
        {
            var company = _companyService.GetCompanyById(companyId, trackChanges: false);
            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, companyId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var employee = _employeeService.GetEmployeeForCompany(companyId, employeeId, trackChanges: false);

            if (employee == null)
            {
                var message = string.Format(ErrorMessages.CompanyDontHaveEmployee, employeeId, companyId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            return Ok(employee);
        }
    }
}
