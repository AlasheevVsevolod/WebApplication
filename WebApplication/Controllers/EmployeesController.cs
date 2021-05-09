using System;
using System.Collections.Generic;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public EmployeesController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IActionResult GetEmployees()
        {
            try
            {
                var employees = _repository.Employee.GetAllEmployees(trackChanges: false);
                var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                return Ok(employeesDto);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetEmployees)} action {e}"); 
                return StatusCode(500, "Internal server error");
            }
        }
    }
}