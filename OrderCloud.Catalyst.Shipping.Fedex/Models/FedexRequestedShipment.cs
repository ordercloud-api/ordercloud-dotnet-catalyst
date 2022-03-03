using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexRequestedShipment
	{
		public FedexAddressWrapper shipper { get; set; } = new FedexAddressWrapper();
		public FedexAddressWrapper recipient { get; set; } = new FedexAddressWrapper();
		public FedexEmailNotificationDetail emailNotificationDetail { get; set; } = new FedexEmailNotificationDetail();

		/// <summary>
		/// Indicate the FedEx service type used for this shipment. The results will be filtered by the service type value indicated. If no serviceType is indicated then all the applicable services and corresponding rates will be returned.
		/// </summary>
		public string serviceType { get; set; }
		/// <summary>
		/// ndicate the currency the caller requests to have used in all returned monetary values (when a choice is possible). Used in conjunction with the rateRequestType data element. This element is used to pull Preferred rates.
		/// </summary>
		public string preferredCurrency { get; set; }
		/// <summary>
		/// Indicate the type of rates to be returned. "LIST" "INCENTIVE" "ACCOUNT" "PREFERRED"
		/// </summary>
		public List<string> rateRequestType { get; set; } = new List<string>();
		/// <summary>
		/// Indicate the Shipment date. This is required for future ship date rates. Defaulted to current date in case if not provided or input date is a past date. The format is YYYY-MM-DD
		/// </summary>
		public string shipDateStamp { get; set; }
		/// <summary>
		/// "CONTACT_FEDEX_TO_SCHEDULE" "DROPOFF_AT_FEDEX_LOCATION" "USE_SCHEDULED_PICKUP". Indicate the pickup type method by which the shipment to be tendered to FedEx.
		/// </summary>
		public string pickupType { get; set; }
		public FedexPickupDetails pickupDetail { get; set; } = new FedexPickupDetails();
		public List<FedexRequestedPackageLineItem> requestedPackageLineItems { get; set; } = new List<FedexRequestedPackageLineItem>();
 
		/// <summary>
		/// Indicate whether or not its a document Shipment.
		/// </summary>
		public bool documentShipment { get; set; }

		public FedexVariableHandlingChargeDetail variableHandlingChargeDetail { get; set; } = new FedexVariableHandlingChargeDetail();
		/// <summary>
		/// This is the Packaging type associated with this rate. For Ground/SmartPost,it will always be YOUR_PACKAGING. For domestic Express, the packaging may have been bumped so it may not match the value specified on the request. For International Express the packaging may be bumped and not mapped.
		/// </summary>
		public string packagingType { get; set; }
		/// <summary>
		/// Indicate the total number of packages in the shipment.
		/// </summary>
		public int totalPackageCount { get; set; }
		/// <summary>
		/// Specify the total weight of the shipment. This only applies to International shipments and should be used on the first package of a multiple piece shipment.This value contains 1 explicit decimal position.
		/// </summary>
		public double totalWeight { get; set; }
		public FedexCustomsClearanceDetails customsClearanceDetail { get; set; } = new FedexCustomsClearanceDetails();
		public bool groupShipment { get; set; }
		public FedexServiceTypeDetails serviceTypeDetail { get; set; } = new FedexServiceTypeDetails();
		public FedexSmartPostInfoDetails smartPostInfoDetail { get; set; } = new FedexSmartPostInfoDetails();
		public FedexExpressFreightDetails expressFreightDetail { get; set; } = new FedexExpressFreightDetails();
		/// <summary>
		/// If set to 'true', indicates it is a Ground shipment.
		/// </summary>
		public bool groundShipment { get; set; }
	}

	public class FedexAddressWrapper
	{
		public FedexAddress address { get; set; } = new FedexAddress();
	}

	public class FedexEmailNotificationDetail
	{
		public List<FedexNotificationRecipient> recipients { get; set; } = new List<FedexNotificationRecipient>();
		public string personalMessage { get; set; }
		public FedexPrintedReference PrintedReference { get; set; } = new FedexPrintedReference();
	}

	public class FedexNotificationRecipient
	{
		/// <summary>
		/// Identifies the email address associated with this contact.
		/// </summary>
		public string emailAddress { get; set; }
		/// <summary>
		/// "ON_DELIVERY" "ON_EXCEPTION" "ON_SHIPMENT" "ON_TENDER" "ON_ESTIMATED_DELIVERY" "ON_PICKUP" "ON_LABEL" "ON_BILL_OF_LADING"
		/// </summary>
		public List<string> notificationEventType { get; set; } = new List<string>();
		/// <summary>
		/// Enum: "HTML" "TEXT". Specifies Notification Format Type.
		/// </summary>
		public string notificationFormatType { get; set; }
		/// <summary>
		/// "BROKER" "OTHER" "RECIPIENT" "SHIPPER" "THIRD_PARTY" "OTHER1" "OTHER2"
		/// </summary>
		public string emailNotificationRecipientType { get; set; }
		/// <summary>
		/// "EMAIL" "SMS_TEXT_MESSAGE"
		/// </summary>
		public string notificationType { get; set; }
		/// <summary>
		/// Specify the locale details. Example: 'en_US'
		/// </summary>
		public string locale { get; set; }
		public FedexSMSDetails smsDetail { get; set; } = new FedexSMSDetails();
	}

	public class FedexSMSDetails
	{
		public string phoneNumberCountryCode { get; set; }
		public string phoneNumber { get; set; }
	}

	public class FedexPrintedReference
	{
		/// <summary>
		/// "BILL_OF_LADING" "CONSIGNEE_ID_NUMBER" "INTERLINE_PRO_NUMBER" "PO_NUMBER" "SHIPPER_ID_NUMBER" "SHIPPER_ID1_NUMBER" "SHIPPER_ID2_NUMBER"
		/// </summary>
		public string printedReferenceType { get; set; }
		public string value { get; set; }
	}

	public class FedexExpressFreightDetails
	{
		/// <summary>
		/// An advance booking number is optional for FedEx 1Day Freight. When you call 1.800.332.0807 to book your freight shipment, you will receive a booking number. This booking number is included in the Ship request, and prints on the shipping label
		/// </summary>
		public string bookingConfirmationNumber { get; set; }
		/// <summary>
		/// Describes the shippers loaded total package counts.
		/// </summary>
		public int shippersLoadAndCount { get; set; }
	}

	public class FedexSmartPostInfoDetails
	{
		/// <summary>
		/// "ADDRESS_CORRECTION" "CARRIER_LEAVE_IF_NO_RESPONSE" "CHANGE_SERVICE" "FORWARDING_SERVICE" "RETURN_SERVICE". Indicate the type of ancillary endorsement.Is required for Presorted Standard but not for returns or parcel select. Note not all are usable for all ancillary endorsements.
		/// </summary>
		public string ancillaryEndorsement { get; set; }
		/// <summary>
		/// Specify the four-digit numeric Hub ID value used during rate quote for smartport shipments.
		/// </summary>
		public string hubId { get; set; }
		/// <summary>
		/// "MEDIA_MAIL" "PARCEL_RETURN" "PARCEL_SELECT" "PRESORTED_BOUND_PRINTED_MATTER" "PRESORTED_STANDARD"
		/// </summary>
		public string indicia { get; set; }
		/// <summary>
		/// "USPS_DELIVERY_CONFIRMATION". Specify the special handling associated with Smartpost Shipment.
		/// </summary>
		public string specialServices { get; set; }
	}

	public class FedexServiceTypeDetails
	{
		/// <summary>
		/// "FDXE" "FDXG" "FXSP" "FXFR" "FDXC" "FXCC"
		/// </summary>
		public string carrierCode { get; set; }
		public string description { get; set; }
		public string serviceName { get; set; }
		public string serviceCategory { get; set; }
	}
}
