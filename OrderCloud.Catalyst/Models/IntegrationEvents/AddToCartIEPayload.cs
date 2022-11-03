using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class AddToCartIEPayload
	{
		public string ProductID { get; set; }
		public int Quantity { get; set; }
		public string BuyerID { get; set; }
		public User BuyerUser { get; set; }
		public string SellerID { get; set; }
		public string Environment { get; set; }
		public string OrderCloudAccessToken { get; set; }
		public dynamic ConfigData { get; set; }
	}

	public class AddToCartIEPayload<TConfigData, TBuyerUser> : AddToCartIEPayload
		where TBuyerUser: User
	{
		public new TBuyerUser BuyerUser { get; set; }
		public new TConfigData ConfigData { get; set; }
	}

	public class AddToCartResponse
	{
		public AdHocProduct Product { get; set; }
		public decimal UnitPrice { get; set; }
	}

	public class AddToCartResponse<TAdHocProduct> : AddToCartResponse
		where TAdHocProduct : AdHocProduct
	{
		public new TAdHocProduct Product { get; set; }
	}
}
