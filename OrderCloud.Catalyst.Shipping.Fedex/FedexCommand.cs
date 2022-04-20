using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexCommand : OCIntegrationCommand, IShippingRatesCalculator
	{
		protected readonly FedexClient _client;

		public FedexCommand(FedexConfig defaultConfig) : base(defaultConfig) 
		{
			_client = new FedexClient(defaultConfig);
		}

		public async Task<List<List<ShippingRate>>> CalculateShippingRatesAsync(IEnumerable<ShippingPackage> shippingPackages, OCIntegrationConfig overrideConfig = null)
		{
			// validating the config override. We don't need the result though b/c the logic of which config to use is in FedexClient
			var validatedOverrideConfig = ValidateConfig<FedexConfig>(overrideConfig);
			var accountNumber = (validatedOverrideConfig ?? (_defaultConfig as FedexConfig)).AccountNumber;
			var rateRequests = shippingPackages.Select(p => FedexPackageMapper.ToRateRequest(p, accountNumber));
			var responses = await Throttler.RunAsync(rateRequests, 100, 6, rateRequest => _client.GetRates(rateRequest, validatedOverrideConfig));
			var shipMethods = responses.Select(FedexRatesMapper.ToOrderCloudShipMethods).ToList();
			return shipMethods;
		}
	}
}
