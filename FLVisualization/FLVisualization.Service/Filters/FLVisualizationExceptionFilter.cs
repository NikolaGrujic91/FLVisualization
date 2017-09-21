using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace FLVisualization.Service.Filters
{
    public class FLVisualizationExceptionFilter : IExceptionFilter
    {
        private readonly bool isDevelopment;

        public FLVisualizationExceptionFilter(bool isDevelopment)
        {
            this.isDevelopment = isDevelopment;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            string stackTrace = (this.isDevelopment) ? context.Exception.StackTrace : string.Empty;
            string message = ex.Message;
            string error = string.Empty;
            IActionResult actionResult;
            if (ex is DbUpdateConcurrencyException)
            {
                //Returns a 400
                error = "Concurrency Issue.";
                actionResult = new BadRequestObjectResult(new { Error = error, Message = message, StackTrace = stackTrace });
            }
            else
            {
                error = "General Error.";
                actionResult = new ObjectResult(
                new { Error = error, Message = message, StackTrace = stackTrace })
                {
                    StatusCode = 500
                };
            }
            context.Result = actionResult;
        }
    }
}
