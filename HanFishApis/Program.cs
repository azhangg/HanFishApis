using HanFishApis.Infrastructure;
using HanFishApis.Infrastructure.AutoMaper;
using HanFishApis.Infrastructure.Filters;
using HanFishApis.Infrastructure.Ids4;
using HanFishApis.Infrastructure.OpenApi;
using HanFishApis.Infrastructure.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.Context;
using Serilog;
using System.Reflection;
using System.Text;
using Utils.Hubs;

var builder = WebApplication.CreateBuilder(args);

var identityServerAddress = builder.Configuration.GetSection("IdentityServer").GetValue<string>("Address");

#region Ids4
builder.Services.AddIdentityServer(options => { options.IssuerUri = identityServerAddress; })
    .AddDeveloperSigningCredential()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiResources(Config.apiResoure)
    .AddInMemoryApiScopes(Config.apiScopes)
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
#endregion

#region 鉴权授权
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = identityServerAddress;
        options.RequireHttpsMetadata = false;
        options.Audience = "HanFish";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
#endregion

#region 日志配置
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        $"Logs/log.txt",
        outputTemplate: @"{Timestamp:yyyy-MM-dd HH:mm-ss.fff }[{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true,
        fileSizeLimitBytes: 10 * 1024,
        encoding: Encoding.UTF8,
        retainedFileCountLimit: 100)
    .CreateLogger();

builder.Host.UseSerilog(logger);
#endregion 

#region SignalR
builder.Services.AddSignalR();
#endregion

builder.Services.AddControllers(option=>
{
    option.Filters.Add<AsyncActionFilter>();
    option.Filters.Add<AsyncExceptionFilter>();
});
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<HanFishDbContext>(opitons => 
opitons.UseSqlServer(builder.Configuration.GetConnectionString("HanFish")));

builder.Services.AddAutoMapper(typeof(CustomProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("any",
        policy =>
        {
            policy.SetIsOriginAllowed(allow => true);
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowCredentials();
        });
});

#region Swagger
builder.Services.AddSwaggerGen(options => {
    typeof(ApiVersion).GetEnumNames().ToList().ForEach(name =>
    {
        options.SwaggerDoc(name, new OpenApiInfo
        {
            Version = name,
            Title = $"{name} API",
            Description = $"An ASP.NET Core Web API for managing {name} items"
        });
        var idsClient = Config.Clients.FirstOrDefault(client => client.ClientId.ToLower() == name.ToLower());
        if (idsClient != null)
        {
            options.AddSecurityDefinition(name, new OpenApiSecurityScheme
            {
                Description = "JWT认证授权，直接在下框中输入账号密码",
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri("/connect/token", UriKind.Relative),
                        Scopes = idsClient.AllowedScopes.ToDictionary(v => v)
                    }
                }
            });
        }
        options.DocumentFilter<CustomDocumentFilter>();
        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
#endregion

builder.Services.AddAssembly("Services");
builder.Services.AddAssembly("Repositories");

var app = builder.Build();

#region 中间件
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options=>
    {
        typeof(ApiVersion).GetEnumNames().ToList().ForEach(name =>
        {
            options.SwaggerEndpoint($"/swagger/{name}/swagger.json",$"{name}");
        });
    });
//}

app.UseCors("any");

app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Files")),
    RequestPath = "/Files"
});

app.MapControllers();

app.MapHub<MessageHub>("/hubs/message");

app.Run();
#endregion