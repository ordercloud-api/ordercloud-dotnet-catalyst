using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Shipping.Fedex
{
	// See https://developer.fedex.com/api/en-us/catalog/rate/v1/docs.html
	public class FedexRateRequestBody
	{
		/// <summary>
		/// This is the Account number details.
		/// </summary>
		public FedexAccountNumber accountNumber { get; set; } = new FedexAccountNumber();
		/// <summary>
		/// Specify the return transit times, services needed on rate failure, choice of variable option and order to sort rate options to filter and sort the expected response.
		/// </summary>
		public FedexRateRequestControlParameters rateRequestControlParameters { get; set; } = new FedexRateRequestControlParameters();
		/// <summary>
		/// Specify the four letter code of a FedEx operating company that meets your requirements.
		/// </summary>
		public List<string> carrierCodes { get; set; } = new List<string>();
		/// <summary>
		/// This is shipment data for which a rate quote (or rate-shipping comparison) is requested.
		/// </summary>
		public FedexRequestedShipment requestedShipment { get; set; } = new FedexRequestedShipment();
	}

	public class FedexAccountNumber
	{
		/// <summary>
		/// This is the account number. Maximum Length is 9.
		/// </summary>
		public string value { get; set; }
	}

	public class FedexRateRequestControlParameters
	{
		/// <summary>
		/// Indicate if the transit time and commit data are to be returned in the reply. Default value is false.
		/// </summary>
		public bool returnTransitTimes { get; set; }
		/// <summary>
		/// Specify the services to be requested if the rate data is not available.
		/// </summary>
		public bool servicesNeededOnRateFailure { get; set; }
		/// <summary>
		/// Specify service options whose combinations are to be considered when replying with available services.
		/// </summary>
		public string variableOptions { get; set; }
		/// <summary>
		/// This is a sort order you can specify to control the order of the response data:
		/// </summary>
		public string rateSortOrder { get; set; }
	}
}
