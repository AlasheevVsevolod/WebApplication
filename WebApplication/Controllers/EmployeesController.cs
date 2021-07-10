using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync(trackChanges: false);

            return Ok(employees);
        }

        /// <summary>
        /// Gets employee, based on the give Id
        /// </summary>
        /// <param name="employeeId">Id of the employee to search by</param>
        /// <returns></returns>
        /// <response code="200">Returns found employee</response>
        /// <response code="404">If employee wasn't found</response>
        [HttpGet("{employeeId}", Name = "GetEmployeeById")]
        [Produces(typeof(EmployeeDto))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeById(Guid employeeId)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(employeeId, trackChanges: false);

            if (employee == null)
            {
                var message = string.Format(ErrorMessages.EmployeeNotFound, employeeId);

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
        public async Task<IActionResult> GetEmployeesByIds([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (!ids?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.ParametersAreNullOrEmpty);

                _logger.LogInfo(message);

                return BadRequest(message);
            }

            var employees = await _employeeService.GetEmployeesByIdsAsync(ids, trackChanges: false);
            if (!employees?.Any() ?? true)
            {
                var message = string.Format(ErrorMessages.EmployeesNotFound, ids.Select(id => id.ToString()));

                _logger.LogInfo(message);

                return NotFound(message);
            }

            return Ok(employees);
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(Guid employeeId)
        {
            await _employeeService.DeleteEmployeeAsync(employeeId);

            return NoContent();
        }

        [HttpPut("{employeeId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid employeeId, [FromBody] EmployeeForUpdateDto employee)
        {
            var employeeEntity = await _employeeService.GetEmployeeByIdAsync(employeeId, trackChanges: false);
            if (employeeEntity == null)
            {
                var message = string.Format(ErrorMessages.EmployeeNotFound, employeeId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            await _employeeService.UpdateEmployeeAsync(employee, employeeId);

            return NoContent();
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmployeeForCompany(Guid employeeId, [FromBody]JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                var message = ErrorMessages.PatchDocIsNull;

                _logger.LogInfo(message);

                return BadRequest(message);
            }

            var employeeEntity = await _employeeService.GetEmployeeByIdAsync(employeeId, trackChanges: false);
            if (employeeEntity == null)
            {
                var message = string.Format(ErrorMessages.EmployeeNotFound, employeeId);

                _logger.LogInfo(message);

                return NotFound(message);
            }

            await _employeeService.PatchEmployeeAsync(employeeId, patchDoc);

            return NoContent();
        }
    }
}
