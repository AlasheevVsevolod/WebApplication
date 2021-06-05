using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Infrastructure.Messages;
using WebApplication.Logger;
using WebApplication.Models;
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
        [Produces(typeof(IEnumerable<CompanyDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCompanies()
        {
            var companiesDto = _companyService.GetAllCompanies(trackChanges: false);

            return Ok(companiesDto);
        }

        [HttpGet("{id}", Name = "GetCompanyById")]
        [Produces(typeof(CompanyDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("({ids})", Name = "GetCompaniesByIds")]
        [Produces(typeof(IEnumerable<CompanyDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCompaniesByIds(IEnumerable<Guid> ids)
        {
            if (!ids?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.ParametersAreNullOrEmpty);

                _logger.LogInfo(message);

                return BadRequest(message);
            }

            var companies = _companyService.GetCompaniesByIds(ids, trackChanges: false);
            if (!companies?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.CompaniesNotFound, ids.Select(id => id.ToString()));

                _logger.LogInfo(message);

                return NotFound(message);
            }

            return Ok(companies);
        }

        [HttpGet("{companyId}/employees")]
        [Produces(typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{companyId}/employees/{employeeId}", Name = "GetEmployeeByCompanyId")]
        [Produces(typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmployeeByCompanyId(Guid companyId, Guid employeeId)
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

        [HttpPost]
        [Produces(typeof(CompanyDto))]
        public IActionResult CreateCompany([FromBody]CompanyForCreationDto company)
        {
            if (company == null)
            {
                var message = ErrorMessages.CompanyIsNull;

                _logger.LogError(message);

                return BadRequest(message);
            }

            var createdCompany = _companyService.CreateCompany(company);

            return CreatedAtRoute("GetCompanyById", new { id = createdCompany.Id },
                createdCompany);
        }

        [HttpPost("collection")]
        [Produces(typeof(IEnumerable<CompanyDto>))]
        public IActionResult CreateCompanies([FromBody]IEnumerable<CompanyForCreationDto> companies)
        {
            if (!companies?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.ParametersAreNullOrEmpty);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var createdCompanies = new List<CompanyDto>();
            foreach (var company in companies)
            {
                var newCompany = _companyService.CreateCompany(company);
                createdCompanies.Add(newCompany);
            }

            var newCompanyIds = string.Join(',', createdCompanies.Select(c => c.Id));

            return CreatedAtRoute("GetCompaniesByIds", new { ids = newCompanyIds },
                createdCompanies);
        }

        [HttpPost("{companyId}/employees")]
        [Produces(typeof(EmployeeDto))]
        public IActionResult CreateEmployee(Guid companyId, [FromBody]EmployeeForCreationDto employee)
        {
            if (employee == null)
            {
                var message = ErrorMessages.EmployeeIsNull;

                _logger.LogError(message);

                return BadRequest(message);
            }

            var company = _companyService.GetCompanyById(companyId, trackChanges: false);
            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, companyId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var createdEmployee = _employeeService.CreateEmployee(employee, companyId);

            return CreatedAtRoute("GetEmployeeByCompanyId", new { companyId, employeeId = createdEmployee.Id },
                createdEmployee);
        }

        [HttpPost("{companyId}/employees/collection")]
        [Produces(typeof(IEnumerable<EmployeeDto>))]
        public IActionResult CreateEmployees(Guid companyId, [FromBody]IEnumerable<EmployeeForCreationDto> employees)
        {
            if (!employees?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.ParametersAreNullOrEmpty);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var createdEmployees = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                var newEmployee = _employeeService.CreateEmployee(employee, companyId);
                createdEmployees.Add(newEmployee);
            }

            var newEmployeeIds = string.Join(',', createdEmployees.Select(c => c.Id));

            return CreatedAtRoute("GetEmployeesByIds", new { ids = newEmployeeIds },
                createdEmployees);
        }
    }
}
