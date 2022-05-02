using System;
using System.Collections.Generic;
using System.Text;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    public class StripeConfirmPaymentIntentMapper
    {
        /// <summary>
        /// https://stripe.com/docs/api/payment_intents/confirm
        /// </summary>
        public static PaymentIntentConfirmOptions MapPaymentIntentConfirmOptions(AuthorizeCCTransaction transaction) =>
            new PaymentIntentConfirmOptions()
            {
                // PaymentMethod = transaction.TransactionID, // is this correct? NO this should be Payment Intent ID
                Shipping = new ChargeShippingOptions()
                {
                    Address = new AddressOptions()
                    {
                        City = transaction.AddressVerification.City,
                        Country = transaction.AddressVerification.Country,
                        Line1 = transaction.AddressVerification.Street1,
                        Line2 = transaction.AddressVerification.Street2,
                        PostalCode = transaction.AddressVerification.Zip,
                        State = transaction.AddressVerification.State
                    },
                    Name = $"{transaction.AddressVerification.FirstName} {transaction.AddressVerification.LastName}",
                    Phone = transaction.AddressVerification.Phone
                }
            };
    }
}
