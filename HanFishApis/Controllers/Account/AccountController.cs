using AutoMapper;
using HanFishApis.Controllers.Base;
using HanFishApis.Infrastructure.Ids4;
using HanFishApis.Infrastructure.OpenApi;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Models.Account;
using Newtonsoft.Json;
using Services.Account;
using System.Diagnostics;

namespace HanFishApis.Controllers.Account
{
    public class AccountController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(IConfiguration configuration, IUserService userService, IMapper mapper)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
        }

        internal string IdentityServerAddress => _configuration.GetSection("IdentityServer").GetValue<string>("Address") ?? "";

        #region 用户注册
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserModel userModel)
        {
            var result = await _userService.AddUserAsync(_mapper.Map<UserModel>(userModel));
            if (result is not null)
                return JsonSuccess(result, "添加成功");
            return JsonFail("添加失败");
        }
        #endregion

        #region 用户登录
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Client))]
        public async Task<IActionResult> ClientLoginAsync(string userName, string password)
        {
            var response = await new HttpClient().RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"{IdentityServerAddress}/connect/token",
                ClientId = "client",
                ClientSecret = Config.Secret,
                UserName = userName,
                Password = password,
                Scope = "client offline_access"
            });
            if (response.IsError)
            {
                return JsonFail("账号或密码错误");
            }
            var result = new
            {
                response.AccessToken,
                ExpiresIn = DateTimeOffset.Now.AddSeconds(response.ExpiresIn).ToUnixTimeSeconds(),
                response.TokenType,
                response.RefreshToken,
                response.Scope
            };
            return JsonSuccess(result, "登录成功");
        }

        [HttpPost]
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> BackendLoginAsync(string userName, string password)
        {
            var response = await new HttpClient().RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"{IdentityServerAddress}/connect/token",
                ClientId = "backend",
                ClientSecret = Config.Secret,
                UserName = userName,
                Password = password,
                Scope = "backend offline_access"
            });
            if (response.IsError)
            {
                return JsonFail("账号或密码错误");
            }
            var result = new
            {
                response.AccessToken,
                ExpiresIn = DateTimeOffset.Now.AddSeconds(response.ExpiresIn).ToUnixTimeSeconds(),
                response.TokenType,
                response.RefreshToken,
                response.Scope
            };
            return JsonSuccess(result, "登录成功");
        }
        #endregion

        #region 刷新令牌
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Client))]
        public async Task<IActionResult> ClientRefreshTokenAsync(string refreshToken)
        {
            var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = $"{IdentityServerAddress}/connect/token",
                ClientId = "client",
                ClientSecret = Config.Secret,
                RefreshToken = refreshToken,
                Scope = "client"
            });
            if (response.IsError)
            {
                return JsonFail("刷新令牌无效");
            }
            var result = new
            {
                response.AccessToken,
                ExpiresIn = DateTimeOffset.Now.AddSeconds(response.ExpiresIn).ToUnixTimeSeconds(),
                response.TokenType,
                response.RefreshToken,
                response.Scope
            };
            return JsonSuccess(result, "令牌获取成功");
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Backend))]
        public async Task<IActionResult> BackendRefreshTokenAsync(string refreshToken)
        {
            var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = $"{IdentityServerAddress}/connect/token",
                ClientId = "backend",
                ClientSecret = Config.Secret,
                RefreshToken = refreshToken,
                Scope = "backend"
            });
            if (response.IsError)
            {
                return JsonFail("刷新令牌无效");
            }
            var result = new
            {
                response.AccessToken,
                ExpiresIn = DateTimeOffset.Now.AddSeconds(response.ExpiresIn).ToUnixTimeSeconds(),
                response.TokenType,
                response.RefreshToken,
                response.Scope
            };
            return JsonSuccess(result, "令牌获取成功");
        }
        #endregion

        #region 微信开放接口
        internal class Result
        {
            public string openId { get; set; }
        }

        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = nameof(ApiVersion.Client))]
        public async Task<IActionResult>Jscode2sessionAsync(string jsCode, string? userName, string? avatarUrl)
        {
            var appid = _configuration.GetSection("Weapp").GetValue<string>("appid");
            var secret = _configuration.GetSection("Weapp").GetValue<string>("secret");
            var response = await new HttpClient().GetStringAsync($"https://api.weixin.qq.com/sns/jscode2session?appid={appid}" +
                $"&secret={secret}&js_code={jsCode}&grant_type=authorization_code");
            var result = JsonConvert.DeserializeObject<Result>(response);
            if (!result.openId.IsNullOrEmpty())
            {
                var isExist = await _userService.IsUserExistAsync(result.openId);
                if (!isExist)
                {
                    RegisterUserModel model = new RegisterUserModel() {
                        LoginName = result.openId,
                        Password = result.openId,
                        Name = userName,
                        AvatarUrl = avatarUrl
                    };
                    await _userService.AddUserAsync(_mapper.Map<UserModel>(model));
                }
            }
            return JsonSuccess(result);
        }
        #endregion
    }
}
