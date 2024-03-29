﻿using AwesomeShop.Services.Orders.Application.Dtos.ViewModels;
using AwesomeShop.Services.Orders.Core.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AwesomeShop.Services.Orders.Application.Queries.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderById, OrderViewModel>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderViewModel> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            return OrderViewModel.FromEntity(order);
        }
    }
}
