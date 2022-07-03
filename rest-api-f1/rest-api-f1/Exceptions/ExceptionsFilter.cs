using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rest_api_f1.Exceptions
{
    public class ExceptionsFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is ComparisonNotExistException comparisonNotExistException)
            {
                context.Result = new ObjectResult(comparisonNotExistException.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
