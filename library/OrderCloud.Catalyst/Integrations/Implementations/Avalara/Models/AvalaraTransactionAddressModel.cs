using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OrderCloud.Catalyst
{
	public class AvalaraTransactionAddressModel
	{
		public long? id { get; set; }
		public long? transactionId { get; set; }
		public AvalaraBoundaryLevel? boundaryLevel { get; set; }
		public string line1 { get; set; }
		public string line2 { get; set; }
		public string line3 { get; set; }
		public string city { get; set; }
		public string region { get; set; }
		public string postalCode { get; set; }
		public string country { get; set; }
		public int? taxRegionId { get; set; }
		public string latitude { get; set; }
		public string longitude { get; set; }
	}

	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum AvalaraBoundaryLevel
	{
		Address = 0,
		Zip9 = 1,
		Zip5 = 2
	}
}
