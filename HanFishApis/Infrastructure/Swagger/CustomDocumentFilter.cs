using HanFishApis.Infrastructure.Attributes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HanFishApis.Infrastructure.Swagger
{
    public class CustomDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var ignoreApis = context.ApiDescriptions.Where(wh => wh.CustomAttributes().Any(any => any is SwaggerIgnoreAttribute));
            if (ignoreApis != null)
            {
                foreach (var ignoreApi in ignoreApis)
                {
                    swaggerDoc.Paths.Remove("/" + ignoreApi.RelativePath);
                }
            }
        }
    }
}
