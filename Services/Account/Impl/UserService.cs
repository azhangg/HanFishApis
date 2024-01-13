using AutoMapper;
using Entities.Account;
using Models;
using Models.Account;
using Repositories.Base;
using Repositories.Module.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Crypto;
using Utils.CustomExceptions;
using Utils.Enums;

namespace Services.Account.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<UserModel> AddUserAsync(UserModel user)
        {
            bool IsExist = await _userRepository.GetEntityAsync(u => u.LoginName == user.LoginName) is not null;
            if(IsExist) throw new UserOperationException("该用户已存在");
            if (string.IsNullOrWhiteSpace(user.Name))
                user.Name = $"新用户{DateTime.Now.ToString("yyMMddHHmmss")}";
            if (user.RoleId is not null && user.RoleId is not default(int))
            {
                var role = await _roleRepository.GetEntityAsync(r => r.Id == user.RoleId);
                if (role is null)
                    throw new UserOperationException("数据库没有该角色");
            }
            else {
                user.RoleId = (int)RoleTypes.普通用户;
            }
            user.CreateTime = DateTime.Now;
            user.Password = MD5Encrypt.EncryptTo32(user.Password,true);
            var entity = _mapper.Map<User>(user);
            await _userRepository.AddEntityAsync(entity);
            await _userRepository.UnitOfWork.SaveChangeAsync();
            user.Id = entity.Id;
            return user;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            User user = await _userRepository.GetEntityAsync(u => u.Id == id);
            if (user is null)
                throw new UserOperationException("该用户不存在");
            _userRepository.DeleteEntity(user);
            return await _userRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            User user = await _userRepository.GetEntityAsync(u => u.Id == id);
            return _mapper.Map<UserModel>(user);
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            List<User> users = await _userRepository.GetAllAsync();
            return users.Select(_mapper.Map<UserModel>).ToList();
        }

        public async Task<PaginationModel<UserModel>> GetUsersToPagination(string searchText, int page, int count)
        {
            (var users,var total) = await _userRepository.GetListToPaginationAsync(u => u.Id, false,
                u=>u.Name.Contains(searchText) || u.LoginName.Contains(searchText),
                page, count);
            return new PaginationModel<UserModel>()
            {
                Page = page,
                PageCount = count,
                Total = total,
                Data = users.Select(_mapper.Map<UserModel>).ToList()
            };
        }

        public async Task<bool> IsUserExistAsync(string loginName)
        {
            User userinfo = await _userRepository.GetEntityAsync(u => u.LoginName == loginName);
            return userinfo != null;
        }

        public async Task<bool> UpdateUserAsync(UserModel user)
        {
            User userinfo = await _userRepository.GetEntityAsync(u => u.Id == user.Id);
            if (userinfo is not null)
            {
                userinfo.Name = user.Name;
                userinfo.Email = user.Email;
                userinfo.AvatarUrl = user.AvatarUrl;
                _userRepository.UpdateEntity(userinfo);
                return await _userRepository.UnitOfWork.SaveChangeAsync();
            }
            return false;
        }

        public async Task<bool> UpdateUserAsync(UpdateClientUserModel user)
        {
            User userinfo = await _userRepository.GetEntityAsync(u => u.Id == user.Id);
            if (userinfo is not null)
            {
                if(user.Name is not null)
                    userinfo.Name = user.Name;
                if (user.Email is not null)
                    userinfo.Email = user.Email;
                if (user.AvatarUrl is not null)
                    userinfo.AvatarUrl = user.AvatarUrl;
                _userRepository.UpdateEntity(userinfo);
                return await _userRepository.UnitOfWork.SaveChangeAsync();
            }
            return false;
        }
    }
}
