using HanFishApis.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Goods;
using Services.Goods;
using System.Security.Claims;
using Utils.CustomExceptions;
using Utils.Enums;

namespace HanFishApis.Controllers.Goods
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersByUserIdAsync() 
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            var result = await _orderService.GetOrdersByUserIdAsync(int.Parse(userId));
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersToPaginationAsync(int page, int count, int userId = 0, int status = 0, string search = "")
        {
            var result = await _orderService.GetOrderToPaginationAsync(page, count, userId, status, search);
            return JsonSuccess(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var result = await _orderService.getOrderByOrderIdAsync(id);
            return JsonSuccess(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderAsync(AddOrderModel model)
        {
            var userId = User.FindFirstValue("id");
            if (userId is null) return Unauthorized();
            model.UserId = int.Parse(userId);
            var result = await _orderService.AddOrderAsync(model);
            if (result is null) return JsonFail("添加失败");
            return JsonSuccess(result,"添加成功");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderAsync(UpdateOrderModel model)
        {
            var result = await _orderService.UpdateOrderAsync(model);
            if (!result) return JsonFail("修改失败");
            return JsonSuccess("修改成功");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrderAsync(DeleteModel model)
        {
            var result = await _orderService.DeleteOrderAsync(model.Id);
            if (!result) return JsonFail("删除失败");
            return JsonSuccess("删除成功");
        }

        [HttpPost]
        public async Task<IActionResult> PayOrderAsync(DeleteModel model)
        {
            var result = await _orderService.PayOrderAsync(model.Id);
            if (!result) return JsonFail("支付失败");
            return JsonSuccess("支付成功");

        }

        [HttpPost]
        public async Task<IActionResult> OrderDeliveryAsync(DeleteModel model)
        {
            var result = await _orderService.OrderDeliveryAsync(model.Id);
            if (!result) return JsonFail("发货失败");
            return JsonSuccess("发货成功");

        }

        [HttpPost]
        public async Task<IActionResult> ReceiveOrderAsync(DeleteModel model)
        {
            var result = await _orderService.ReceiveOrderAsync(model.Id);
            if (!result) return JsonFail("收货失败");
            return JsonSuccess("收货成功");

        }

        [HttpPost]
        public async Task<IActionResult> CancelOrderAsync(DeleteModel model)
        {
            var result = await _orderService.CancelOrderAsync(model.Id);
            if (!result) return JsonFail("取消失败");
            return JsonSuccess("取消成功");

        }
    }
}
