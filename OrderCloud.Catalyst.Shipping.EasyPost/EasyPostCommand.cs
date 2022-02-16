using OrderCloud.SDK;
using System;
using System.Collections.Generic;
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
		}
	}
}
