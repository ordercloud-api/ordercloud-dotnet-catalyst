using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.PayPal.Models;

namespace OrderCloud.Integrations.Payment.PayPal.Mappers
{
    public class PayPalOrderPaymentMapper
    {
        public List<PurchaseUnit> MapToPurchaseUnit(AuthorizeCCTransaction transaction, PayPalConfig config)
        {
            PayPalAddress address = null;
            if (transaction.AddressVerification != null)
            {
                address = new PayPalAddress()
                {
                    address_line_1 = transaction.AddressVerification?.Street1,
                    address_line_2 = transaction.AddressVerification?.Street2,
                    admin_area_1 = transaction.AddressVerification?.State,
                    admin_area_2 = transaction.AddressVerification?.City,
                    postal_code = transaction.AddressVerification?.Zip,
                    country_code = transaction.AddressVerification?.Country
                };
            }
            var purchaseUnits = new List<PurchaseUnit>();
            if (config.Merchants.Any())
            {
                config.Merchants.ForEach(m =>
                {
                    var merchantLines =
                        transaction?.OrderWorksheet?.LineItems?.Where(li => li.Product.DefaultSupplierID == m.SupplierID).ToList();
                    if (merchantLines != null && merchantLines.Any())
                    {
                        var merchantUnit = new PurchaseUnit()
                        {
                            amount = new Amount()
                            {
                                currency_code = transaction.Currency,
                                value = merchantLines.Sum(li => li.LineTotal).ToString(CultureInfo.InvariantCulture) ??
                                        null // sum Amount for each merchant
                            },
                            payee = new Payee()
                            {
                                merchant_id = m.MerchantID,
                            },
                            description = transaction?.OrderWorksheet?.Order?.Comments,
                            reference_id = Guid.NewGuid().ToString(),
                            invoice_id = Guid.NewGuid().ToString(),
                        };
                        if (address != null)
                        {
                            merchantUnit.shipping = new Shipping()
                            {
                                address = address
                            };
                        }
                    
                        purchaseUnits.Add(merchantUnit);
                    }
                });
            }

            var unit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = transaction.Currency,
                    value = transaction.Amount.ToString(CultureInfo.InvariantCulture) ?? null
                }
            };
            if (address != null)
            {
                unit.shipping = new Shipping()
                {
                    address = address
                };
            }
            purchaseUnits.Add(unit);

            return purchaseUnits;
        }

        public CCTransactionResult MapAuthorizedPaymentToCCTransactionResult(PayPalOrder authorizedOrder)
        {
            var innerTransactions = new List<CCTransactionResult>();
            authorizedOrder.purchase_units.ForEach(u =>
            {
                var capture = u.payments.authorizations.FirstOrDefault();
                if (capture != null)
                {
                    innerTransactions.Add(new CCTransactionResult()
                    {
                        TransactionID = capture.id,
                        Amount = ConvertStringAmountToDecimal(u.amount.value),
                        Succeeded = capture.status.ToLowerInvariant() == "completed",
                        MerchantID = u.payee.merchant_id
                    });
                }
            });
            var ccTransaction = new CCTransactionResult
            {
                Succeeded = authorizedOrder.status.ToLowerInvariant() == "completed",
                TransactionID = authorizedOrder.id, // Authorization ID needed to Capture payment or Void Authorization
                ResponseCode = authorizedOrder.processor_response.response_code,
                AuthorizationCode = null,
                AVSResponseCode = authorizedOrder.processor_response.avs_code,
                Message = null,
                Amount = authorizedOrder.purchase_units.Sum(unit => ConvertStringAmountToDecimal(unit.amount.value)),
                InnerTransactions = innerTransactions
            };
            return ccTransaction;
        }

        public CCTransactionResult MapCapturedPaymentToCCTransactionResult(PayPalOrder capturedOrder)
        {
            var innerTransactions = new List<CCTransactionResult>();
            capturedOrder.purchase_units.ForEach(u =>
            {
                var capture = u.payments.captures.FirstOrDefault();
                if (capture != null)
                {
                    innerTransactions.Add(new CCTransactionResult()
                    {
                        TransactionID = capture.id,
                        Amount = ConvertStringAmountToDecimal(u.amount.value),
                        Succeeded = capture.status.ToLowerInvariant() == "completed",
                        MerchantID = u.payee.merchant_id
                    });
                }
            });
            return new CCTransactionResult
            {
                TransactionID = capturedOrder.id, // Capture ID needed to Refund payment
                ResponseCode = capturedOrder.processor_response.response_code,
                AuthorizationCode = null,
                AVSResponseCode = capturedOrder.processor_response.avs_code,
                Message = null,
                Succeeded = capturedOrder.status.ToLowerInvariant() == "completed",
                Amount = capturedOrder.purchase_units.Sum(unit => ConvertStringAmountToDecimal(unit.amount.value)),
                InnerTransactions = innerTransactions
            };
        }
           

        public CCTransactionResult MapRefundPaymentToCCTransactionResult(PayPalOrderReturn orderReturn) =>
            new CCTransactionResult
            {
                Succeeded = orderReturn.status.ToLowerInvariant() == "completed" && orderReturn.id != null,
                Amount = ConvertStringAmountToDecimal(orderReturn.amount.value),
                TransactionID = orderReturn.id,
                ResponseCode = null,
                AuthorizationCode = null,
                AVSResponseCode = null,
                Message = orderReturn.note
            };

        private decimal ConvertStringAmountToDecimal(string value) =>
            Convert.ToDecimal(value, CultureInfo.InvariantCulture);
    }
}
