using AutoMapper;
using Entities.Goods;
using Microsoft.IdentityModel.Tokens;
using Models.Goods;
using Repositories.Module.Account;
using Repositories.Module.Goods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.CustomExceptions;

namespace Services.Goods.Impl
{
    internal class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IUserRepository userRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AddressModel> AddAddressAsync(int userId, AddAddressModel model)
        {
            if (model.ContactNum.IsNullOrEmpty()) throw new CustomException("联系号码不能为空");
            if (model.DeliveryAddress.IsNullOrEmpty()) throw new CustomException("配送地址不能为空");
            var user = await _userRepository.GetUserById(userId);
            if (user is null) throw new CustomException("该用户不存在");
            var userAddresses = await GetAddressesByUserIdAsync(userId);
            Address address = new Address(){ UserId = userId, Name = model.Name, ContactNum = model.ContactNum, DeliveryAddress = model.DeliveryAddress };
            if (userAddresses.Count() == 0) address.IsDefault = true;
            await _addressRepository.AddEntityAsync(address);
            await _addressRepository.UnitOfWork.SaveChangeAsync();
            return _mapper.Map<AddressModel>(address);
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            var address = await _addressRepository.GetEntityAsync(a => a.Id == id);
            if(address is null) throw new CustomException("该地址不存在");
            _addressRepository.DeleteEntity(address);
            return await _addressRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<AddressModel>> GetAddressesByUserIdAsync(int userId)
        {
            var result = await _addressRepository.GetListAsNoTrackingAsync(a => a.UserId == userId);
            return  result.Select(_mapper.Map<AddressModel>).ToList();
        }

        public async Task<bool> SetAddressToDefaultAsync(int userId, int id)
        {
           var addresses = await _addressRepository.GetEntitiesAsync(a => a.UserId == userId);
            if(!addresses.Any(a => a.Id == id)) throw new CustomException("该地址不存在");
            foreach (var address in addresses)
            {
                if (address is null) continue;
                address.IsDefault = address.Id == id ? true : false;
            }
            _addressRepository.UpdateEntities(addresses);
            return await _addressRepository.UnitOfWork.SaveChangeAsync();
        }

        public async Task<bool> UpdateAddressAsync(UpdateAddressModel model)
        {
            var address = await _addressRepository.GetEntityAsync(a => a.Id == model.Id);
            if (address is null) throw new CustomException("该地址不存在");
            if (!model.Name.IsNullOrEmpty()) address.Name = model.Name;
            if (!model.ContactNum.IsNullOrEmpty()) address.ContactNum = model.ContactNum;
            if (!model.DeliveryAddress.IsNullOrEmpty()) address.DeliveryAddress = model.DeliveryAddress;
            _addressRepository.UpdateEntity(address);
            return await _addressRepository.UnitOfWork.SaveChangeAsync();
        }
    }
}
