using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect;
using OrderCloud.Integrations.Payment.CardConnect.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Payment.CardConnect
{
	public class CardConnectService : OCIntegrationService, ICreditCardProcessor
	{
		public CardConnectService(CardConnectConfig defaultConfig) : base(defaultConfig) { }

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
	}
}
