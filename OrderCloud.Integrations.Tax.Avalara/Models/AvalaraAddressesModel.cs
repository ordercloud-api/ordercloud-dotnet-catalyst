using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tax.Avalara
{
	public class AvalaraAddressesModel
	{
		public AvalaraAddressLocationInfo singleLocation { get; set; }
		public AvalaraAddressLocationInfo shipFrom { get; set; }
		public AvalaraAddressLocationInfo shipTo { get; set; }
		public AvalaraAddressLocationInfo pointOfOrderOrigin { get; set; }
		public AvalaraAddressLocationInfo pointOfOrderAcceptance { get; set; }
	}

	public class AvalaraAddressLocationInfo
	{
		public string locationCode { get; set; }
		public string line1 { get; set; }
		public string line2 { get; set; }
		public string line3 { get; set; }
		public string city { get; set; }
		public string region { get; set; }
		public string country { get; set; }
		public string postalCode { get; set; }
		public decimal? latitude { get; set; }
		public decimal? longitude { get; set; }
	}
}
