using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect;
using OrderCloud.Integrations.Payment.CardConnect.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Payment.CardConnect
{
	public class CardConnectService : OCIntegrationService, ICreditCardProcessor, ICreditCardSaver
	{
		public CardConnectService(CardConnectConfig defaultConfig) : base(defaultConfig) { }

		public async Task<string> GetIFrameCredentialAsync(OCIntegrationConfig overrideConfig = null)
		{
			// CardConnect Iframe does not need any credentials. This is basically an empty implementation to satisfy the ICreditCardProcessor interface.
			var token = await Task.FromResult(string.Empty);
			return token;
		}

		public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			var cardConnectAuthorizationResponse = await CardConnectClient.AuthorizeTransaction(transaction.ToCardConnectAuthorizationRequest(config), config);
			return cardConnectAuthorizationResponse.ToIntegrationsCCAuthorizationResponse();
		}

		public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			var cardConnectCaptureResponse = await CardConnectClient.CapturePreviousAuthorization(transaction.ToCardConnectCaptureRequest(config), config);
			return cardConnectCaptureResponse.ToIntegrationsCCTransactionResponse();
		}

		public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			var cardConnectRefundCaptureResult = await CardConnectClient.RefundCardCapture(transaction.ToCardConnectFundReversalRequest(config), config);
			return cardConnectRefundCaptureResult.ToIntegrationsCCFundReversalResponse();
		}

		public async Task<CCTransactionResult> VoidAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			var cardConnectVoidAuthorizationResponse = await CardConnectClient.VoidPreviousAuthorization(transaction.ToCardConnectFundReversalRequest(config), config);
			return cardConnectVoidAuthorizationResponse.ToIntegrationsCCFundReversalResponse();
		}
		public async Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerId, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			var cardConnectSavedCards = await CardConnectClient.GetProfileAsync(customerId, config.MerchantId, config);
			return cardConnectSavedCards.ToIntegrationsGetSavedCardsResponse();
		}
		public async Task<PCISafeCardDetails> GetSavedCardAsync(string customerId, string cardId, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			var cardConnectSavedCard = await CardConnectClient.GetProfileAccountAsync(customerId, cardId, config.MerchantId, config);
			return cardConnectSavedCard.ToIntegrationsGetSavedCardResponse();
		}
		public async Task<CardCreatedResponse> CreateSavedCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			if (customer.CustomerAlreadyExists)
            {
				return (await CardConnectClient.CreateOrUpdateProfile(card.ToCardConnectUpdateProfileRequest(customer, config.MerchantId), config)).ToIntegrationsCardCreatedResponse();
            } else
            {
				return (await CardConnectClient.CreateOrUpdateProfile(card.ToCardConnectCreateProfileRequest(config.MerchantId), config)).ToIntegrationsCardCreatedResponse(); ;
            }
		}
		public async Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<CardConnectConfig>(overrideConfig ?? _defaultConfig);
			await CardConnectClient.DeleteProfile(customerID, cardID, config);
		}
	}
}
