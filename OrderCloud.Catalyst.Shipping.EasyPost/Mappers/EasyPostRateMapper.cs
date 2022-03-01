using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	public static class EasyPostRateMapper
	{
		public static List<ShipMethod> ToOrderCloudShipMethods(EasyPostShipment shipment)
		{
			if (shipment?.rates == null) // if no rates are returned
			{
				return new List<ShipMethod> { };
			}
			var rates = shipment.rates.Select(ToOrderCloudShipMethods).ToList();
			return rates;
		}

		public static ShipMethod ToOrderCloudShipMethods(EasyPostRate rate)
		{
			var shipMethod = new ShipMethod()
			{
				ID = rate.id,
				Name = rate.service,
				Cost = decimal.Parse(rate.rate),
				EstimatedTransitDays = rate.delivery_days ?? 0,
				xp = new
				{
					Carrier = rate.carrier,
				}
			};
			return shipMethod;
		}
	}
}
