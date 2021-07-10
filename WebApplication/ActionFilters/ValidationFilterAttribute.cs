using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication.Infrastructure.Messages;
using WebApplication.Logger;

namespace WebApplication.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManager _logger;

        public ValidationFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            var param = context.ActionArguments
                .SingleOrDefault(a => a.Value.ToString().Contains("Dto")).Value;

            if (param == null)
            {
                var message = string.Format(ErrorMessages.ObjectIsNull, controller, action);

                _logger.LogError(message);
                context.Result = new BadRequestObjectResult(message);

                return;
            }

            if (!context.ModelState.IsValid)
            {
                var message = string.Format(ErrorMessages.InvalidModelState, controller, action);

                _logger.LogError(message);
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
