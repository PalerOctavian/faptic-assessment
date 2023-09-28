using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FapticService.Filters;

// rudimental exception filter so at least the error is JSON formatted and a message is displayed.
public class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger logger;
    
    public GlobalExceptionFilter(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger(nameof(GlobalExceptionFilter));
    }
    
    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        logger.LogError($"{exception.Message}. Stack trace: {exception}.");
        
        var exceptionMessage = new ExceptionMessage(exception);
        context.HttpContext.Response.StatusCode = exceptionMessage.ResponseCode;
        context.Result = new JsonResult(exceptionMessage);
    }
    
    public class ExceptionMessage
    {
        public ExceptionMessage(Exception exception)
        {
            ResponseCode = (int)HttpStatusCode.InternalServerError;
            Message = $"{exception.Message} {exception.InnerException?.Message}";
        }

        public int ResponseCode { get; set; }
        public string Message { get; set; }
    }
}