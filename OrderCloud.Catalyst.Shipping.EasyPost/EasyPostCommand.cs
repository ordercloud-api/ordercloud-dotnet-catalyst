using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	public class EasyPostCommand : OCIntegrationCommand, IShipMethodCalculator
	{ 
		public EasyPostCommand(EasyPostConfig defaultConfig) : base(defaultConfig) { }

		public async Task<List<List<ShipMethod>>> CalculateShipMethodsAsync(IEnumerable<ShipPackage> shippingPackages, OCIntegrationConfig configOverride = null)
		{
			var config = GetValidatedConfig<EasyPostConfig>(configOverride);
			var easyPostShipments = shippingPackages.Select(p => EasyPostPackageMapper.ToEasyPostShipment(p, config.CarrierAccountIDs));
			var responses = await Throttler.RunAsync(easyPostShipments, 100, 6, ship => EasyPostClient.PostShipmentAsync(ship, config));
			var shipMethods = responses.Select(EasyPostRateMapper.ToOrderCloudShipMethods).ToList();
			return shipMethods;
		}
	}
}
