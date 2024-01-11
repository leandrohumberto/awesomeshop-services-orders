using System;

namespace AwesomeShop.Services.Orders.Core.ValueObjects
{
    public class DeliveryAddress
    {
        public DeliveryAddress(string street, string number, string city, string state, string zipCode)
        {
            Street = street;
            Number = number;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string Street { get; private set; }

        public string Number { get; private set; }

        public string City { get; private set; }

        public string State { get; private set; }
        
        public string ZipCode { get; private set; }

        public override bool Equals(object obj) => obj is DeliveryAddress address && Equals(address);

        private bool Equals(DeliveryAddress other) => Street == other.Street && Number == other.Number &&
            City == other.City && State == other.State && ZipCode == other.ZipCode;

        public override int GetHashCode() => HashCode.Combine(Street, Number, City, State, ZipCode);
    }
}
