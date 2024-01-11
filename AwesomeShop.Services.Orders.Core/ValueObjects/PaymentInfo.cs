using System;

namespace AwesomeShop.Services.Orders.Core.ValueObjects
{
    public class PaymentInfo
    {
        public PaymentInfo(string cardNumber, string fullName, string expiration, string cvv)
        {
            CardNumber = cardNumber;
            FullName = fullName;
            Expiration = expiration;
            Cvv = cvv;
        }

        public string CardNumber { get; private set; }
        
        public string FullName { get; private set;}

        public string Expiration { get; private set; }

        public string Cvv { get; private set; }

        public override bool Equals(object obj) => obj is PaymentInfo paymentInfo && Equals(paymentInfo);

        private bool Equals(PaymentInfo other) => CardNumber == other.CardNumber
            && FullName == other.FullName
            && Expiration == other.Expiration
            && Cvv == other.Cvv;

        public override int GetHashCode() =>
            HashCode.Combine(base.GetHashCode(), CardNumber, FullName, Expiration, Cvv);
    }
}
