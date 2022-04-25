using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Integrations.Interfaces
{
	public interface ICreditCardSaver
	{
		Task<List<SavedCreditCard>> ListSavedCardsAsync(string customerID, OCIntegrationConfig configOverride = null);
		Task<SavedCreditCard> GetSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null);
		Task<SavedCreditCard> CreateSavedCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card, OCIntegrationConfig configOverride = null);
		Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null);
	}

	public class PaymentSystemCustomer
	{
		public string ID { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool CustomerAlreadyExists { get; set; }
	}

	public class SavedCreditCard : PCISafeCardDetails
	{
		public string ID { get; set; }
	}

	public class PCISafeCardDetails
	{
		public string Token { get; set; }
		public string CardHolderName { get; set; }
		[MaxLength(4, ErrorMessage = "Invalid partial number: Must be 4 digits. Ex: 4111")]
		public string NumberLast4Digits { get; set; }
		[MaxLength(2, ErrorMessage = "Invalid expiration month format: MM")]
		public string ExpirationMonth { get; set; }
		[MaxLength(4, ErrorMessage = "Invalid expiration year format: YYYY")]
		public string ExpirationYear { get; set; }
		public string CardType { get; set; }
	}
}
