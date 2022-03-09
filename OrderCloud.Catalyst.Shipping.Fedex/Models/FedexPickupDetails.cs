using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexPickupDetails
	{
		public string companyCloseTime { get; set; }
		public FedexPickupOrigin pickupOrigin { get; set; } = new FedexPickupOrigin();
		public string geographicalPostalCode { get; set; }
		public string requestType { get; set; }
		public string buildingPartDescription { get; set; }
		public string courierInstructions { get; set; }
		/// <summary>
		/// "APARTMENT" "BUILDING" "DEPARTMENT" "FLOOR" "ROOM" "SUITE"
		/// </summary>
		public string buildingPart { get; set; }
		public string latestPickupDateTime { get; set; }
		public string packageLocation { get; set; }
		public string readyPickupDateTime { get; set; }
		public bool earlyPickup { get; set; }
	}

	public class FedexPickupOrigin
	{
		public string accountNumber { get; set; }
		public FedexPickupAddress address { get; set; } = new FedexPickupAddress();
		public FedexPickupContact contact { get; set; } = new FedexPickupContact();
	}

	public class FedexPickupAddress
	{
		public string city { get; set; }
		public string stateOrProvinceCode { get; set; }
		public string postalCode { get; set; }
		public string countryCode { get; set; }
		public bool residential { get; set; }
		public string addressVerificationId { get; set; }
		public List<string> streetLines { get; set; } = new List<string>();
	}

	public class FedexPickupContact
	{
		public string personName { get; set; }
		public string emailAddress { get; set; }
		public string phoneNumber { get; set; }
		public string phoneExtension { get; set; }
		public string faxNumber { get; set; }
		public string companyName { get; set; }
	}
}
