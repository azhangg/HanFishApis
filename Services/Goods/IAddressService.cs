using Models.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Goods
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressModel>> GetAddressesByUserIdAsync(int userId);

        Task<AddressModel> AddAddressAsync(int userId, AddAddressModel model);

        Task<bool> UpdateAddressAsync(UpdateAddressModel model);

        Task<bool> DeleteAddressAsync(int id);

        Task<bool> SetAddressToDefaultAsync(int userId, int id);
    }
}
