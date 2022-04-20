using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	public static class EasyPostRateMapper
	{
		public static List<ShippingRate> ToOrderCloudShipMethods(EasyPostShipment shipment)
		{
			if (shipment?.rates == null) // if no rates are returned
			{
				return new List<ShippingRate> { };
			}
			var rates = shipment.rates.Select(ToOrderCloudShipMethods).ToList();
			return rates;
		}

		public static ShippingRate ToOrderCloudShipMethods(EasyPostRate rate)
		{
			var shipMethod = new ShippingRate()
			{
				ID = rate.id,
				Name = rate.service,
				Cost = decimal.Parse(rate.rate),
				EstimatedTransitDays = rate.delivery_days ?? 0,
				Carrier = rate.carrier,
			};
			return shipMethod;
		}
	}
}