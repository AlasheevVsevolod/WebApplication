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

        /// <summary>
        /// Gets all employees without any filter
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all employees</response>
        [HttpGet]
        [Produces(typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetEmployees()
        {
            var employees = _employeeService.GetAllEmployees(trackChanges: false);

            return Ok(employees);
        }

        /// <summary>
        /// Gets employee, based on the give Id
        /// </summary>
        /// <param name="id">Id of the employee to search by</param>
        /// <returns></returns>
        /// <response code="200">Returns found employee</response>
        /// <response code="404">If employee wasn't found</response>
        [HttpGet("{id}", Name = "GetEmployeeById")]
        [Produces(typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("({ids})", Name = "GetEmployeesByIds")]
        [Produces(typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmployeesByIds(IEnumerable<Guid> ids)
        {
            if (!ids?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.ParametersAreNullOrEmpty);

                _logger.LogInfo(message);

                return BadRequest(message);
            }

            var employees = _employeeService.GetEmployeesByIds(ids, trackChanges: false);
            if (!employees?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.EmployeesNotFound, ids.Select(id => id.ToString()));

                _logger.LogInfo(message);

                return NotFound(message);
            }

            return Ok(employees);
        }
    }
}
