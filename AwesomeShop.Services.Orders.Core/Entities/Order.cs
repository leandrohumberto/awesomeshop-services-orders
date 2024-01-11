using AwesomeShop.Services.Orders.Core.Enums;
using AwesomeShop.Services.Orders.Core.Events;
using AwesomeShop.Services.Orders.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwesomeShop.Services.Orders.Core.Entities
{
    public class Order : AggregateRoot
    {
        public Order(Customer customer, List<OrderItem> items, DeliveryAddress deliveryAddress,
            PaymentAddress paymentAddress, PaymentInfo paymentInfo)
        {
            Id = Guid.NewGuid();
            TotalPrice = items?.Sum(i => i.Price * i.Quantity) ?? decimal.Zero;
            Customer = customer;
            Items = items;
            DeliveryAddress = deliveryAddress;
            PaymentAddress = paymentAddress;
            PaymentInfo = paymentInfo;

            Status = OrderStatus.Started;
            CreatedAt = DateTime.UtcNow;
            AddEvent(new OrderCreated(Id, TotalPrice, paymentInfo, customer.FullName, customer.Email));
        }

        public decimal TotalPrice { get; private set; }

        public Customer Customer { get; private set; }

        public List<OrderItem> Items { get; private set; } = new();

        public DeliveryAddress DeliveryAddress { get; private set; }

        public PaymentAddress PaymentAddress { get; private set; }

        public PaymentInfo PaymentInfo { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public OrderStatus Status { get; private set; }

        public void SetAsCompleted() => Status = OrderStatus.Completed;

        public void SetAsRejected() => Status = OrderStatus.Rejected;
    }
}
