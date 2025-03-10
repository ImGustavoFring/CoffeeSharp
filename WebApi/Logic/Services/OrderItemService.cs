﻿using CoffeeSharp.Domain.Entities;
using WebApi.Logic.Services.Interfaces;

namespace WebApi.Logic.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IRepository<OrderItem> _repository;

        public OrderItemService(IRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem item)
        {
            return await _repository.AddAsync(item);
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem item)
        {
            return await _repository.UpdateAsync(item);
        }

        public async Task DeleteOrderItemAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
