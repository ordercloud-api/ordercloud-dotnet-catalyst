using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class OrderCheckoutIEPayload
	{
		public OrderWorksheet OrderWorksheet { get; set; }
		public string Environment { get; set; }
		public string OrderCloudAccessToken { get; set; }
		public dynamic ConfigData { get; set; }
	}

	public class OrderCheckoutIEPayload<TConfigData, TOrderWorksheet> : OrderCheckoutIEPayload
		where TOrderWorksheet : OrderWorksheet
	{
		public new TOrderWorksheet OrderWorksheet { get; set; }
		public new TConfigData ConfigData { get; set; }
	}
}
