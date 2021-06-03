using System;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Infrastructure.Messages;
using WebApplication.Logger;
using WebApplication.Services.Interface;

namespace WebApplication.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(ILoggerManager logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _employeeService.GetAllEmployees(trackChanges: false);

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(Guid id)
        {
            var employee = _employeeService.GetEmployeeById(id, trackChanges: false);

            if (employee == null)
            {
                var message = string.Format(ErrorMessages.EmployeeNotFound, id);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            return Ok(employee);
        }
    }
}
