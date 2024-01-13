using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Enums
{
    public enum OrderStatus
    {
        待付款 = 1,
        待发货 = 2,
        待收货 = 3,
        待评价 = 4,
        已完成 = 5,
        已取消 = 6
    }
}
