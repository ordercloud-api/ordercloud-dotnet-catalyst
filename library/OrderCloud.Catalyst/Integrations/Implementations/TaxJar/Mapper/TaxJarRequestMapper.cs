using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{ 
	public static class TaxJarRequestMapper
	{
		public const string ShippingLineCode = "shipping_code";
		private static string CreateLineTransactionID(string orderID, string lineItemID) => $"OrderID:|{orderID}|LineItemsID:|{lineItemID}";
		private static string CreateShipTransactionID(string orderID, string shipEstimateID) => $"OrderID:|{orderID}|ShippingEstimateID:|{shipEstimateID}";

		/// <summary>
		///	Returns a list of TaxJarOrders because each order has a single to and from address. They coorespond to OrderCloud LineItems. 
		/// </summary>
		public static List<TaxJarOrder> ToOrders(OrderWorksheet order)
		{
			var itemLines = order.LineItems.Select(li => ToTaxJarLineOrder(li, order.Order.ID));
			var shippingLines = order.ShipEstimateResponse.ShipEstimates.Select(se =>
			{
				var firstLineItem = order.GetShipEstimateLineItems(se.ID).First().LineItem;
				return ToTaxJarShipOrder(se, firstLineItem, order.Order.ID);
			});
			return itemLines.Concat(shippingLines).ToList();
		}

		private  static TaxJarOrder ToTaxJarShipOrder(ShipEstimate shipEstimate, LineItem lineItem, string orderID)
		{
			var selectedShipMethod = shipEstimate.GetSelectedShipMethod();
			return new TaxJarOrder()
			{
				transaction_id = CreateShipTransactionID(orderID, shipEstimate.ID),
				shipping = 0, // will create separate lines for shipping

				from_city = lineItem.ShipFromAddress.City,
				from_zip = lineItem.ShipFromAddress.Zip,
				from_state = lineItem.ShipFromAddress.State,
				from_country = lineItem.ShipFromAddress.Country,
				from_street = lineItem.ShipFromAddress.Street1,

				to_city = lineItem.ShippingAddress.City,
				to_zip = lineItem.ShippingAddress.Zip,
				to_state = lineItem.ShippingAddress.State,
				to_country = lineItem.ShippingAddress.Country,
				to_street = lineItem.ShippingAddress.Street1,

				line_items = new List<TaxJarLineItem> {
					new TaxJarLineItem()
					{
						id = shipEstimate.ID,
						quantity = 1,
						unit_price = selectedShipMethod.Cost,
						description = selectedShipMethod.Name,
						product_identifier = ShippingLineCode,
					}
				}
			};
		}

		private static TaxJarOrder ToTaxJarLineOrder(LineItem lineItem, string orderID)
		{
			return new TaxJarOrder()
			{
				transaction_id = CreateLineTransactionID(orderID, lineItem.ID),
				shipping = 0, // will create separate lines for shipping

				from_city = lineItem.ShipFromAddress.City,
				from_zip = lineItem.ShipFromAddress.Zip,
				from_state = lineItem.ShipFromAddress.State,
				from_country = lineItem.ShipFromAddress.Country,
				from_street = lineItem.ShipFromAddress.Street1,

				to_city = lineItem.ShippingAddress.City,
				to_zip = lineItem.ShippingAddress.Zip,
				to_state = lineItem.ShippingAddress.State,
				to_country = lineItem.ShippingAddress.Country,
				to_street = lineItem.ShippingAddress.Street1,

				line_items = new List<TaxJarLineItem> {
					new TaxJarLineItem()
					{
						id = lineItem.ID,
						quantity = lineItem.Quantity,
						unit_price = lineItem.UnitPrice ?? 0,
						description = lineItem.Product.Name,
						product_identifier = lineItem.Product.ID,
					}
				}
			};
		}
	}
}
