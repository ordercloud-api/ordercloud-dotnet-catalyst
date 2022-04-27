using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapService : OCIntegrationService, ICreditCardProcessor
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
	}
}
