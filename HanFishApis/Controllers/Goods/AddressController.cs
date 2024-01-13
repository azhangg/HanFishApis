using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Goods;
using Services.Goods;
using System.Security.Claims;

namespace HanFishApis.Controllers.Goods
{
    public class AddressController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressByUserIdAsync()
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var result = await _addressService.GetAddressesByUserIdAsync(int.Parse(userId));
            return JsonSuccess(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddressAsync(AddAddressModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var result = await _addressService.AddAddressAsync(int.Parse(userId), model);
            if (result is null) return JsonFail("添加失败");
            return JsonSuccess(result,"添加成功");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddressAsync(UpdateAddressModel model)
        {
            var result = await _addressService.UpdateAddressAsync(model);
            if (!result) return JsonFail("修改失败");
            return JsonSuccess("修改成功");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddressAsync(DeleteModel model)
        {
            var result = await _addressService.DeleteAddressAsync(model.Id);
            if (!result) return JsonFail("删除失败");
            return JsonSuccess("删除成功");
        }

        [HttpPost]
        public async Task<IActionResult> SetDefaultAddressAsync(DeleteModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var result = await _addressService.SetAddressToDefaultAsync(int.Parse(userId), model.Id);
            if (!result) return JsonFail("设置失败");
            return JsonSuccess("设置成功");
        }
    }
}
