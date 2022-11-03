using OrderCloud.SDK;
using System.Collections.Generic;

namespace OrderCloud.Catalyst
{
	public class OrderReturnIEPayload
	{
		public OrderReturn OrderReturn {get; set; }
		public OrderWorksheet OrderWorkSheet { get; set; } 
	}

	public class OrderReturnIEPayload<TOrderReturn, TOrderWorksheet> : OrderReturnIEPayload
		where TOrderReturn : OrderReturn
		where TOrderWorksheet : OrderWorksheet
	{
		public new OrderReturn OrderReturn { get; set; }
		public new OrderWorksheet OrderWorkSheet { get; set; }
	}

	public class OrderReturnResponse
	{
		public decimal RefundAmount { get; set; }
		public List<LineItemReturnCalculation> ItemsToReturnCalcs { get; set; } = new List<LineItemReturnCalculation>();
	}

	public class LineItemReturnCalculation
	{
		public string LineItemID { get; set; }
		public decimal RefundAmount { get; set; }
	}
}
