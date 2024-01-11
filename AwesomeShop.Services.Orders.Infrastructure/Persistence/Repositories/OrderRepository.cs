using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.Repositories;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("orders");
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orders.InsertOneAsync(order, new InsertOneOptions(), cancellationToken);
        }

        public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var cursor = await _orders.FindAsync(o => o.Id == id, cancellationToken: cancellationToken);
            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _orders.ReplaceOneAsync(o => o.Id == order.Id, order, cancellationToken: cancellationToken);
        }
    }
}
