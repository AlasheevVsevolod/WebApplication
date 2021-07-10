using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApplication.ActionFilters;
using WebApplication.Infrastructure.Messages;
using WebApplication.Infrastructure.ModelBinders;
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
        public async Task<IActionResult> GetCompanies()
        {
            var companiesDto = await _companyService.GetAllCompaniesAsync(trackChanges: false);

            return Ok(companiesDto);
        }

        [HttpGet("{companyId}", Name = "GetCompanyById")]
        [Produces(typeof(CompanyDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public IActionResult GetCompanyById(Guid companyId)
        {
            var existingCompany = HttpContext.Items["company"] as Company;

            var company = _companyService.GetCompanyById(existingCompany);

            return Ok(company);
        }

        [HttpGet("({ids})", Name = "GetCompaniesByIds")]
        [Produces(typeof(IEnumerable<CompanyDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompaniesByIds([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (!ids?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.ParametersAreNullOrEmpty);

                _logger.LogInfo(message);

                return BadRequest(message);
            }

            var companies = await _companyService.GetCompaniesByIdsAsync(ids, trackChanges: false);
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
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> GetEmployeesByCompanyId(Guid companyId)
        {
            var employees = await _employeeService.GetAllEmployeesForCompanyAsync(companyId, trackChanges: false);

            return Ok(employees);
        }

        [HttpGet("{companyId}/employees/{employeeId}", Name = "GetEmployeeByCompanyId")]
        [Produces(typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> GetEmployeeByCompanyId(Guid companyId, Guid employeeId)
        {
            var employee = await _employeeService.GetEmployeeForCompanyAsync(companyId, employeeId, trackChanges: false);

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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyForCreationDto company)
        {
            var createdCompany = await _companyService.CreateCompanyAsync(company);

            return CreatedAtRoute("GetCompanyById", new { companyId = createdCompany.Id },
                createdCompany);
        }

        [HttpPost("collection")]
        [Produces(typeof(IEnumerable<CompanyDto>))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompanies([FromBody]IEnumerable<CompanyForCreationDto> companies)
        {
            var createdCompanies = new List<CompanyDto>();
            foreach (var company in companies)
            {
                var newCompany = await _companyService.CreateCompanyAsync(company);
                createdCompanies.Add(newCompany);
            }

            var newCompanyIds = string.Join(',', createdCompanies.Select(c => c.Id));

            return CreatedAtRoute("GetCompaniesByIds", new { ids = newCompanyIds },
                createdCompanies);
        }

        [HttpPost("{companyId}/employees")]
        [Produces(typeof(EmployeeDto))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> CreateEmployee(Guid companyId, [FromBody]EmployeeForCreationDto employee)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee, companyId);

            return CreatedAtRoute("GetEmployeeByCompanyId", new { companyId, employeeId = createdEmployee.Id },
                createdEmployee);
        }

        [HttpPost("{companyId}/employees/collection")]
        [Produces(typeof(IEnumerable<EmployeeDto>))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployees(Guid companyId, [FromBody]IEnumerable<EmployeeForCreationDto> employees)
        {
            var createdEmployees = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                var newEmployee = await _employeeService.CreateEmployeeAsync(employee, companyId);
                createdEmployees.Add(newEmployee);
            }

            var newEmployeeIds = string.Join(',', createdEmployees.Select(c => c.Id));

            return CreatedAtRoute("GetEmployeesByIds", new { ids = newEmployeeIds },
                createdEmployees);
        }

        [HttpDelete("{companyId}")]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var existingCompany = HttpContext.Items["company"] as Company;

            await _companyService.DeleteCompanyAsync(existingCompany);

            return NoContent();
        }

        [HttpPut("{companyId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid companyId, [FromBody]CompanyForUpdateDto company)
        {
            var companyEntity = HttpContext.Items["company"] as Company;

            await _companyService.UpdateCompanyAsync(company, companyEntity);
            return NoContent();
        }

        [HttpPatch("{companyId}")]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateCompany(Guid companyId, [FromBody]JsonPatchDocument<CompanyForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                var message = ErrorMessages.PatchDocIsNull;

                _logger.LogInfo(message);

                return BadRequest(message);
            }

            var existingCompany = HttpContext.Items["company"] as Company;

            await _companyService.PatchCompanyAsync(existingCompany, patchDoc);

            return NoContent();
        }
    }
}
