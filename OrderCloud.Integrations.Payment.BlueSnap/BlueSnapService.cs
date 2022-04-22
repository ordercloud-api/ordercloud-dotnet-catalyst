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

		public async Task<CardTransactionResult> AuthorizeOnlyAsync(CreateCardTransaction transaction, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionRequestMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.AUTH_ONLY, transaction);
			var result = await BlueSnapClient.PostCardTransaction(trans, config);
			return BlueSnapTransactionResponseMapper.ToCardTransactionResult(result);
		}

		public async Task<CardTransactionResult> AuthorizeAndCaptureAsync(CreateCardTransaction transaction, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionRequestMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.AUTH_CAPTURE, transaction);
			var result = await BlueSnapClient.PostCardTransaction(trans, config);
			return BlueSnapTransactionResponseMapper.ToCardTransactionResult(result);
		}

		public async Task<CardTransactionResult> CapturePriorAuthorizationAsync(ModifyCardTransaction transaction, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionRequestMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.CAPTURE, transaction);
			var result = await BlueSnapClient.PostCardTransaction(trans, config);
			return BlueSnapTransactionResponseMapper.ToCardTransactionResult(result);
		}

		public async Task<CardTransactionStatus> GetTransactionAsync(string transactionID, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var transaction = await BlueSnapClient.GetCardTransaction(transactionID, config);
			return BlueSnapTransactionResponseMapper.ToCardTransactionStatus(transaction);

		}

		public async Task<CardTransactionResult> RefundCaptureAsync(ModifyCardTransaction transaction, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var refund = BlueSnapRefundMapper.ToBlueSnapRefund(transaction);
			var result = await BlueSnapClient.RefundCardTransaction(transaction.TransactionID, refund, config);
			return BlueSnapRefundMapper.ToCardTransactionResult(result);
		}

		public async Task<CardTransactionResult> VoidAuthorizationAsync(ModifyCardTransaction transaction, OCIntegrationConfig overrideConfig)
		{
			var config = ValidateConfig<BlueSnapConfig>(overrideConfig ?? _defaultConfig);
			var trans = BlueSnapTransactionRequestMapper.ToBlueSnapCardTransaction(BlueSnapTransactionType.AUTH_REVERSAL, transaction);
			var result = await BlueSnapClient.PostCardTransaction(trans, config);
			return BlueSnapTransactionResponseMapper.ToCardTransactionResult(result);
		}
	}
}
