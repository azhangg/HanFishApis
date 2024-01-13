using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Utils;

namespace HanFishApis.Infrastructure.Filters
{
    public class AsyncActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<AsyncActionFilter> _logger;

        public AsyncActionFilter(ILogger<AsyncActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requestType = context.HttpContext.Request.Method;
            switch (requestType)
            {
                case "GET":
                    _logger.LogInformation(
                    "时间：{time}\n远程地址：{remoteIp}\n请求接口：{requestInterface}\n参数：{parameters}",
                    DateTimeHelper.DataTimeString,
                    context.HttpContext.Connection.RemoteIpAddress,
                    context.HttpContext.Request.Path,
                     context.HttpContext.Request.QueryString);
                    break;
                case "POST":
                    var argumentsList = context.ActionArguments.ToList();
                    if (argumentsList.Count > 0)
                        _logger.LogInformation(
                        "时间：{time}\n远程地址：{remoteIp}\n请求接口：{requestInterface}\n参数：{arguments}",
                        DateTimeHelper.DataTimeString,
                        context.HttpContext.Connection.RemoteIpAddress,
                         context.HttpContext.Request.Path,
                         JsonConvert.SerializeObject(argumentsList[0].Value, Formatting.Indented)
                        );
                    break;
                default:
                    break;
            }
            await next();
        }
    }
}
