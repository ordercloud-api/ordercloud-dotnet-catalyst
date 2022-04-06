using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Integrations.Interfaces
{
	public class PaymentSystemCustomer
	{
		public string ID { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
	}

	public interface ICreditCardRememberer
	{
		public List<object> GetRememberedCards(PaymentSystemCustomer customer, OCIntegrationConfig configOverride = null)
		{

		}

		public object RememberCreditCard(PaymentSystemCustomer customer, )
	}
}
