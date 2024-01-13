using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Utils;

namespace HanFishApis.Infrastructure.Filters
{
    public class AsyncExceptionFilter : IAsyncExceptionFilter
    {

        private readonly ILogger<AsyncActionFilter> _logger;

        public AsyncExceptionFilter(ILogger<AsyncActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            var streamByte = StreamHelper.SerializeObject(new
            {
                isSuccess = false,
                code = context.HttpContext.Response.StatusCode,
                message = context.Exception.Message ?? ""
            });
            context.HttpContext.Response.Headers.ContentType = "application/json";
            await context.HttpContext.Response.Body.WriteAsync(streamByte);
            var exception = context.Exception;
            _logger.LogError(exception, exception.Message);
            context.ExceptionHandled = true;
        }
    }
}
