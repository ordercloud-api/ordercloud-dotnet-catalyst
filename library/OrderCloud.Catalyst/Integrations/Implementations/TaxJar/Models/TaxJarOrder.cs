using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class TaxJarOrder
	{
		public decimal sales_tax { get; set; }
		public decimal shipping { get; set; }
		public decimal amount { get; set; }
		public string to_street { get; set; }
		public string to_city { get; set; }
		public string to_state { get; set; }
		public string to_zip { get; set; }
		public string to_country { get; set; }
		public string from_street { get; set; }
		public string from_city { get; set; }
		public string from_state { get; set; }
		public string from_zip { get; set; }
		public string from_country { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public string exemption_type { get; set; }
		public string provider { get; set; }
		public string transaction_date { get; set; }
		public string transaction_id { get; set; }
		public string customer_id { get; set; }
		public List<TaxJarLineItem> line_items { get; set; }
	}

	public class TaxJarLineItem
	{
		public string id { get; set; }
		public int quantity { get; set; }
		public string product_identifier { get; set; }
		public string description { get; set; }
		public string product_tax_code { get; set; }
		public decimal unit_price { get; set; }
		public decimal discount { get; set; }
		public decimal sales_tax { get; set; }
	}
}
