using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Shipping.UPS
{
	public class UPSCommand : OCIntegrationCommand, IShipMethodCalculator
	{
		public UPSCommand(UPSConfig defaultConfig) : base(defaultConfig) { }

		public async Task<List<List<ShipMethod>>> CalculateShipMethodsAsync(IEnumerable<ShipPackage> shippingPackages, OCIntegrationConfig overrideConfig = null)
		{
			var config = ValidateConfig<UPSConfig>(overrideConfig ?? _defaultConfig);
			var rateRequests = shippingPackages.Select(UPSPackageMapper.ToRateRequest);
			var responses = await Throttler.RunAsync(rateRequests, 100, 6, ship => UPSClient.ShopRates(ship, config));
			var shipMethods = responses.Select(UPSRatesMapper.ToOrderCloudShipMethods).ToList();
			return shipMethods;
		}
	}
}
