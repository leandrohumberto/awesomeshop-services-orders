using AwesomeShop.Services.Orders.Core.Entities;
using AwesomeShop.Services.Orders.Core.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwesomeShop.Services.Orders.Application.Commands
{
    public class AddOrder : IRequest<Guid>
    {
        public CustomerInputModel Customer { get; set; }

        public List<OrderItemInputModel> OrderItems { get; set; }

        public DeliveryAddressInputModel DeliveryAddress { get; set; }

        public PaymentAddressInputModel PaymentAddress { get; set; }

        public PaymentInfoInputModel PaymentInfo { get; set; }

        public Order ToEntity()
            => new (
                    customer: new Customer(Customer.Id, Customer.FullName, Customer.Email),
                    items: OrderItems.Select(i => new OrderItem(i.ProdutId, i.Quantity, i.Price)).ToList(),
                    deliveryAddress: new DeliveryAddress(DeliveryAddress.Street, DeliveryAddress.Number,
                        DeliveryAddress.City, DeliveryAddress.State, DeliveryAddress.ZipCode),
                    paymentAddress: new PaymentAddress(PaymentAddress.Street, PaymentAddress.Number,
                        PaymentAddress.City, PaymentAddress.State, PaymentAddress.ZipCode),
                    paymentInfo: new PaymentInfo(PaymentInfo.CardNumber, PaymentInfo.FullName,
                        PaymentInfo.Expiration, PaymentInfo.Cvv)
                );
    }

    public class CustomerInputModel
    {
        public Guid Id { get; set; }
        
        public string FullName { get; set; }

        public string Email { get; set; }
    }

    public class OrderItemInputModel
    {
        public Guid ProdutId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }

    public class DeliveryAddressInputModel
    {
        public string Street { get; set; }

        public string Number { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
    }

    public class PaymentAddressInputModel
    {
        public string Street { get; set; }

        public string Number { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
    }

    public class PaymentInfoInputModel
    {
        public string CardNumber { get; set; }

        public string FullName { get; set; }

        public string Expiration { get; set; }

        public string Cvv { get; set; }
    }
}
