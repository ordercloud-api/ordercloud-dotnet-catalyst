using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Tax.TaxJar
{ 
	public static class TaxJarRequestMapper
	{
		public const string ShippingLineCode = "shipping_code";
		private static string CreateLineTransactionID(string orderID, string lineItemID) => $"OrderID:|{orderID}|LineItemsID:|{lineItemID}";
		private static string CreateShipTransactionID(string orderID, string shipEstimateID) => $"OrderID:|{orderID}|ShippingEstimateID:|{shipEstimateID}";

		/// <summary>
		///	Returns a list of TaxJarOrders because each order has a single to and from address. They coorespond to OrderCloud LineItems. 
		/// </summary>
		public static List<TaxJarOrder> ToOrders(OrderSummaryForTax order)
		{
			var itemLines = order.LineItems.Select(li => ToTaxJarLineOrder(li, order.OrderID));
			var shippingLines = order.ShippingCosts.Select(se => ToTaxJarShipOrder(se, order.OrderID));
			return itemLines.Concat(shippingLines).ToList();
		}

		private  static TaxJarOrder ToTaxJarShipOrder(ShipEstimateSummaryForTax shipEstimate, string orderID)
		{
			return new TaxJarOrder()
			{
				transaction_id = CreateShipTransactionID(orderID, shipEstimate.ShipEstimateID),
				shipping = 0, // will create separate lines for shipping

				from_city = shipEstimate.ShipFrom.Zip,
				from_state = shipEstimate.ShipFrom.State,
				from_country = shipEstimate.ShipFrom.Country,
				from_street = shipEstimate.ShipFrom.Street1,

				to_city = shipEstimate.ShipTo.City,
				to_zip = shipEstimate.ShipTo.Zip,
				to_state = shipEstimate.ShipTo.State,
				to_country = shipEstimate.ShipTo.Country,
				to_street = shipEstimate.ShipTo.Street1,

				line_items = new List<TaxJarLineItem> {
					new TaxJarLineItem()
					{
						id = shipEstimate.ShipEstimateID,
						quantity = 1,
						unit_price = shipEstimate.Cost,
						description = shipEstimate.Description,
						product_identifier = ShippingLineCode,
					}
				}
			};
		}

		private static TaxJarOrder ToTaxJarLineOrder(LineItemSummaryForTax lineItem, string orderID)
		{
			return new TaxJarOrder()
			{
				transaction_id = CreateLineTransactionID(orderID, lineItem.LineItemID),
				shipping = 0, // will create separate lines for shipping

				from_city = lineItem.ShipFrom.Zip,
				from_state = lineItem.ShipFrom.State,
				from_country = lineItem.ShipFrom.Country,
				from_street = lineItem.ShipFrom.Street1,

				to_city = lineItem.ShipTo.City,
				to_zip = lineItem.ShipTo.Zip,
				to_state = lineItem.ShipTo.State,
				to_country = lineItem.ShipTo.Country,
				to_street = lineItem.ShipTo.Street1,

				line_items = new List<TaxJarLineItem> {
					new TaxJarLineItem()
					{
						id = lineItem.LineItemID,
						quantity = lineItem.Quantity,
						unit_price = lineItem.UnitPrice,
						description = lineItem.ProductName,
						product_identifier = lineItem.ProductID,
					}
				}
			};
		}
	}
}
