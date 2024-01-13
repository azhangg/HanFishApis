using AutoMapper;
using Entities.System;
using Models.System;
using Repositories.Module.Account;
using Repositories.Module.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.CustomExceptions;

namespace Services.Sysyem.Impl
{
    internal class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MenuService(IMenuRepository menuRepository, IMapper mapper, IUserRepository userRepository)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<MenuModel> AddMenuAsync(AddMenuModel model)
        {
            var isExist = await _menuRepository.GetEntityAsync(m => m.Name == model.Name) != null;
            if(isExist) throw new UserOperationException("已存在该菜单");
            Menu menu = _mapper.Map<Menu>(model);
            menu.CreateTime = DateTime.Now;
            await _menuRepository.AddEntityAsync(menu);
            await _menuRepository.UnitOfWork.SaveChangeAsync();
            return _mapper.Map<MenuModel>(menu);
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            Menu menu = await _menuRepository.GetEntityAsync(m => m.Id == id);
            if (menu is null)
                throw new UserOperationException("找不到该菜单");
            _menuRepository.DeleteEntity(menu);
            return await _menuRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<List<MenuModel>> GetAllMenuAsync()
        {
            List<Menu> menus = await _menuRepository.GetAllAsync();
            var result = menus.Select(_mapper.Map<MenuModel>).ToList();
            return GetMenus(result,null);
        }

        public async Task<List<MenuModel>> GetMenusByUserId(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user is null) throw new UserOperationException("该用户不存在");
            var menus = await _menuRepository.GetMenusByUserIds(user.Role.MenuIds);
            var result = menus.Select(_mapper.Map<MenuModel>).ToList();
            return GetMenus(result, null);
        }

        public async Task<bool> UpdateMenuAsync(UpdateMenuModel model)
        {
            Menu menu = await _menuRepository.GetEntityAsNoTrackingAsync(m => m.Id == model.Id);
            if (menu is null)
                throw new UserOperationException("找不到该菜单");
            menu = CustomMapper.Map(menu,model);
            _menuRepository.UpdateEntity(menu);
            return await _menuRepository.UnitOfWork.SaveChangeAsync();
        }

        #region 菜单树结构转换
        public List<MenuModel> GetMenus(List<MenuModel> menus, int? pId)
        {
            var current = menus.Where(m => m.PId == pId).OrderByDescending(m => m.Order).ToList();
            if (current.Count == 0) return new List<MenuModel>();
            foreach (var menu in current)
            {
                menu.Children = GetMenus(menus, menu.Id);
            }
            return current;
        }
        #endregion
    }
}
