using System;
using System.Collections.Generic;
using OrderCloud.Catalyst;
using Stripe;

namespace OrderCloud.Integrations.Payment.Stripe.Mappers
{
    /// <summary>
    /// https://stripe.com/docs/api/payment_intents
    /// </summary>
    public class StripePaymentIntentMapper
    {
        // See https://stripe.com/docs/currencies#zero-decimal
        private List<string> ZERO_DECIMAL_CURRIENCIES = new List<string> 
        {
            "BIF",
            "CLP",
            "DJF",
            "GNF",
            "JPY",
            "KMF",
            "KRW",
            "MGA",
            "PYG",
            "RWF",
            "UGX",
            "VND",
            "VUV",
            "XAF",
            "XOF",
            "XPF",
        };

        private bool IsZeroDecimalCurrency(string currencyCode) => ZERO_DECIMAL_CURRIENCIES.Contains(currencyCode);

        public PaymentIntentCreateOptions MapPaymentIntentCreateAndConfirmOptions(AuthorizeCCTransaction transaction) 
        {
            var coefficient = IsZeroDecimalCurrency(transaction.Currency) ? 1 : 100;
            return new PaymentIntentCreateOptions()
            {
                Amount = Convert.ToInt64((transaction.Amount * coefficient)),
                Confirm = true, // Creates and Confirms PaymentIntent, otherwise Confirm PaymentIntent would be a separate call
                CaptureMethod = "manual", // Required value for separate auth and capture
                Currency = transaction.Currency,
                Customer = transaction.ProcessorCustomerID,
                PaymentMethod = transaction?.CardDetails?.SavedCardID ?? transaction?.CardDetails?.Token, // Represents PaymentMethodID
                Metadata = MapPaymentIntentMetaData(transaction)
            };
        }

        public CCTransactionResult MapPaymentIntentCreateAndConfirmResponse(PaymentIntent createdPaymentIntent) =>
            new CCTransactionResult()
            {
                Message = createdPaymentIntent.Status,
                Succeeded = createdPaymentIntent.Status.ToLower() == "requires_capture",
                TransactionID =
                    createdPaymentIntent
                        .Id, // transaction.TransactionID represents PaymentMethodID, this now represents PaymentIntentID
                Amount = createdPaymentIntent.Amount
            };

        public PaymentIntentCaptureOptions MapPaymentIntentCaptureOptions(FollowUpCCTransaction transaction)
        {
            var options = new PaymentIntentCaptureOptions();
            if (transaction.Amount > 0)
                // defaults to full amount_capturable if not provided
                options.AmountToCapture = Convert.ToInt64(transaction.Amount);
            return options;
        }

        public CCTransactionResult MapPaymentIntentCaptureResponse(PaymentIntent capturedPaymentIntent) =>
            new CCTransactionResult()
            {
                Message = capturedPaymentIntent.Status,
                Succeeded = capturedPaymentIntent.Status.ToLower() == "succeeded",
                TransactionID = capturedPaymentIntent.Id,
                Amount = capturedPaymentIntent.Amount
            };

        public PaymentIntentCancelOptions MapPaymentIntentCancelOptions(FollowUpCCTransaction transaction) =>
            new PaymentIntentCancelOptions()
                { };

        public CCTransactionResult MapPaymentIntentCancelResponse(PaymentIntent canceledPaymentIntent) =>
            new CCTransactionResult()
            {
                Message = canceledPaymentIntent.Status,
                Succeeded = canceledPaymentIntent.Status.ToLower() == "canceled",
                TransactionID = canceledPaymentIntent.Id,
                Amount = canceledPaymentIntent.Amount
            };

        private Dictionary<string, string> MapPaymentIntentMetaData(AuthorizeCCTransaction transaction)
		{
			var metadata = new Dictionary<string, string>
			{
				{ "Authorize-Request-IP-Address", transaction.CustomerIPAddress },
				{ "OrderCloud-Order-ID", transaction.OrderID },
				{ "OrderCloud-Order-FromUser-ID", transaction.OrderWorksheet.Order.FromUser.ID },
				{ "OrderCloud-Order-FromUser-FirstName", transaction.OrderWorksheet.Order.FromUser.FirstName},
                { "OrderCloud-Order-FromUser-LastName", transaction.OrderWorksheet.Order.FromUser.LastName},
                { "OrderCloud-Order-FromUser-Email", transaction.OrderWorksheet.Order.FromUser.Email },
				{ "OrderCloud-Order-FromCompany-ID", transaction.OrderWorksheet.Order.FromCompanyID },
                { "OrderCloud-Order-Billing-Address", MapAddressToString(transaction.OrderWorksheet.Order.BillingAddress) },
            };
            return metadata;
        }

        private string MapAddressToString(OrderCloud.SDK.Address a) => $"{a.Street1} {a.Street2} {a.City}, {a.State} {a.Zip}. {a.Country}";
    }
}
