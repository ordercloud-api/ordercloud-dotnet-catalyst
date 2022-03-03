using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexCustomsClearanceDetails
	{
		public FedexCommercialInvoice commercialInvoice { get; set; } = new FedexCommercialInvoice();
		/// <summary>
		/// "CARRIER_RISK" "OWN_RISK"
		/// </summary>
		public string freightOnValue { get; set; }
		public FedexDutiesPayment dutiesPayment { get; set; }
		public List<FedexCustomsCommodity> commodities { get; set; } = new List<FedexCustomsCommodity>();
	}

	public class FedexCommercialInvoice
	{
		/// <summary>
		/// "GIFT" "NOT_SOLD" "PERSONAL_EFFECTS" "REPAIR_AND_RETURN" "SAMPLE" "SOLD" "COMMERCIAL" "RETURN_AND_REPAIR" "PERSONAL_USE"
		/// </summary>
		public string shipmentPurpose { get; set; }
	}

	public class FedexDutiesPayment
	{
		/// <summary>
		///"SENDER". Indicate the payment Type.Applicable for Express and Ground rates.
		/// </summary>
		public string paymentType { get; set; }
	}

	public class FedexCustomsCommodity
	{
		/// <summary>
		/// Indicate the description of the dutiable packages. Maximum Length is 450. Example: DOCUMENTS
		/// </summary>
		public string description { get; set; }
		public FedexWeight weight { get; set; } = new FedexWeight();
		public int quantity { get; set; }
		public FedexMoney customsValue { get; set; } = new FedexMoney();
		public int numberOfPieces { get; set; }
		public string countryOfManufacture { get; set; }
		public string quantityUnits { get; set; }
		public string name { get; set; }
		public string partNumber { get; set; }
		/// <summary>
		/// This is to specify the Harmonized Tariff System (HTS) code to meet U.S. and foreign governments' customs requirements. These are mainly used to estimate the duties and taxes.
		/// </summary>
		public string harmonizedCode { get; set; }
	}
}
