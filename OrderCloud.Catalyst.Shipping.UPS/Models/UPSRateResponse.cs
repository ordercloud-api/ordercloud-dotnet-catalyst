using System.Collections.Generic;

namespace OrderCloud.Catalyst.Shipping.UPS
{
    public class UPSRestResponseBody
    {
        public UPSRateResponse RateResponse { get; set; }
    }

    public class UPsResponseMeta
    {
        public UPSCodeDescription ResponseStatus { get; set; }
        public List<UPSCodeDescription> Alert { get; set; }
        public UPSTransactionReference TransactionReference { get; set; }
    }

    public class UPSRateResponse
    {
        public UPsResponseMeta Response { get; set; }
        public List<UPSRatedShipment> RatedShipment { get; set; }
    }

    public class UPSChargeDetail
    {
        public string CurrencyCode { get; set; }
        public string MonetaryValue { get; set; }
        public string Code { get; set; }
        public string SubType { get; set; }

    }

    public class UPSRatedShipment
    {
        public UPSCodeDescription Service { get; set; }
        public List<UPSCodeDescription> RatedShipmentAlert { get; set; }
        public UPSWeightMeasurement BillingWeight { get; set; }
        public UPSChargeDetail TransportationCharges { get; set; }
        public UPSChargeDetail BaseServiceCharge { get; set; }
        public UPSChargeDetail ServiceOptionsCharges { get; set; }
        public UPSChargeDetail TotalCharges { get; set; }
        public UPSRatedPackage RatedPackage { get; set; }
        public UPSGuaranteedDelivery GuaranteedDelivery { get; set; }
    }

    public class UPSGuaranteedDelivery
	{
        public string BusinessDaysInTransit { get; set; }
    }

    public class UPSRatedPackage
    {
        public UPSChargeDetail TransportationCharges { get; set; }
        public UPSChargeDetail BaseServiceCharge { get; set; }
        public UPSChargeDetail ServiceOptionsCharges { get; set; }
        public UPSChargeDetail[] ItemizedCharges { get; set; }
        public UPSChargeDetail TotalCharges { get; set; }
        public string Weight { get; set; }
        public UPSWeightMeasurement BillingWeight { get; set; }
    }

    public enum UPSServiceCodes
    {
        //Domestic
        UPS_Next_Day_Air_Early = 14,
        UPS_Next_Day_Air = 1,
        UPS_Next_Day_Air_Saver = 13,
        UPS_2nd_Day_Air_AM = 59,
        UPS_2nd_Day_Air = 2,
        UPS_3_Day_Select = 12,
        UPS_Ground = 03,
        //International	
        UPS_Standard = 11,
        UPS_Worldwide_Express = 7,
        UPS_Worldwide_Express_Plus = 54,
        UPS_Worldwide_Expedited = 8,
        UPS_Worldwide_Saver = 65,
        UPS_Worldwide_Express_Freight = 96,
        UPS_Today_Standard = 82,
        UPS_Today_Dedicated_Courier = 83,
        UPS_Today_Intercity = 84,
        UPS_Today_Express = 85,
        UPS_Today_Express_Saver = 86,
        UPS_Access_Point_Economy = 70
    }
}
