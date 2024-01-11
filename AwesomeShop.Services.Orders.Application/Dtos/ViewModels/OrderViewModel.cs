using AwesomeShop.Services.Orders.Core.Entities;
using System;

namespace AwesomeShop.Services.Orders.Application.Dtos.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel(Guid id, decimal totalPrice, DateTime createdAt, string status)
        {
            Id = id;
            TotalPrice = totalPrice;
            CreatedAt = createdAt;
            Status = status;
        }

        public Guid Id { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; private set; }

        public static OrderViewModel FromEntity(Order order)
            => order != null ? new (order.Id, order.TotalPrice, order.CreatedAt, order.Status.ToString()) : null;
    }
}
