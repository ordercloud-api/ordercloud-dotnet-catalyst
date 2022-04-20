using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Shipping.UPS
{
	public static class UPSRatesMapper
    {
        public static List<ShippingRate> ToOrderCloudShipMethods(UPSRestResponseBody upsRatesResponse)
        {
            var shipMethods = new List<ShippingRate>();
            foreach (var rate in upsRatesResponse.RateResponse.RatedShipment)
			{
                if (rate?.GuaranteedDelivery?.BusinessDaysInTransit != null)
                {
                    shipMethods.Add(ToOrderCloudShipMethod(rate));
                }
			}
            return shipMethods;
        }

        public static ShippingRate ToOrderCloudShipMethod(UPSRatedShipment rate)
		{
            var shipMethod = new ShippingRate
            {
                ID = rate.Service.Code,
                Cost = decimal.Parse(rate.TotalCharges.MonetaryValue),
                EstimatedTransitDays = int.Parse(rate.GuaranteedDelivery.BusinessDaysInTransit),
                Name = ToUPSServiceName(rate.Service.Code),
                Carrier = "UPS"
            };
            return shipMethod;
        }


        public static string ToUPSServiceName(string serviceCode)
        {
            var isValidServiceCode = Enum.TryParse(serviceCode, out UPSServiceCodes upsService);
            if (!isValidServiceCode) return "Invald UPS service Type";
            return upsService.ToString().Replace("_", " ");
        }
    }
}
