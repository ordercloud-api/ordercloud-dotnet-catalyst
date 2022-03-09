using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexRatesMapper
	{
		public static List<ShipMethod> ToOrderCloudShipMethods(FedexRateResponse fedexRatesResponse)
		{
			return fedexRatesResponse.output.rateReplyDetails.Select(ToOrderCloudShipMethods).ToList();
		}

		public static ShipMethod ToOrderCloudShipMethods(FedexRateReplyDetails rate)
		{
			var shipMethod = new ShipMethod()
			{
				ID = rate.serviceDescription.serviceId,
				Name = rate.serviceName,
				Cost = rate.ratedShipmentDetails.FirstOrDefault().totalNetCharge,
				EstimatedTransitDays = GetDaysFromNow(DateTime.Parse(rate.operationalDetail.deliveryDate)),
				xp = new
				{
					Carrier = "Fedex"
				}
			};
			return shipMethod;
		}

		public static int GetDaysFromNow(DateTime deliveryDate)
		{
			var span = deliveryDate - DateTime.Now;
			return Math.Max(1, span.Days); // set a lower boundary of 1 for transit days
		}
	}
}
