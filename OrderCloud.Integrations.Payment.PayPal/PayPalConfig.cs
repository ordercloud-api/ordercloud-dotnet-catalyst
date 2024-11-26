using System.Collections.Generic;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Payment.PayPal
{
    public class PayPalConfig : OCIntegrationConfig
    {
        public override string ServiceName { get; } = "PayPal";
        [RequiredIntegrationField]
        public string BaseUrl { get; set; }
        [RequiredIntegrationField]
        public string ClientID {get; set; }
        [RequiredIntegrationField]
        public string SecretKey { get; set; }
        /// <summary>
        /// Optional property. BN codes provide tracking on all transactions that originate or are associated with a particular partner.
        /// If provided, it will be included in all request headers: https://developer.paypal.com/docs/multiparty/accept-payments/#link-bncode
        /// </summary>
        public string PartnerAttributionID { get; set; }
        /// <summary>
        /// Optional property. A list of paypal merchant IDs that correspond with OrderCloud Suppliers.
        /// if provided, transactions will be split into multiple purchase_units by supplier line items.
        /// </summary>
        public List<PayPalMerchantConfig> Merchants { get; set; }
    }

    public class PayPalMerchantConfig
    {
        public string SupplierID { get; set; }
        public string MerchantID { get; set; }
    }
}
