using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
    /// <summary>
    /// https://www.easypost.com/docs/api#shipment-object
    /// </summary>
    public class EasyPostShipment
	{
        public string id { get; set; }
        public string mode { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public string tracking_code { get; set; }
        public string reference { get; set; }
        public string status { get; set; }
        public bool? is_return { get; set; }
        public EasyPostAddress from_address { get; set; }
        public EasyPostAddress to_address { get; set; }
        public EasyPostParcel parcel { get; set; }
        public List<EasyPostRate> rates { get; set; }
        public EasyPostRate selected_rate { get; set; }
        public EasyPostAddress buyer_address { get; set; }
        public EasyPostAddress return_address { get; set; }
        public string refund_status { get; set; }
        public EasyPostInsurance insurance { get; set; }
        public string batch_status { get; set; }
        public string batch_message { get; set; }
        public string usps_zone { get; set; }
        public List<EasyPostMessage> messages { get; set; }
        public EasyPostOptions options { get; set; } 
        public List<EasyPostCarrierAccount> carrier_accounts { get; set; }
        public string batch_id { get; set; }
        public string order_id { get; set; }
        public EasyPostCustomsInfo customs_info { get; set; }
    }

    /// <summary>
    /// https://www.easypost.com/docs/api#customs-info-object
    /// </summary>
    public class EasyPostCustomsInfo
    {
        public bool customs_certify { get; set; }
        public string customs_signer { get; set; }
        public string contents_type { get; set; }
        public string restriction_type { get; set; }
        public string eel_pfc { get; set; }
        public List<EasyPostCustomsItem> customs_items { get; set; } = new List<EasyPostCustomsItem>();
    }

    /// <summary>
    /// https://www.easypost.com/docs/api#customs-item-object
    /// </summary>
    public class EasyPostCustomsItem
    {
        public string description { get; set; }
        public int quantity { get; set; }
        public decimal weight { get; set; }
        public decimal value { get; set; }
        public string hs_tariff_number { get; set; }
        public string origin_country { get; set; }
    }

    public class EasyPostMessage
    {
        public string type { get; set; }
        public string carrier { get; set; }
        public string message { get; set; }
    };
}
