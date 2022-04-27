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

		public async Task<string> GetIFrameCredentialAsync(InitiateCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var token = await BlueSnapClient.GetHostedPaymentFieldToken(config);
			return token;
		}

		public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.AUTH_ONLY, transaction);
			var result = await BlueSnapClient.PostCardTransaction(trans, config);
			return BlueSnapTransactionMapper.ToCardTransactionResult(result);
		}

		public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.CAPTURE, transaction);
			var result = await BlueSnapClient.PostCardTransaction(trans, config);
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
			var result = await BlueSnapClient.PostCardTransaction(trans, config);
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
			return cards.FirstOrDefault(BlueSnapVaultedShopperMapper.CardMatchesID);
		}

		public async Task<PCISafeCardDetails> CreateSavedCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			BlueSnapVaultedShopper shopper;
			if (customer.CustomerAlreadyExists)
			{
				shopper = BlueSnapVaultedShopperMapper.BuildAddCardRequest(customer, card);
			}
			else
			{
				shopper = BlueSnapVaultedShopperMapper.BuildCreateShopperRequest(customer, card);
			}
			var result = await BlueSnapClient.CreateVaultedShopper(shopper, config);
			var cards = BlueSnapVaultedShopperMapper.ToCardList(result);
			return cards.FirstOrDefault(BlueSnapVaultedShopperMapper.CardMatchesID);
		}

		public async Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var deleteCard = BlueSnapVaultedShopperMapper.BuildDeleteCardRequest(cardID);
			await BlueSnapClient.UpdateVaultedShopper(customerID, deleteCard, config);
		}
	}
}
