using Entities.Account;
using IdentityModel;
using IdentityServer4.Validation;
using Repositories.Module.Account;
using System.Security.Claims;
using Utils.Crypto;
using Utils.Enums;
using static IdentityModel.OidcConstants;

namespace HanFishApis.Infrastructure.Ids4
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserRepository _userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var pwd = MD5Encrypt.EncryptTo32(context.Password);
            User user = null;
            switch (context.Request.ClientId)
            {
                case "client":
                    user = await _userRepository.GetEntityAsync(u => u.LoginName == context.UserName && u.Password == pwd
                    && u.RoleId == (int)RoleTypes.普通用户);
                    break;
                case "backend":
                    user = await _userRepository.GetEntityAsync(u => u.LoginName == context.UserName && u.Password == pwd
                    && u.RoleId != (int)RoleTypes.普通用户);
                    break;
                default:
                    throw new BadHttpRequestException("没有配置该参数");
            }
            if (user is not null)
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Name, user.Name),
                    new Claim(JwtClaimTypes.Role, user.RoleId.ToString()),
                };
                context.Result = new GrantValidationResult(user.Id.ToString(), AuthenticationMethods.Password, claims);
            }
        }
    }
}
