﻿using OrderCloud.Catalyst;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Shipping.EasyPost
{
	public class EasyPostService : OCIntegrationService, IShippingRatesCalculator
	{ 
		public EasyPostService(EasyPostConfig defaultConfig) : base(defaultConfig) { }

		public async Task<List<List<ShippingRate>>> CalculateShippingRatesAsync(IEnumerable<ShippingPackage> shippingPackages, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<EasyPostConfig>(overrideConfig ?? _defaultConfig);
			var easyPostShipments = shippingPackages.Select(p => EasyPostPackageMapper.ToEasyPostShipment(p, config.CarrierAccountIDs));
			var responses = await Throttler.RunAsync(easyPostShipments, 100, 6, ship => EasyPostClient.PostShipmentAsync(ship, config));
			var shipMethods = responses.Select(EasyPostRateMapper.ToOrderCloudShipMethods).ToList();
			return shipMethods;
		}
	}
}
