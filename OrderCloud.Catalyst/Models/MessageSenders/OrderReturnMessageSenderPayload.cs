using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Used for message sender types OrderReturnSubmitted, OrderReturnSubmittedForApproval, OrderReturnApproved, OrderReturnDeclined, OrderReturnSubmittedForYourApproval, OrderReturnSubmittedForYourApprovalHasBeenApproved, OrderReturnSubmittedForYourApprovalHasBeenDeclined, OrderReturnCompleted 
	/// </summary>
	public class OrderReturnMessageSenderPayload : MessageSenderPayload
	{
		public OrderReturnMessageSenderEventBody EventBody { get; set; }
	}

	/// <summary>
	/// Used for message sender types OrderReturnSubmitted, OrderReturnSubmittedForApproval, OrderReturnApproved, OrderReturnDeclined, OrderReturnSubmittedForYourApproval, OrderReturnSubmittedForYourApprovalHasBeenApproved, OrderReturnSubmittedForYourApprovalHasBeenDeclined, OrderReturnCompleted 
	/// </summary>
	public class OrderReturnMessageSenderEventBody
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
		/// The order return that was submitted
		/// </summary>
		public OrderReturn OrderReturn { get; set; }
	}


	/// <summary>
	/// Used for message sender types OrderReturnSubmitted, OrderReturnSubmittedForApproval, OrderReturnApproved, OrderReturnDeclined, OrderReturnSubmittedForYourApproval, OrderReturnSubmittedForYourApprovalHasBeenApproved, OrderReturnSubmittedForYourApprovalHasBeenDeclined, OrderReturnCompleted 
	/// </summary>
	public class OrderReturnMessageSenderPayload<TMessageSenderXp, TUser, TOrder, TOrderApproval, TLineItem, TProduct, TOrderReturn> : MessageSenderPayload<TMessageSenderXp, TUser>
		where TUser : User
		where TOrder : Order
		where TOrderApproval : OrderApproval
		where TLineItem : LineItem
		where TProduct : Product
		where TOrderReturn : OrderReturn
	{
		public OrderReturnMessageSenderEventBody<TOrder, TOrderApproval, TLineItem, TProduct, TOrderReturn> EventBody { get; set; }
	}

	/// <summary>
	/// Used for message sender types OrderReturnSubmitted, OrderReturnSubmittedForApproval, OrderReturnApproved, OrderReturnDeclined, OrderReturnSubmittedForYourApproval, OrderReturnSubmittedForYourApprovalHasBeenApproved, OrderReturnSubmittedForYourApprovalHasBeenDeclined, OrderReturnCompleted 
	/// </summary>
	public class OrderReturnMessageSenderEventBody<TOrder, TOrderApproval, TLineItem, TProduct, TOrderReturn> : OrderReturnMessageSenderEventBody
		where TOrder : Order
		where TOrderApproval : OrderApproval
		where TLineItem : LineItem
		where TProduct : Product
		where TOrderReturn : OrderReturn
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
		/// The order return that was submitted
		/// </summary>
		public new TOrderReturn OrderReturn { get; set; }
	}

}
