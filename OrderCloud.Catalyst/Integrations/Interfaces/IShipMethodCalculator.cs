using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	public interface IShipMethodCalculator
	{
		Task<List<List<ShipMethod>>> CalculateShipMethodsAsync(IEnumerable<ShipPackage> shippingPackages, OCIntegrationConfig configOverride = null);
	}

	// inherit from SDK to expose candidates for platform additions
	// avoid circular deps
	public class ShipPackage
	{
		public decimal Length { get; set; }
		public decimal Width { get; set; } 
		public decimal Height { get; set; }
		public decimal Weight { get; set; }
		public Address ShipFrom { get; set; }
		public Address ShipTo { get; set; } // maybe needs residential flag
		public Address ReturnAddress { get; set; }
		public bool SignatureRequired { get; set; }
		public InsuranceInfo Insurance { get; set; }
		public CustomsInfo Customs { get; set; }
	}

	public class CustomsInfo
	{
		/// <summary>
		/// "EEL" or "PFC" value less than $2500: "NOEEI 30.37(a)"; value greater than $2500
		/// </summary>
		public string EEL_PFC { get; set; }
		/// <summary>
		/// "documents", "gift", "merchandise", "returned_goods", "sample", or "other"
		/// </summary>
		//[MaxLength(250)]
		public string ContentType { get; set; }
		/// <summary>
		/// Human readable description of content. Required for certain carriers and always required if contents_type is "other"
		/// </summary>
		public string Explanation { get; set; }
		/// <summary>
		/// Abandon or return. Default to return
		/// </summary>
		public CustomsNonDeliveryOption NonDeliveryOption { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public List<CustomsItem> Items { get; set; } = new List<CustomsItem> { };

	}

	public class CustomsItem
	{
		public string Description { get; set; }
		public decimal UnitPrice { get; set; }
		public string Currency { get; set; } = "USD"; // default
		public int Quantity { get; set; }
		public decimal UnitWeight { get; set; }
	}

	public enum CustomsNonDeliveryOption
	{
		Return,
		Abandon
	}

	public class InsuranceInfo
	{
		public decimal Amount { get; set; } // Easypost only takes in USD. Fedex, you can specify a currency. 
		public string Currency { get; set; } = "USD"; // default
	}
}
