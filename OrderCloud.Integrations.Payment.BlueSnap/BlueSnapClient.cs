using Flurl.Http;
using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Payment.BlueSnap
{
	public class BlueSnapClient
	{
		protected static IFlurlRequest BuildClient(BlueSnapConfig config) => config.BaseUrl.WithBasicAuth(config.APIUsername, config.APIPassword);

		/// <summary>
		/// https://support.bluesnap.com/docs/faqs-hosted-payment-fields#creating-a-pftoken
		/// </summary>
		public static async Task<string> GetHostedPaymentFieldToken(BlueSnapConfig config)
		{
			var response = await BuildClient(config)
				.AppendPathSegments("services", "2", "payment-fields-tokens")
				.PostWithErrorHandlingAsync<BlueSnapError>(config);
			response.Headers.TryGetFirst("Location", out string locationResponse);
			var token = locationResponse.Split('/').Last();
			return token;
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/auth-only
		/// </summary>
		public static async Task<BlueSnapCardTransactionResponse> CreateCardTransaction(BlueSnapCardTransaction transaction, BlueSnapConfig config)
		{
			var transactionResult = await BuildClient(config)
				.AppendPathSegments("services", "2", "transactions")
				.PostJsonWithErrorHandlingAsync<BlueSnapError>(transaction, config)
				.ReceiveJson<BlueSnapCardTransactionResponse>();
			return transactionResult;
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/capture
		/// https://developers.bluesnap.com/v8976-JSON/docs/auth-reversal
		/// </summary>
		public static async Task<BlueSnapCardTransactionResponse> UpdateCardTransaction(BlueSnapCardTransaction transaction, BlueSnapConfig config)
		{
			var transactionResult = await BuildClient(config)
				.AppendPathSegments("services", "2", "transactions")
				.PostJsonWithErrorHandlingAsync<BlueSnapError>(transaction, config)
				.ReceiveJson<BlueSnapCardTransactionResponse>();
			return transactionResult;
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/retrieve
		/// </summary>
		public static async Task<BlueSnapCardTransactionResponse> GetCardTransaction(string transactionID, BlueSnapConfig config)
		{
			var transactionResult = await BuildClient(config)
				.AppendPathSegments("services", "2", "transactions", transactionID)
				.GetJsonWithErrorHandlingAsync<BlueSnapCardTransactionResponse, BlueSnapError>(config);
			return transactionResult;
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/refund
		/// </summary>
		public static async Task<BlueSnapRefundResponse> RefundCardTransaction(string transactionID, BlueSnapRefund refund, BlueSnapConfig config)
		{
			var refundResult = await BuildClient(config)
				.AppendPathSegments("services", "2", "transactions", "refund", transactionID)
				.PostJsonWithErrorHandlingAsync<BlueSnapError>(refund, config)
				.ReceiveJson<BlueSnapRefundResponse>();

			return refundResult;
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/create-vaulted-shopper
		/// </summary>
		public static async Task<BlueSnapVaultedShopper> CreateVaultedShopper(BlueSnapVaultedShopper shopper, BlueSnapConfig config)
		{
			var shopperResponse = await BuildClient(config)
				.AppendPathSegments("services", "2", "vaulted-shoppers")
				.PostJsonWithErrorHandlingAsync<BlueSnapError>(shopper, config)
				.ReceiveJson<BlueSnapVaultedShopper>();

			return shopperResponse;
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/retrieve-vaulted-shopper
		/// </summary>
		public static async Task<BlueSnapVaultedShopper> GetVaultedShopper(string vaultedShopperID, BlueSnapConfig config)
		{
			var shopperResponse = await BuildClient(config)
				.AppendPathSegments("services", "2", "vaulted-shoppers", vaultedShopperID)
				.GetJsonWithErrorHandlingAsync<BlueSnapVaultedShopper, BlueSnapError>(config);

			return shopperResponse;
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/update-vaulted-shopper
		/// </summary>
		public static async Task<BlueSnapVaultedShopper> UpdateVaultedShopper(string vaultedShopperID, BlueSnapVaultedShopper shopper, BlueSnapConfig config)
		{
			var shopperResponse = await BuildClient(config)
				.AppendPathSegments("services", "2", "vaulted-shoppers", vaultedShopperID)
				.PostJsonWithErrorHandlingAsync<BlueSnapError>(shopper, config)
				.ReceiveJson<BlueSnapVaultedShopper>();

			return shopperResponse;
		}
	}
}
