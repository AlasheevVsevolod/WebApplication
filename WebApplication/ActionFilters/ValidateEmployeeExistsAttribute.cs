using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication.Infrastructure.Messages;
using WebApplication.Logger;
using WebApplication.Repository.Interface;

namespace WebApplication.ActionFilters
{
    public class ValidateEmployeeExistsAttribute : IAsyncActionFilter
    {
        private readonly IEnumerable<string> _methodsAllowedToUpdate = new[] {"PUT", "PATCH" };
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ValidateEmployeeExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = _methodsAllowedToUpdate.Any(m => context.HttpContext.Request.Method.Equals(m));
            var employeeId = (Guid) context.ActionArguments["employeeId"];

            var employee = await _repository.Employee.GetEmployeeByIdAsync(employeeId, trackChanges);

            if (employee == null)
            {
                var message = string.Format(ErrorMessages.EmployeeNotFound, employeeId);

                _logger.LogInfo(message);

                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("employee", employee);
                await next();
            }
        }
    }
}
