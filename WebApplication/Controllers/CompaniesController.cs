using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Infrastructure.Messages;
using WebApplication.Logger;
using WebApplication.Models;
using WebApplication.Repository.Interface;

namespace WebApplication.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CompaniesController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            var companies = _repository.Company.GetAllCompanies(trackChanges: false);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companiesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetCompanyById(Guid id)
        {
            var company = _repository.Company.GetCompanyById(id, trackChanges: false);

            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, id);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var companyDto = _mapper.Map<CompanyDto>(company);

            return Ok(companyDto);
        }

        [HttpGet("{companyId}/employees")]
        public IActionResult GetEmployeesByCompanyId(Guid companyId)
        {
            var company = _repository.Company.GetCompanyById(companyId, trackChanges: false);
            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, companyId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var employees = _repository.Employee.GetAllEmployeesForCompany(companyId, trackChanges: false);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeesDto);
        }

        [HttpGet("{companyId}/employees/{employeeId}")]
        public IActionResult GetEmployeesByCompanyId(Guid companyId, Guid employeeId)
        {
            var company = _repository.Company.GetCompanyById(companyId, trackChanges: false);
            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, companyId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var employee = _repository.Employee.GetEmployeeForCompany(companyId, employeeId, trackChanges: false);

            if (employee == null)
            {
                var message = string.Format(ErrorMessages.EmployeeNotFound, employeeId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);
        }
    }
}
