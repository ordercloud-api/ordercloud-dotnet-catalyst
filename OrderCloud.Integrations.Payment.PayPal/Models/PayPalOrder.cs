using System.Collections.Generic;

namespace OrderCloud.Integrations.Payment.PayPal.Models
{
    public class PayPalOrder
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<RelatedLink> links { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public PaymentSource payment_source { get; set; }
        public Payer payer { get; set; }
        public ProcessorResponse processor_response { get; set; }
    }

    public class ProcessorResponse {
        public string avs_code { get; set; }
        public string cvv_code { get; set; }
        public string response_code { get; set; }
    }

    public class Payer
    {
        public Name name { get; set; }
        public string email_address { get; set; }
        public string payer_id { get; set; }
    }

    public class Payee
    {
        public string merchant_id { get; set; }
    }

    public class RelatedLink
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

    public class PayPalAddress
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }

        public string admin_area_1 { get; set; }
        public string admin_area_2 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class Shipping
    {
        public PayPalAddress address { get; set; }
    }

    public class PurchaseUnit
    {
        // The merchant ID for the purchase unit.
        public string reference_id { get; set; }
        public string description { get; set; }
        public string invoice_id { get; set; }
        public Amount amount { get; set; }
        public PurchaseUnitPayment payments { get; set; }
        public Shipping shipping { get; set; }
        public Payee payee { get; set; }
    }

    public class PurchaseUnitPayment
    {
        public List<PurchaseUnitAuthorization> authorizations { get; set; }
        public List<PurchaseUnitAuthorization> captures { get; set; }
    }

    public class PurchaseUnitAuthorization
    {
        public string id { get; set; }
        public string status { get; set; }
        public Amount amount { get; set; }
        public List<RelatedLink> links { get; set; }
    }

    public class Amount
    {
        // The three-character ISO-4217 currency code.
        public string currency_code { get; set; }
        // The total amount charged to the payee by the payer. For refunds, represents the amount that the payee refunds to the original payer. Maximum length is 10 characters, which includes:
        // 
        // Seven digits before the decimal point.
        // The decimal point.
        // Two digits after the decimal point
        public string value { get; set; }
    }

    public class PaymentSource
    {
        public PayPal paypal { get; set; }
        public Card card { get; set; }
        public PaymentToken token { get; set; }
    }

    public class ExperienceContext
    {
        public string shipping_preference { get; set; }
    }

    public class PayPal
    {
        public Name name { get; set; }
        public string email_address { get; set; }
        public string account_id { get; set; }
        public ExperienceContext experience_context { get; set; }
    }

    public class Card
    {
        public string name { get; set; }
        public string last_digits { get; set; }
        public string expiry { get; set; }
        public string brand { get; set; }
        public string vault_id { get; set; }
        public string single_use_token { get; set; }
    }

    public class PaymentToken
    {
        public string id { get; set; }
        public string type { get; set; }
    }

    public class Name
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }

    public class PayPalOrderReturn
    {
        public string id { get; set; }
        public Amount amount { get; set; }
        public string status { get; set; }
        public string invoice_id { get; set; }
        public string note { get; set; }
    }

    public class PayPalCapture
    {
        public string id { get; set; }
        public string status { get; set; }
        public Amount amount { get; set; }
        public string invoice_id { get; set; }
    }

    public class PaymentTokenResponse
    {
        public PayPalCustomer customer { get; set; }
        public List<PayPalPaymentToken> payment_tokens { get; set; }
    }

    public class PayPalCustomer
    {
        public string id { get; set; }
        public string merchant_customer_id { get; set; }
    }

    public class PayPalPaymentToken
    {
        public string id { get; set; }
        public PayPalCustomer customer { get; set; }
        public PaymentSource payment_source { get; set; }
        public List<RelatedLink> links { get; set; }
        public string status { get; set; }
    }
}
