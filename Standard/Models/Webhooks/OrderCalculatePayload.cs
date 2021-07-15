using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class OrderCalculatePayload<TConfig>
	{
		public OrderWorksheet OrderWorksheet { get; set; }
		public string Environment { get; set; }
		public string OrderCloudAccessToken { get; set; }
		public TConfig ConfigData { get; set; }
	}

	public class OrderCalculatePayload<TConfig, TOrder, TLineItem, TShipEstimateResponse, TOrderCalculateResponse, TOrderSubmitResponse, TOrderSubmitForApprovalResponse, TOrderApprovedResponse>
		where TOrder: Order
		where TLineItem: LineItem
		where TShipEstimateResponse: ShipEstimateResponse
		where TOrderCalculateResponse: OrderCalculateResponse
		where TOrderSubmitResponse: OrderSubmitResponse
		where TOrderSubmitForApprovalResponse: OrderSubmitForApprovalResponse
		where TOrderApprovedResponse: OrderApprovedResponse
	{
		public OrderWorksheet<TOrder, TLineItem, TShipEstimateResponse, TOrderCalculateResponse, TOrderSubmitResponse, TOrderSubmitForApprovalResponse, TOrderApprovedResponse> OrderWorksheet { get; set; }
		public string Environment { get; set; }
		public string OrderCloudAccessToken { get; set; }
		public TConfig ConfigData { get; set; }
	}
}
