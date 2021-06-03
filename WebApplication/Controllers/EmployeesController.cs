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
            var employees = _repository.Employee.GetAllEmployees(trackChanges: false);

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return Ok(employeesDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = _repository.Employee.GetEmployeeById(id, trackChanges: false);

            if (employee == null)
            {
                var message = string.Format(ErrorMessages.EmployeeNotFound, id);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return Ok(employeeDto);
        }
    }
}
