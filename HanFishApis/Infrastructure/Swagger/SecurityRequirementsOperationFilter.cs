using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HanFishApis.Infrastructure.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.GetCustomAttributes(true);
            if (attributes.OfType<AllowAnonymousAttribute>().Any())
                return;

            if (!attributes.OfType<AuthorizeAttribute>().Any() &&
                !context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any())
                return;

            var securitySchemeId = string.Empty;
            switch (context.DocumentName)
            {
                case "Client": securitySchemeId = "Client"; break;
                case "Backend": securitySchemeId = "Backend"; break;
                default: return;
            }
            var oAuthScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = securitySchemeId }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [ oAuthScheme ] = Enumerable.Empty<string>().ToList()
                }
            };
        }
    }
}
