using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.UPS
{
	public static class UPSRatesMapper
    {
        public static List<ShipMethod> ToOrderCloudShipMethods(this UPSRestResponseBody upsRatesResponse)
        {
            var shipMethods = new List<ShipMethod>();
            foreach (var rate in upsRatesResponse.RateResponse.RatedShipment)
			{
                if (rate?.GuaranteedDelivery?.BusinessDaysInTransit != null)
                {
                    shipMethods.Add(ToOrderCloudShipMethod(rate));
                }
			}
            return shipMethods;
        }

        public static ShipMethod ToOrderCloudShipMethod(this UPSRatedShipment rate)
		{
            var shipMethod = new ShipMethod
            {
                ID = rate.Service.Code,
                Cost = decimal.Parse(rate.TotalCharges.MonetaryValue),
                EstimatedTransitDays = int.Parse(rate.GuaranteedDelivery.BusinessDaysInTransit),
                Name = ToUPSServiceName(rate.Service.Code),
                xp = new
                {
                    Carrier = "UPS",
                }
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
