
namespace OrderCloud.Catalyst.Shipping.UPS
{
    public class UPSRestRequestBody
    {
        public UPSRateRequest RateRequest { get; set; }
    }

    public class UPSRateRequest
    {
        public UPSRequestParameters Request { get; set; }
        public UPSShipment Shipment { get; set; }
    }

    public class UPSRequestParameters
    {
        public string SubVersion { get; set; }
        public UPSTransactionReference TransactionReference { get; set; }
    }

    public class UPSTransactionReference
    {
        public string CustomerContext { get; set; }
    }

    public class UPSShipment
    {
        public UPSShipmentRatingOptions ShipmentRatingOptions { get; set; }
        //Shipper container. Information associated with the UPS account number.
        public UPSAddress Shipper { get; set; }
        public UPSAddress ShipTo { get; set; }
        public UPSAddress ShipFrom { get; set; }
        //Only valid with RequestOption = Rate for both Small package and GFP Rating requests
        public UPSCodeDescription Service { get; set; }
        public UPSWeightMeasurement ShipmentTotalWeight { get; set; }
        public UPSPackage Package { get; set; }
    }

    public class UPSPackage
    {
        //The code for the UPS packaging type associated with the package.
        //Valid values: 00 = UNKNOWN 01 = UPS Letter 02 =
        //Package 03 = Tube 04 = Pak 21 = Express Box 24 =
        //25KG Box 25 = 10KG Box 30 = Pallet 2a = Small Express
        //Box 2b = Medium Express Box 2c = Large Express Box.
        //For FRS rating requests the only valid value is customer
        //supplied packaging “02”
        public UPSCodeDescription PackagingType { get; set; }
        public UPSDimensions Dimensions { get; set; }
        public UPSWeightMeasurement PackageWeight { get; set; }
    }

    public class UPSDimensions
    {
        public UPSCodeDescription UnitOfMeasurement { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }

    public class UPSCodeDescription
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class UPSWeightMeasurement
    {
        public UPSCodeDescription UnitOfMeasurement { get; set; }
        public string Weight { get; set; }
    }

    public class UPSAddress
    {
        public string Name { get; set; }
        //User level promotion is for the customers who do not have a UPS shipper account. The following conditions need to
        //be met to use user level promotions: UserLevelDiscountIndicator in request, do not pass shipper number, Username
        //should be present and user should be eligible for a user level promotion.
        //If User level promotion is requested with a UPS shipper account (Shipper/ShipperNumber), User level promotion
        //will not be requested for a given shipment.
        public string ShipperNumber { get; set; }
        public UPSAddressDetails Address { get; set; }
    }

    public class UPSAddressDetails
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class UPSShipmentRatingOptions
    {
        public string UserLevelDiscountIndicator { get; set; }
    }
}
