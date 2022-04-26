using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Shipping.EasyPost
{
	// https://www.easypost.com/docs/api#options
	public class EasyPostOptions
	{
		public bool additional_handling { get; set; }
		public string address_validation_level { get; set; }
		public bool alcohol { get; set; }
		public bool by_drone { get; set; }
		public bool carbon_neutral { get; set; }
		public string cod_amount { get; set; }
		public string cod_method { get; set; }
		public string cod_address_id { get; set; }
		public string currency { get; set; }
		public string delivery_confirmation { get; set; }
		public string dropoff_type { get; set; }
		public bool dry_ice { get; set; }
		public string dry_ice_medical { get; set; }
		public string dry_ice_weight { get; set; }
		public string endorsement { get; set; }
		public double freight_charge { get; set; }
		public string handling_instructions { get; set; }
		public string hazmat { get; set; }
		public bool hold_for_pickup { get; set; }
		public string incoterm { get; set; }
		public string invoice_number { get; set; }
		public string label_date { get; set; }
		public string label_format { get; set; }
		public bool machinable { get; set; }
		public object payment { get; set; }
		public string print_custom_1 { get; set; }
		public string print_custom_2 { get; set; }
		public string print_custom_3 { get; set; }
		public bool print_custom_1_barcode { get; set; }
		public bool print_custom_2_barcode { get; set; }
		public bool print_custom_3_barcode { get; set; }
		public string print_custom_1_code { get; set; }
		public string print_custom_2_code { get; set; }
		public string print_custom_3_code { get; set; }
		public bool saturday_delivery { get; set; }
		public string special_rates_eligibility { get; set; }
		public string smartpost_hub { get; set; }
		public string smartpost_manifest { get; set; }
		public string billing_ref { get; set; }
		public bool certified_mail { get; set; }
		public bool registered_mail { get; set; }
		public double registered_mail_amount { get; set; }
		public bool return_receipt { get; set; }
	}
}
