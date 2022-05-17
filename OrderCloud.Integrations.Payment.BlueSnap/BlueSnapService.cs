using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapService : OCIntegrationService, ICreditCardSaver, ICreditCardProcessor
	{
		public BlueSnapService(BlueSnapConfig defaultConfig) : base(defaultConfig) { }

		public async Task<string> GetIFrameCredentialAsync(OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var token = await BlueSnapClient.GetHostedPaymentFieldToken(config);
			return token;
		}

		public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.AUTH_ONLY, transaction);
			var result = await BlueSnapClient.CreateCardTransaction(trans, config);
			return BlueSnapTransactionMapper.ToCardTransactionResult(result);
		}

		public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.CAPTURE, transaction);
			var result = await BlueSnapClient.CreateCardTransaction(trans, config);
			return BlueSnapTransactionMapper.ToCardTransactionResult(result);
		}

		public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var refund = BlueSnapRefundMapper.ToBlueSnapRefund(transaction);
			var result = await BlueSnapClient.RefundCardTransaction(transaction.TransactionID, refund, config);
			return BlueSnapRefundMapper.ToCardTransactionResult(result);
		}

		public async Task<CCTransactionResult> VoidAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.AUTH_REVERSAL, transaction);
			var result = await BlueSnapClient.CreateCardTransaction(trans, config);
			return BlueSnapTransactionMapper.ToCardTransactionResult(result);
		}

		public async Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerID, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var shopper = await BlueSnapClient.GetVaultedShopper(customerID, config);
			var cards = BlueSnapVaultedShopperMapper.ToCardList(shopper);
			return cards;
		}

		public async Task<PCISafeCardDetails> GetSavedCardAsync(string customerID, string cardID, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var shopper = await BlueSnapClient.GetVaultedShopper(customerID, config);
			var cards = BlueSnapVaultedShopperMapper.ToCardList(shopper);
			return cards.FirstOrDefaultWithID(cardID);
		}

		public async Task<CardCreatedResponse> CreateSavedCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var shopper = BlueSnapVaultedShopperMapper.BuildAddCardRequest(customer, card);
			if (customer.CustomerAlreadyExists)
			{
				shopper = await BlueSnapClient.UpdateVaultedShopper(customer.ID, shopper, config);
			}
			else
			{
				shopper = await BlueSnapClient.CreateVaultedShopper(shopper, config);
			}
			card.SavedCardID = BlueSnapVaultedShopperMapper.BuildSavedCardID(card);
			return new CardCreatedResponse()
			{
				CustomerID = shopper.vaultedShopperId,
				Card = card
			};
		}

		public async Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var deleteCard = BlueSnapVaultedShopperMapper.BuildDeleteCardRequest(cardID);
			await BlueSnapClient.UpdateVaultedShopper(customerID, deleteCard, config);
		}
	}
}
