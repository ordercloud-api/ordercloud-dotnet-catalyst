using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderCloud.Catalyst;
using OrderCloud.Integrations.Payment.PayPal.Mappers;

namespace OrderCloud.Integrations.Payment.PayPal
{
    public class PayPalService : OCIntegrationService, ICreditCardProcessor, ICreditCardSaver
    {
        #region ICreditCardProcessor

        public Task<string> GetIFrameCredentialAsync(OCIntegrationConfig overrideConfig = null)
        {
            throw new NotImplementedException("Not required for PayPal");
        }

        public async Task<CCTransactionResult> InitializePaymentRequestAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig = null, bool isCapture = false)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            var purchaseUnitMapper = new PayPalOrderPaymentMapper();
            var purchaseUnits = purchaseUnitMapper.MapToPurchaseUnit(transaction, config);
            var order = await PayPalClient.CreateAuthorizedOrderAsync(config, purchaseUnits, transaction, isCapture);
            return new CCTransactionResult
            {
                Succeeded = new List<string>{ "created", "completed" }.Contains(order.status.ToLowerInvariant()),
                Amount = transaction.Amount,
                TransactionID = order.id
            };
        }

        public async Task<CCTransactionResult> AuthorizeOnlyAsync(AuthorizeCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            // AuthorizeCCTransaction.OrderID represents PayPal OrderID, NOT OrderCloud OrderID
            var authorizedPaymentForOrder = await PayPalClient.AuthorizePaymentForOrderAsync(config, transaction);
            var ccTransactionMapper = new PayPalOrderPaymentMapper();
            return ccTransactionMapper.MapAuthorizedPaymentToCCTransactionResult(authorizedPaymentForOrder);
        }

        public async Task<CCTransactionResult> CapturePriorAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            // FollowUpCCTransaction.TransactionID represents the PayPal Authorization ID
            var capturedPaymentForOrder = await PayPalClient.CapturePriorAuthAsync(config, transaction);
            var ccTransactionMapper = new PayPalOrderPaymentMapper();
            var ccTransaction = ccTransactionMapper.MapCapturedPaymentToCCTransactionResult(capturedPaymentForOrder);
            ccTransaction.Amount = transaction.Amount; // PayPal Capture Authorized Payment doesn't return an amount in the response body
            return ccTransaction;
        }

        public async Task<CCTransactionResult> CapturePaymentAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            // FollowUpCCTransaction.TransactionID represents the PayPal Authorization ID
            var capturedPaymentForOrder = await PayPalClient.CapturePaymentAsync(config, transaction);
            var ccTransactionMapper = new PayPalOrderPaymentMapper();
            var ccTransaction = ccTransactionMapper.MapCapturedPaymentToCCTransactionResult(capturedPaymentForOrder);
            return ccTransaction;
        }

        public async Task<CCTransactionResult> VoidAuthorizationAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            // FollowUpCCTransaction.TransactionID represents the PayPal Authorization ID
            var voidTransactionResponse = await PayPalClient.VoidPaymentAsync(config, transaction);
            return new CCTransactionResult() 
            {
                Amount = transaction.Amount,
                Succeeded = voidTransactionResponse.StatusCode == 204
            };
        }

        public async Task<CCTransactionResult> RefundCaptureAsync(FollowUpCCTransaction transaction, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            // FollowUpCCTransaction.TransactionID represents the PayPal Capture ID
            var orderReturn = await PayPalClient.RefundPaymentAsync(config, transaction);
            var ccTransactionMapper = new PayPalOrderPaymentMapper();
            return ccTransactionMapper.MapRefundPaymentToCCTransactionResult(orderReturn);
            // CCTransactionResult.TransactionID represents PayPal Refund ID
        }

        #endregion

        #region ICreditCardSaver
        public PayPalService(OCIntegrationConfig defaultConfig) : base(defaultConfig)
        {
        }

        public async Task<string> InitializeCreateSavedCardRequestAsync(OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            return await PayPalClient.CreateVaultSetupToken(config);
        }

        public async Task<List<PCISafeCardDetails>> ListSavedCardsAsync(string customerID, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            var paymentTokenResponse = await PayPalClient.ListPaymentTokensAsync(config, customerID);
            var listOfCardDetails = new List<PCISafeCardDetails>();
            var cardDetailsMapper = new PayPalPaymentTokensMapper();
            foreach (var paymentToken in paymentTokenResponse.payment_tokens)
            {
                var mappedDetails = cardDetailsMapper.MapPaymentTokenToPCISafeCardDetails(paymentToken);
                listOfCardDetails.Add(mappedDetails);
            }

            return listOfCardDetails;
        }

        public async Task<PCISafeCardDetails> GetSavedCardAsync(string customerID, string cardID, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            var paymentToken = await PayPalClient.GetPaymentTokenAsync(config, cardID);
            var cardDetailsMapper = new PayPalPaymentTokensMapper();
            return cardDetailsMapper.MapPaymentTokenToPCISafeCardDetails(paymentToken);
        }

        public async Task<CardCreatedResponse> CreateSavedCardAsync(PaymentSystemCustomer customer, PCISafeCardDetails card,
            OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            var paymentToken = await PayPalClient.CreatePaymentTokenAsync(config, card, customer);
            var cardDetailsMapper = new PayPalPaymentTokensMapper();
            return cardDetailsMapper.MapPaymentTokenToCardCreatedResponse(paymentToken);
        }

        public async Task DeleteSavedCardAsync(string customerID, string cardID, OCIntegrationConfig overrideConfig = null)
        {
            var config = ValidateConfig<PayPalConfig>(overrideConfig ?? _defaultConfig);
            await PayPalClient.DeletePaymentTokenAsync(config, cardID);
        }
        #endregion
    }
}
