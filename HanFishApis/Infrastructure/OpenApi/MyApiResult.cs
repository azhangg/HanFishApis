using Microsoft.AspNetCore.Http.HttpResults;

namespace HanFishApis.Infrastructure.OpenApi
{
    public class MyApiResult
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public object? Data { get; set; }
    }
}
