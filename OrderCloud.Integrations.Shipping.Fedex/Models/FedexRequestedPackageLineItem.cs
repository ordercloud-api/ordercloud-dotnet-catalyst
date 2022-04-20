using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexRequestedPackageLineItem
	{
		public string subPackagingType { get; set; }
		public int groupPackageCount { get; set; }
		public List<FedexContentRecord> contentRecord { get; set; } = new List<FedexContentRecord>();
		public FedexMoney declaredValue { get; set; } = new FedexMoney();
		public FedexWeight weight { get; set; } = new FedexWeight();
		public FedexDimensions dimensions { get; set; } = new FedexDimensions();
		/// <summary>
		/// Indicate the details on how to calculate variable handling charges at the package level. If indicated, element rateLevelType is required.
		/// </summary>
		public FedexVariableHandlingChargeDetail variableHandlingChargeDetail { get; set; } = new FedexVariableHandlingChargeDetail();
		public FedexPackageSpecialServices packageSpecialServices { get; set; } = new FedexPackageSpecialServices();
	}
	 
	public class FedexContentRecord
	{
		public string itemNumber { get; set; }
		public int receivedQuantity { get; set; }
		public string description { get; set; }
		public string partNumber { get; set; }
	}

	public class FedexMoney
	{
		public decimal amount { get; set; }
		public string currency { get; set; }
	}

	public class FedexWeight
	{
		/// <summary>
		/// "KG" or "LB"
		/// </summary>
		public string units { get; set; }
		/// <summary>
		/// Specifies the package weight. Example: 68.25
		/// </summary>
		public double value { get; set; }
	}

	public class FedexDimensions
	{
		public int length { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		/// <summary>
		/// "CM" "IN"
		/// </summary>
		public string units { get; set; }
	}

	public class FedexVariableHandlingChargeDetail
	{
		/// <summary>
		/// "ACCOUNT" "ACTUAL" "CURRENT" "CUSTOM" "LIST" "INCENTIVE" "PREFERRED" "PREFERRED_INCENTIVE" "PREFERRED_CURRENCY"
		/// </summary>
		public string rateType { get; set; }
		/// <summary>
		/// Indicate the variable handling percentage. Actual percentage (10 means 10%, which is a mutiplier of 0.1).
		/// </summary>
		public double percentValue { get; set; }
		/// <summary>
		/// "BUNDLED_RATE" "INDIVIDUAL_PACKAGE_RATE"
		/// </summary>
		public string rateLevelType { get; set; }
		/// <summary>
		/// If you choose FIXED_AMOUNT as the ChargeType, this element allows you to enter the fixed value of the handling charge. The element allows entry of 7 characters before the decimal and 2 characters following the decimal.
		/// </summary>
		public FedexMoney fixedValue { get; set; } = new FedexMoney();
		/// <summary>
		/// "NET_CHARGE" "NET_FREIGHT" "BASE_CHARGE" "NET_CHARGE_EXCLUDING_TAXES"
		/// </summary>
		public string rateElementBasis { get; set; }
	}

	public class FedexPackageSpecialServices
	{
		public List<string> specialServiceTypes { get; set; } = new List<string>();
		public FedexAlcoholDetails alcoholDetail { get; set; } = new FedexAlcoholDetails();
		public FedexDangerousGoodsDetails dangerousGoodsDetail { get; set; } = new FedexDangerousGoodsDetails();
		public FedexPackageCODDetails packageCODDetail { get; set; } = new FedexPackageCODDetails();
		public int pieceCountVerificationBoxCount { get; set; }
		public List<FedexBatteryDetails> batteryDetails { get; set; } = new List<FedexBatteryDetails>();
		public FedexWeight dryIceWeight { get; set; } = new FedexWeight();
	}

	public class FedexAlcoholDetails
	{
		/// <summary>
		/// "LICENSEE" "CONSUMER"
		/// </summary>
		public string alcoholRecipientType { get; set; }
		/// <summary>
		/// Specify the shipper entity type. Example: Fulfillment house, Retailer or a Winery
		/// </summary>
		public string shipperAgreementType { get; set; }
	}

	public class FedexDangerousGoodsDetails
	{
		/// <summary>
		/// Indicate the Offeror's name or contract number, per DOT regulation. Example: John Smith
		/// </summary>
		public string offeror { get; set; }
		/// <summary>
		/// "ACCESSIBLE" "INACCESSIBLE". Indicate the Dangerous Goods Accessibility Type. Inaccessible means it does not have to be accessible on the aircraft.Accessible means it must be fully accessible on the aircraft, and is more strictly controlled.
		/// </summary>
		public string accessibility { get; set; }
		/// <summary>
		/// Indicate the emergency telephone/contact number.
		/// </summary>
		public string emergencyContactNumber { get; set; }
		/// <summary>
		/// "HAZARDOUS_MATERIALS" "BATTERY" "ORM_D" "REPORTABLE_QUANTITIES" "SMALL_QUANTITY_EXCEPTION" "LIMITED_QUANTITIES_COMMODITIES"
		/// </summary>
		public List<string> options { get; set; } = new List<string>();
	}

	public class FedexPackageCODDetails
	{
		/// <summary>
		/// "ANY" "CASH" "COMPANY_CHECK" "GUARANTEED_FUNDS" "PERSONAL_CHECK". Indicate the type of funds FedEx should collect upon shipment delivery.
		/// </summary>
		public string codCollectionType { get; set; }
		public FedexMoney codCollectionAmount { get; set; } = new FedexMoney();
	}

	public class FedexBatteryDetails
	{
		/// <summary>
		/// "LITHIUM_METAL" "LITHIUM_ION"
		/// </summary>
		public string material { get; set; }
		/// <summary>
		/// Value: "IATA_SECTION_II". Specify the regulation specific classification for the battery or cell.
		/// </summary>
		public string regulatorySubType { get; set; }
		/// <summary>
		/// "CONTAINED_IN_EQUIPMENT" "PACKED_WITH_EQUIPMENT". Indicate the packing arrangement of the battery or cell with respect to other items within the same package.
		/// </summary>
		public string packing { get; set; }
	}
}
