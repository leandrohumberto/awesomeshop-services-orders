using AwesomeShop.Services.Orders.Core.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    }
}
