using Flurl.Http;
using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.CardConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OrderCloud.Integrations.Payment.CardConnect.Extensions;

namespace OrderCloud.Integrations.Payment.CardConnect
{
	public class CardConnectClient
	{
		/// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#authorization
		/// </summary>
		public static async Task<CardConnectAuthorizationResponse> AuthorizeTransaction(CardConnectAuthorizationRequest transaction, CardConnectConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var transactionResult = await request
					.AppendPathSegments("auth")
					.PostJsonAsync(transaction)
					.ReceiveJson<CardConnectAuthorizationResponse>();
                if (!transactionResult.WasSuccessful())
                {
                    throw new IntegrationErrorResponseException(config, request.Url, (int)HttpStatusCode.OK,
                        transactionResult);
                }
				return transactionResult;
			});
		}

		/// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#capture
		/// </summary>
		public static async Task<CardConnectCaptureResponse> CapturePreviousAuthorization(CardConnectCaptureRequest transaction, CardConnectConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var transactionResult = await request
					.AppendPathSegments("capture")
					.PostJsonAsync(transaction)
					.ReceiveJson<CardConnectCaptureResponse>();
                if (!transactionResult.WasSuccessful())
                {
                    throw new IntegrationErrorResponseException(config, request.Url, (int)HttpStatusCode.OK,
                        transactionResult);
                }
				return transactionResult;
			});
		}

		/// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#refund
		/// </summary>
		public static async Task<CardConnectFundReversalResponse> RefundCardCapture(CardConnectFundReversalRequest transaction, CardConnectConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
			{
				var transactionResult = await request
					.AppendPathSegments("refund")
					.PostJsonAsync(transaction)
					.ReceiveJson<CardConnectFundReversalResponse>();
                if (!transactionResult.WasSuccessful())
                {
                    throw new IntegrationErrorResponseException(config, request.Url, (int)HttpStatusCode.OK,
                        transactionResult);
                }
				return transactionResult;
			});
		}
		
		/// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#void
		/// </summary>
		public static async Task<CardConnectFundReversalResponse> VoidPreviousAuthorization(CardConnectFundReversalRequest transaction, CardConnectConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
            {
                var transactionResult = await request
                    .AppendPathSegments("void")
                    .PostJsonAsync(transaction)
                    .ReceiveJson<CardConnectFundReversalResponse>();

				if (!transactionResult.WasSuccessful())
                {
                    throw new IntegrationErrorResponseException(config, request.Url, (int)HttpStatusCode.OK,
                        transactionResult);
                };
				return transactionResult;
			});
		}

		/// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#get-profile-request
		/// </summary>
		public static async Task<List<CardConnectProfile>> GetSavedCardsAsync(string profileid, string merchid, CardConnectConfig config)
		{
            try
            {
                return await $"{config.BaseUrl}/profile/{profileid}//{merchid}"
                    .WithBasicAuth(config.APIUsername, config.APIPassword).GetAsync()
                    .ReceiveJson<List<CardConnectProfile>>();
            }
            catch (FlurlHttpTimeoutException ex)  // simulate with this https://stackoverflow.com/questions/100841/artificially-create-a-connection-timeout-error
            {
                // candidate for retry here?
                throw new IntegrationNoResponseException(config, $"{config.BaseUrl}/profile/{profileid}//{merchid}");
            }
            catch (FlurlHttpException ex)
            {
                var status = ex?.Call?.Response?.StatusCode;
                if (status == null) // simulate by putting laptop on airplane mode
                {
                    throw new IntegrationNoResponseException(config, $"{config.BaseUrl}/profile/{profileid}//{merchid}");
                }
                if (status == 401)
                {
                    throw new IntegrationAuthFailedException(config, $"{config.BaseUrl}/profile/{profileid}//{merchid}", (int)status);
                }
                var body = await ex.Call.Response.GetJsonAsync();
                throw new IntegrationErrorResponseException(config, $"{config.BaseUrl}/profile/{profileid}//{merchid}", (int)status, body);
            }
		}
        /// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#get-profile-request
		/// </summary>
		public static async Task<CardConnectProfile> GetSavedCardAsync(string profileId, string cardId, string merchId, CardConnectConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
            {
                var transactionResult = await request
                    .AppendPathSegments("profile", profileId, cardId, merchId)
                    .GetAsync()
                    .ReceiveJson<CardConnectGetProfileResponse>();

				if (!transactionResult.WasSuccessful())
                {
                    throw new IntegrationErrorResponseException(config, request.Url, (int)HttpStatusCode.OK,
                        transactionResult);
                };
				return transactionResult.profiles.FirstOrDefault();
			});
		}
		/// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#create-update-profile-request
		/// </summary>
		public static async Task<CardConnectCreateUpdateProfileResponse> CreateOrUpdateProfile(CardConnectCreateUpdateProfileRequest payload, CardConnectConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
            {
                var transactionResult = await request
                    .AppendPathSegments("profile")
                    .PostJsonAsync(payload)
                    .ReceiveJson<CardConnectCreateUpdateProfileResponse>();

				if (!transactionResult.WasSuccessful())
                {
                    throw new IntegrationErrorResponseException(config, request.Url, (int)HttpStatusCode.OK,
                        transactionResult);
                };
				return transactionResult;
			});
		}
		/// <summary>
		/// https://developer.cardpointe.com/cardconnect-api#delete-profile-request
		/// </summary>
		public static async Task<CardConnectDeleteProfileResponse> DeleteProfile(string customerId, string cardId, CardConnectConfig config)
		{
			return await TryCatchRequestAsync(config, async (request) =>
            {
                var transactionResult = await request
                    .AppendPathSegments("profile")
                    .DeleteAsync()
                    .ReceiveJson<CardConnectDeleteProfileResponse>();

				if (!transactionResult.WasSuccessful())
                {
                    throw new IntegrationErrorResponseException(config, request.Url, (int)HttpStatusCode.OK,
                        transactionResult);
                };
				return transactionResult;
			});
		}

		protected static async Task<T> TryCatchRequestAsync<T>(CardConnectConfig config, Func<IFlurlRequest, Task<T>> run)
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
				var body = await ex.Call.Response.GetJsonAsync();
				throw new IntegrationErrorResponseException(config, request.Url, (int)status, body);
			}
		}
	}
}