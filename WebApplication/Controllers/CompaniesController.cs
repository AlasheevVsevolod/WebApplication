using System;
using System.Collections.Generic;
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

        [HttpPost]
        [Produces(typeof(CompanyDto))]
        public IActionResult CreateCompany(CompanyForCreationDto company)
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
    }
}
