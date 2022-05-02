using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.CardConnect.Mappers
{
	public static class CardConnectAuthorizationRequestMapper
	{
		public static CardConnectAuthorizationRequest ToCardConnectAuthorizationRequest(this AuthorizeCCTransaction transaction, CardConnectConfig config)
		{
			return new CardConnectAuthorizationRequest()
			{
				orderid = transaction.OrderID,
				account = transaction.CardDetails.Token,
				amount = transaction.Amount.ToString(),
				address = transaction.AddressVerification.Street1,
				city = transaction.AddressVerification.City,
				region = transaction.AddressVerification.State,
				postal = transaction.AddressVerification.Zip,
				country = transaction.AddressVerification.Country,
				currency = transaction.Currency,
				merchid = config.MerchantId,
				expiry = $"{transaction.CardDetails.ExpirationYear}{transaction.CardDetails.ExpirationMonth}"
			};
		}
	}
}
