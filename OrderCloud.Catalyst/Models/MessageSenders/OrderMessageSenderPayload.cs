using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Used for message sender types OrderSubmitted, OrderSubmittedForApproval, OrderApproved, OrderDeclined, OrderSubmittedForYourApproval, OrderSubmittedForYourApprovalHasBeenApproved, and OrderSubmittedForYourApprovalHasBeenDeclined.
	/// </summary>
	public class OrderMessageSenderPayload : MessageSenderPayload
	{
		public OrderMessageSenderEventBody EventBody { get; set; }
	}

	/// <summary>
	/// Used for message sender types OrderSubmitted, OrderSubmittedForApproval, OrderApproved, OrderDeclined, OrderSubmittedForYourApproval, OrderSubmittedForYourApprovalHasBeenApproved, and OrderSubmittedForYourApprovalHasBeenDeclined.
	/// </summary>
	public class OrderMessageSenderEventBody
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
	}


	/// <summary>
	/// Used for message sender types OrderSubmitted, OrderSubmittedForApproval, OrderApproved, OrderDeclined, OrderSubmittedForYourApproval, OrderSubmittedForYourApprovalHasBeenApproved, and OrderSubmittedForYourApprovalHasBeenDeclined.
	/// </summary>
	public class OrderMessageSenderPayload<TMessageSenderXp, TUser, TOrder, TOrderApproval, TLineItem, TProduct> : MessageSenderPayload<TMessageSenderXp, TUser>
		where TUser: User
		where TOrder : Order
		where TOrderApproval : OrderApproval
		where TLineItem : LineItem
		where TProduct : Product
	{
		public OrderMessageSenderEventBody<TOrder, TOrderApproval, TLineItem, TProduct> EventBody { get; set; }
	}

	/// <summary>
	/// Used for message sender types OrderSubmitted, OrderSubmittedForApproval, OrderApproved, OrderDeclined, OrderSubmittedForYourApproval, OrderSubmittedForYourApprovalHasBeenApproved, and OrderSubmittedForYourApprovalHasBeenDeclined.
	/// </summary>
	public class OrderMessageSenderEventBody<TOrder, TOrderApproval, TLineItem, TProduct> : OrderMessageSenderEventBody
		where TOrder : Order
		where TOrderApproval : OrderApproval
		where TLineItem : LineItem
		where TProduct : Product
	{
		/// <summary>
		/// The order that was submitted
		/// </summary>
		public new TOrder Order { get; set; }
		/// <summary>
		/// The array of order approvals for the order
		/// </summary>
		public new List<TOrderApproval> Approvals { get; set;}
		/// <summary>
		/// The array of line items for the orders
		/// </summary>
		public new List<TLineItem> LineItems { get; set; }
		/// <summary>
		/// The array of products for the order
		/// </summary>
		public new List<TProduct> Products { get; set; }
	}

}
