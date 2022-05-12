using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    /// <summary>
    /// https://stripe.com/docs/api/customers
    /// </summary>
    public class StripeCustomerCreateMapper
    {
        public static CustomerCreateOptions MapCustomerOptions(PaymentSystemCustomer customer)
        {
            return new CustomerCreateOptions()
            {
                Name = $"{customer.FirstName} {customer.LastName}",
                Email = customer.Email
            };
        }
    }
}
