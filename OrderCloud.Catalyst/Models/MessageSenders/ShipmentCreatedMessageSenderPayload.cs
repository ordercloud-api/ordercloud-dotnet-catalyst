using OrderCloud.SDK;
using System.Collections.Generic;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Used for the ShipmentCreated message sender type
	/// </summary>
	public class ShipmentCreatedMessageSenderPayload : MessageSenderPayload
	{
		public ShipmentCreatedMessageSenderEventBody EventBody { get; set; }
	}

	public class ShipmentCreatedMessageSenderEventBody
	{
		/// <summary>
		/// The order that was submitted
		/// </summary>
		public Order Order { get; set; }
		/// <summary>
		/// The array of order approvals for the order
		/// </summary>
		public List<OrderApproval> Approvals { get; set; }
		/// <summary>
		/// The array of line items for the orders
		/// </summary>
		public List<LineItem> LineItems { get; set; }
		/// <summary>
		/// The array of products for the order
		/// </summary>
		public List<Product> Products { get; set; }
		/// <summary>
		/// The shipment for the order
		/// </summary>
		public Shipment Shipment { get; set; }
		/// <summary>
		/// The array of shipment items for the shipment
		/// </summary>
		public List<ShipmentItem> ShipmentItems { get; set; }
	}

	/// <summary>
	/// Used for the ShipmentCreated message sender type
	/// </summary>
	public class ShipmentCreatedMessageSenderPayload<TMessageSenderXp, TUser, TOrder, TOrderApproval, TLineItem, TProduct, TShipment, TShipmentItem> : MessageSenderPayload<TMessageSenderXp, TUser>
		where TUser : User
		where TOrder : Order
		where TOrderApproval : OrderApproval
		where TLineItem : LineItem
		where TProduct : Product
		where TShipment : Shipment
		where TShipmentItem : ShipmentItem
	{
		public ShipmentCreatedMessageSenderEventBody<TOrder, TOrderApproval, TLineItem, TProduct, TShipment, TShipmentItem> EventBody { get; set; }
	}

	public class ShipmentCreatedMessageSenderEventBody<TOrder, TOrderApproval, TLineItem, TProduct, TShipment, TShipmentItem> : ShipmentCreatedMessageSenderEventBody
		where TOrder : Order
		where TOrderApproval : OrderApproval
		where TLineItem : LineItem
		where TProduct : Product
		where TShipment : Shipment
		where TShipmentItem : ShipmentItem
	{
		/// <summary>
		/// The order that was submitted
		/// </summary>
		public new TOrder Order { get; set; }
		/// <summary>
		/// The array of order approvals for the order
		/// </summary>
		public new List<TOrderApproval> Approvals { get; set; }
		/// <summary>
		/// The array of line items for the orders
		/// </summary>
		public new List<TLineItem> LineItems { get; set; }
		/// <summary>
		/// The array of products for the order
		/// </summary>
		public new List<TProduct> Products { get; set; }
		/// <summary>
		/// The shipment for the order
		/// </summary>
		public new TShipment Shipment { get; set; }
		/// <summary>
		/// The array of shipment items for the shipment
		/// </summary>
		public new List<TShipmentItem> ShipmentItems { get; set; }
	}
}
