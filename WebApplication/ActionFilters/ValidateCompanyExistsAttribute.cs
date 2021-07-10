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
    public class ValidateCompanyExistsAttribute : IAsyncActionFilter
    {
        private readonly IEnumerable<string> _methodsAllowedToUpdate = new[] {"PUT", "PATCH" };
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ValidateCompanyExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = _methodsAllowedToUpdate.Any(m => context.HttpContext.Request.Method.Equals(m));
            var companyId = (Guid) context.ActionArguments["companyId"];

            var company = await _repository.Company.GetCompanyByIdAsync(companyId, trackChanges);

            if (company == null)
            {
                var message = string.Format(ErrorMessages.CompanyNotFound, companyId);

                _logger.LogInfo(message);

                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("company", company);
                await next();
            }
        }
    }
}
