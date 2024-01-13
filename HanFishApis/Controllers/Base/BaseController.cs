using HanFishApis.Infrastructure.OpenApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HanFishApis.Controllers.Base
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected virtual IActionResult JsonSuccess(string message)
        {
            return JsonSuccess(null, message);
        }

        protected virtual IActionResult JsonSuccess(object data)
        {
            return JsonSuccess(data, "");
        }

        protected virtual IActionResult JsonSuccess(object? data, string message)
        {
            return Ok(new MyApiResult
            {
                IsSuccess = true,
                Message = message,
                Data = data
            });
        }

        protected virtual IActionResult JsonFail(string message)
        {
            return JsonFail(null, message);
        }

        protected virtual IActionResult JsonFail(object data)
        {
            return JsonFail(data, "");
        }

        protected virtual IActionResult JsonFail(object? data, string message)
        {
            return Ok(new MyApiResult
            {
                IsSuccess = false,
                Data = data,
                Message = message
            });
        }
    }
}
