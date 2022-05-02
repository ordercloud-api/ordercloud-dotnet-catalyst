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
		/// <summary>
		/// https://support.bluesnap.com/docs/faqs-hosted-payment-fields#creating-a-pftoken
		/// </summary>
		public static async Task<string> GetHostedPaymentFieldToken(BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var response = await request
					.AppendPathSegments("services", "2", "payment-fields-tokens")
					.PostAsync();
				response.Headers.TryGetFirst("Location", out string locationResponse);
				var token = locationResponse.Split('/').Last();
				return token;
			});
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/auth-only
		/// </summary>
		public static async Task<BlueSnapCardTransactionResponse> CreateCardTransaction(BlueSnapCardTransaction transaction, BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var transactionResult = await request
					.AppendPathSegments("services", "2", "transactions")
					.PostJsonAsync(transaction)
					.ReceiveJson<BlueSnapCardTransactionResponse>();
				return transactionResult;
			});
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/capture
		/// https://developers.bluesnap.com/v8976-JSON/docs/auth-reversal
		/// </summary>
		public static async Task<BlueSnapCardTransactionResponse> UpdateCardTransaction(BlueSnapCardTransaction transaction, BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var transactionResult = await request
					.AppendPathSegments("services", "2", "transactions")
					.PutJsonAsync(transaction)
					.ReceiveJson<BlueSnapCardTransactionResponse>();
				return transactionResult;
			});
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/retrieve
		/// </summary>
		public static async Task<BlueSnapCardTransactionResponse> GetCardTransaction(string transactionID, BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var transactionResult = await request
					.AppendPathSegments("services", "2", "transactions", transactionID)
					.GetJsonAsync<BlueSnapCardTransactionResponse>();
				return transactionResult;
			});
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/refund
		/// </summary>
		public static async Task<BlueSnapRefundResponse> RefundCardTransaction(string transactionID, BlueSnapRefund refund, BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var refundResult = await request
					.AppendPathSegments("services", "2", "transactions", "refund", transactionID)
					.PostJsonAsync(refund)
					.ReceiveJson<BlueSnapRefundResponse>();

				return refundResult;
			});
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/create-vaulted-shopper
		/// </summary>
		public static async Task<BlueSnapVaultedShopper> CreateVaultedShopper(BlueSnapVaultedShopper shopper, BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var shopperResponse = await request
					.AppendPathSegments("services", "2", "vaulted-shoppers")
					.PostJsonAsync(shopper)
					.ReceiveJson<BlueSnapVaultedShopper>();

				return shopperResponse;
			});
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/retrieve-vaulted-shopper
		/// </summary>
		public static async Task<BlueSnapVaultedShopper> GetVaultedShopper(string vaultedShopperID, BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var shopperResponse = await request
					.AppendPathSegments("services", "2", "vaulted-shoppers", vaultedShopperID)
					.GetJsonAsync<BlueSnapVaultedShopper>();

				return shopperResponse;
			});
		}

		/// <summary>
		/// https://developers.bluesnap.com/v8976-JSON/docs/update-vaulted-shopper
		/// </summary>
		public static async Task<BlueSnapVaultedShopper> UpdateVaultedShopper(string vaultedShopperID, BlueSnapVaultedShopper shopper, BlueSnapConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var shopperResponse = await request
					.AppendPathSegments("services", "2", "vaulted-shoppers", vaultedShopperID)
					.PutJsonAsync(shopper)
					.ReceiveJson<BlueSnapVaultedShopper>();

				return shopperResponse;
			});
		}

		protected static async Task<T> TryCatchRequestAsync<T>(BlueSnapConfig config, Func<IFlurlRequest, Task<T>> run)
		{
			var request = config.BaseUrl.WithBasicAuth(config.APIUsername, config.APIPassword);
			try
			{
				return await run(request);
			}
			catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
			{
				// candidate for retry here?
				throw new IntegrationNoResponseException(config, request.Url);
			}
			catch (FlurlHttpException ex)
			{
				var status = ex?.Call?.Response?.StatusCode;
				if (status == null) // simulate by putting laptop on airplane mode
				{
					throw new IntegrationNoResponseException(config, request.Url);
				}
				if (status == 401)
				{
					throw new IntegrationAuthFailedException(config, request.Url, (int)status);
				}
				var body = await ex.Call.Response.GetJsonAsync<BlueSnapError>();
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}
