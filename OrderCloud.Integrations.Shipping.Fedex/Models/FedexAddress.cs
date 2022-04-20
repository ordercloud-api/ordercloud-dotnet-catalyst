using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexAddress
	{
		/// <summary>
		/// This is a placeholder for City Name. Example: Beverly Hills
		/// </summary>
		public string city { get; set; }
		/// <summary>
		/// This is a placeholder for State or Province code. Example: CA
		/// </summary>
		public string stateOrProvinceCode { get; set; }
		/// <summary>
		/// Indicate the Postal code. This is optional for non postal-aware countries. Maximum length is 10. Example: 65247
		/// </summary>
		public string postalCode { get; set; }
		/// <summary>
		/// This is the two-letter country code. Maximum length is 2. Example: US
		/// </summary>
		public string countryCode { get; set; }
		/// <summary>
		/// Indicate whether this address is residential (as opposed to commercial).
		/// </summary>
		public bool residential { get; set; }
	}
}
