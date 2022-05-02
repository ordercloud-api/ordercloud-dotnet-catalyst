using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/credit-card
	/// </summary>
	public class BlueSnapCreditCard
	{
		public string cardNumber { get; set; }
		public string encryptedCardNumber { get; set; }
		public string cardLastFourDigits { get; set; }
		public string cardType { get; set; }
		public int? expirationMonth { get; set; } = null;
		public int? expirationYear { get; set; } = null;
		public string securityCode { get; set; }
		public string encryptedSecurityCode { get; set; }
		public string securityCodePfToken { get; set; }
	}

	/// <summary>
	/// https://developers.bluesnap.com/v8976-JSON/docs/credit-card#section-response
	/// </summary>
	public class BlueSnapCreditCardResponse
	{
		public string cardLastFourDigits { get; set; }
		public string cardType { get; set; }
		public string cardSubType { get; set; }
		public string cardCategory { get; set; }
		public string binCategory { get; set; }
		public string binNumber { get; set; }
		public string cardRegulated { get; set; }
		public string issuingBank { get; set; }
		public string issuingCountryCode { get; set; }
		public int? expirationMonth { get; set; } = null;
		public int? expirationYear { get; set; } = null;
	}
}
