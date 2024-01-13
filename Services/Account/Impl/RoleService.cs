using AutoMapper;
using Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Models;
using Models.Account;
using Repositories.Base;
using Repositories.Module.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Account.Impl
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleModel> AddRoleAsync(AddRoleModel roleModel)
        {
            Role role = new Role { Name = roleModel.Name, MenuIds = roleModel.MenuIds };
            await _roleRepository.AddEntityAsync(role);
            await _roleRepository.UnitOfWork.SaveChangeAsync();
            return new RoleModel
            {
                Id = role.Id,
                Name = roleModel.Name,
                MenuIds = roleModel.MenuIds,
            };
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleRepository.GetEntityAsync(r => r.Id == id);
            if (role is null)
                throw new UserOperationException("该角色不存在");
            _roleRepository.DeleteEntity(role);
            return await _roleRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<List<RoleModel>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleModel { 
                Id = r.Id,
                Name = r.Name,
                MenuIds = r.MenuIds
            }).ToList();
        }

        public async Task<PaginationModel<RoleModel>> GetRolesToPaginationAsync(int pageIndex, int pageCount)
        {
            (var data, var total) = await _roleRepository.GetListToPaginationAsync(o => o.Id, false, pageIndex, pageCount);
            return new PaginationModel<RoleModel>
            {
                Page = pageIndex,
                Total = total,
                PageCount = pageCount,
                Data = data.Select(r => new RoleModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    MenuIds = r.MenuIds
                }).ToList()
            };
        }

        public async Task<bool> UpdateRoleAsync(UpdateRoleModel role)
        {
            var entity = await _roleRepository.GetEntityAsync(r => r.Id == role.Id);
            if (entity is null)
                throw new UserOperationException("该角色不存在");
            entity.Name = role.Name;
            entity.MenuIds = role.MenuIds;
            _roleRepository.UpdateEntity(entity);
            return await _roleRepository.UnitOfWork.SaveChangeAsync();
        }
    }
}
