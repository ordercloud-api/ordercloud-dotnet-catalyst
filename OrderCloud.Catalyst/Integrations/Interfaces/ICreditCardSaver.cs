using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// An interface to define the behavior of a system that can store saved credit cards. Full CC details are never passed to it, as that would put it in PCI compliance scope. Instead, it accepts iframe generated tokens.
	/// </summary>
	public interface ICreditCardSaver
	{
		/// <summary>
		/// List Saved Credit Cards
		/// </summary>
		Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerID, OCIntegrationConfig configOverride = null);
		/// <summary>
		/// Get a single saved credit card
		/// </summary>
		Task<PCISafeCardDetails> GetSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null);
		/// <summary>
		/// Save a credit card for future use
		/// </summary>
		Task<CardCreatedResponse> CreateSavedCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card, OCIntegrationConfig configOverride = null);
		/// <summary>
		/// Remove a saved credit card
		/// </summary>
		Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig configOverride = null);
	}

	/// <summary>
	/// In a payment processing system, saved cards are usually stored under a customer record.
	/// </summary>
	public class PaymentSystemCustomer
	{
		/// <summary>
		/// The ID of an existing customer, or the ID to set for a newly created customer.
		/// </summary>
		public string ID { get; set; }
		/// <summary>
		/// Email of a new customer. Ignored if CustomerAlreadyExists is true. 
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// First name of a new customer. Ignored if CustomerAlreadyExists is true. 
		/// </summary>
		public string FirstName { get; set; }
		/// <summary>
		/// Last name of a new customer. Ignored if CustomerAlreadyExists is true. 
		/// </summary>
		public string LastName { get; set; }
		/// <summary>
		/// Is a customer record with matching ID expected to exist? Use to determine whether to attempt to create a new customer.
		/// </summary>
		public bool CustomerAlreadyExists { get; set; }
	}

	public class CardCreatedResponse
	{
		public PCISafeCardDetails Card { get; set; }
		public string CustomerID { get; set; }
	}

	/// <summary>
	/// Partial credit card details that do not put the solution under PCI compliance. Only the last 4 digits of the number and no CVV. Includes a token from the processor system representing the full card details.
	/// </summary>
	public class PCISafeCardDetails
	{
		/// <summary>
		/// Left null if card is not saved in processor. During authorization if it is non-empty, it will be prefered over Token. 
		/// </summary>
		public string SavedCardID { get; set; }
		/// <summary>
		/// A token from the processor system representing the full card details. During authorization, it will be used only if SavedCardID is empty.
		/// </summary>
		public string Token { get; set; }
		/// <summary>
		/// Card Holder First and Last Name
		/// </summary>
		public string CardHolderName { get; set; }
		/// <summary>
		/// Last 4 digits of the card number
		/// </summary>
		[MaxLength(4, ErrorMessage = "Invalid partial number: Must be 4 digits. Ex: 4111")]
		public string NumberLast4Digits { get; set; }
		/// <summary>
		/// Expiration month in the form MM
		/// </summary>
		[MaxLength(2, ErrorMessage = "Invalid expiration month format: MM")]
		public string ExpirationMonth { get; set; }
		/// <summary>
		/// Expiration year in the form YYYY
		/// </summary>
		[MaxLength(4, ErrorMessage = "Invalid expiration year format: YYYY")]
		public string ExpirationYear { get; set; }
		/// <summary>
		/// Card Issuer Type, e.g. "Visa", "MasterCard", ect.
		/// </summary>
		public string CardType { get; set; }
	}
}
