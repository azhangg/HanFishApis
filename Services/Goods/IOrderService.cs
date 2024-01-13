using Models;
using Models.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Enums;

namespace Services.Goods
{
    public interface IOrderService
    {
        Task<PaginationModel<OrderModel>> GetOrderToPaginationAsync(int page, int count, int userId = 0, int status = 0, string search = "");

        Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(int userId);

        Task<OrderModel> getOrderByOrderIdAsync(int id);

        Task<OrderModel> AddOrderAsync(AddOrderModel model);

        Task<bool> UpdateOrderAsync(UpdateOrderModel model);

        Task<bool> DeleteOrderAsync(int id);

        Task<bool> ModifyOrderStatusAsync(int id, OrderStatus status);

        Task<bool> PayOrderAsync(int id);

        Task<bool> OrderDeliveryAsync(int id);

        Task<bool> ReceiveOrderAsync(int id);

        Task<bool> CancelOrderAsync(int id);
    }
}
