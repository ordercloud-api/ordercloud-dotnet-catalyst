using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.PayPal.Models;

namespace OrderCloud.Integrations.Payment.PayPal
{
    public class PayPalClient
    {
        protected static IFlurlRequest BuildClient(PayPalConfig config, string requestID)
        {
            var request = config.BaseUrl
                .WithBasicAuth(config.ClientID, config.SecretKey)
                .WithHeader("PayPal-Request-Id", requestID)
                .WithHeader("Prefer", "return=representation");

            if (config?.PartnerAttributionID != null)
            {
                request.WithHeader("PayPal-Partner-Attribution-Id", config.PartnerAttributionID);
            }

            return request;
        }

        #region Step 1: Create order with Authorize intent
        // https://developer.paypal.com/docs/api/orders/v2/#orders_create
        public static async Task<PayPalOrder> CreateAuthorizedOrderAsync(PayPalConfig config, List<PurchaseUnit> purchaseUnits, AuthorizeCCTransaction transaction, bool isCapture)
        {
            var paymentSource = new PaymentSource();
            if (transaction.CardDetails != null)
            {
                paymentSource.card = new Card();
                if (transaction.CardDetails.SavedCardID != null)
                {
                    paymentSource.card.vault_id = transaction.CardDetails.SavedCardID;
                } else if (transaction.CardDetails.Token != null)
                {
                    paymentSource.card.single_use_token = transaction.CardDetails.Token;
                }
            } else if (transaction.AddressVerification != null)
            {
                paymentSource.paypal = new Models.PayPal()
                {
                    experience_context = new ExperienceContext() { shipping_preference = "SET_PROVIDED_ADDRESS" }
                };
            }

            return await BuildClient(config, transaction.RequestID)
                .AppendPathSegments("v2", "checkout", "orders")
                .PostJsonAsync(new
                {
                    intent = isCapture ? "CAPTURE" : "AUTHORIZE",
                    purchase_units = purchaseUnits,
                    payment_source = (paymentSource?.card != null || paymentSource.paypal != null) ? paymentSource : null
                })
                .ReceiveJson<PayPalOrder>();
        }
        #endregion

        #region Step 2: Authorize the order
        // https://developer.paypal.com/docs/api/orders/v2/#orders_authorize
        public static async Task<PayPalOrder> AuthorizePaymentForOrderAsync(PayPalConfig config, AuthorizeCCTransaction transaction)
        {
            return await BuildClient(config, transaction.RequestID)
                .AppendPathSegments("v2", "checkout", "orders", transaction.OrderID, "authorize")
                .PostJsonAsync(new { })
                .ReceiveJson<PayPalOrder>();
        }
        #endregion

        #region Step 3: Capture the order
        // capture previously authorized payment
        // https://developer.paypal.com/docs/api/payments/v2/#authorizations_capture
        public static async Task<PayPalOrder> CapturePriorAuthAsync(PayPalConfig config, FollowUpCCTransaction transaction)
        {
            return await BuildClient(config, transaction.RequestID)
                .AppendPathSegments("v2", "payments", "authorizations", transaction.TransactionID, "capture")
                .PostJsonAsync(new { })
                .ReceiveJson<PayPalOrder>();
        }

        // OR capture payment immediately without prior auth
        // https://developer.paypal.com/docs/api/orders/v2/#orders_capture
        public static async Task<PayPalOrder> CapturePaymentAsync(PayPalConfig config, FollowUpCCTransaction transaction)
        {
            return await BuildClient(config, transaction.RequestID)
                .AppendPathSegments("v2", "checkout", "orders", transaction.TransactionID, "capture")
                .PostJsonAsync(new { })
                .ReceiveJson<PayPalOrder>();
        }
        #endregion

        // https://developer.paypal.com/docs/api/payments/v2/#authorizations_void
        public static async Task<IFlurlResponse> VoidPaymentAsync(PayPalConfig config, FollowUpCCTransaction transaction)
        {
            return await BuildClient(config, transaction.RequestID)
                .AppendPathSegments("v2", "payments", "authorizations", transaction.TransactionID, "void")
                .PostJsonAsync(new { });
        }

        // https://developer.paypal.com/docs/api/payments/v2/#captures_refund
        public static async Task<PayPalOrderReturn> RefundPaymentAsync(PayPalConfig config, FollowUpCCTransaction transaction)
        {
            // get capture details to get the currency
            var captureDetails = await BuildClient(config, transaction.RequestID)
                .AppendPathSegments("v2", "payments", "captures", transaction.TransactionID)
                .GetJsonAsync<PayPalCapture>();

            return await BuildClient(config, transaction.RequestID)
                .AppendPathSegments("v2", "payments", "captures", transaction.TransactionID, "refund")
                .PostJsonAsync(new
                {
                    amount = new
                    {
                        value = transaction.Amount.ToString(),
                        captureDetails.amount.currency_code
                    },
                    captureDetails.invoice_id
                }).ReceiveJson<PayPalOrderReturn>();
        }

        public static async Task<string> CreateVaultSetupToken(PayPalConfig config)
        {
            var response = await BuildClient(config, Guid.NewGuid().ToString())
                .AppendPathSegments("v3", "vault", "setup-tokens")
                .PostJsonAsync(new
                {
                    payment_source = new PaymentSource()
                    {
                        card = new Card()
                    }
                });

            var tokenResponse = await response.GetJsonAsync<PayPalPaymentToken>();
            return tokenResponse.id;
        }

        // https://developer.paypal.com/docs/api/payment-tokens/v3/#payment-tokens_create
        public static async Task<PayPalPaymentToken> CreatePaymentTokenAsync(PayPalConfig config, PCISafeCardDetails card, PaymentSystemCustomer customer)
        {
            var response =  await BuildClient(config, Guid.NewGuid().ToString())
                .AppendPathSegments("v3", "vault", "payment-tokens")
                .PostJsonAsync(new
                {
                    payment_source = new PaymentSource()
                    {
                        token = new PaymentToken()
                        {
                            id = card.Token,
                            type = "SETUP_TOKEN"
                        }
                    }
                });

            return await response.GetJsonAsync<PayPalPaymentToken>();
        }

        // https://developer.paypal.com/docs/api/payment-tokens/v3/#customer_payment-tokens_get
        public static async Task<PaymentTokenResponse> ListPaymentTokensAsync(PayPalConfig config, string customerID)
        {
            return await BuildClient(config, Guid.NewGuid().ToString())
                .AppendPathSegments("v3", "vault", "payment-tokens")
                .SetQueryParam("customer_id", customerID)
                .GetJsonAsync<PaymentTokenResponse>();
        }

        // https://developer.paypal.com/docs/api/payment-tokens/v3/#payment-tokens_get
        public static async Task<PayPalPaymentToken> GetPaymentTokenAsync(PayPalConfig config, string tokenID)
        {
            return await BuildClient(config, Guid.NewGuid().ToString())
                .AppendPathSegments("v3", "vault", "payment-tokens", tokenID)
                .GetJsonAsync<PayPalPaymentToken>();
        }

        // https://developer.paypal.com/docs/api/payment-tokens/v3/#payment-tokens_deletes
        public static async Task DeletePaymentTokenAsync(PayPalConfig config, string tokenID)
        {
            await BuildClient(config, Guid.NewGuid().ToString())
                .AppendPathSegments("v3", "vault", "payment-tokens", tokenID)
                .DeleteAsync();
        }
    }
}
