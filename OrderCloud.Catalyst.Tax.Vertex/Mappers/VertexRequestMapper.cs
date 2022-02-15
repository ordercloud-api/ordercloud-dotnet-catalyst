using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace OrderCloud.Catalyst.Tax.Vertex
{
	public static class VertexRequestMapper
	{
		public const string ShippingLineCode = "shipping_code";

		public static VertexCalculateTaxRequest ToVertexCalculateTaxRequest(OrderSummaryForTax order, string companyCode, VertexSaleMessageType type)
		{
			var itemLines = order.LineItems.Select(ToVertexLineItem);
			var shippingLines = order.ShipEstimates.Select(ToVertexShipLineItem);

			return new VertexCalculateTaxRequest()
			{
				postingDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
				saleMessageType = type.ToString(),
				transactionType = VertexTransactionType.SALE.ToString(),
				transactionId = order.OrderID,
				seller = new VertexSeller()
				{
					company = companyCode
				},
				customer = new VertexCustomer()
				{
					customerCode = new VertexCustomerCode()
					{
						value = order.CustomerCode
					},
				},
				lineItems = itemLines.Concat(shippingLines).ToList()
			};
		}

		public static VertexLineItem ToVertexLineItem(LineItemSummaryForTax lineItem)
		{
			return new VertexLineItem()
			{
				customer = new VertexCustomer()
				{
					destination = ToVertexLocation(lineItem.ShipTo),
				},
				product = new VertexProduct()
				{
					productClass = lineItem.ProductID,
					value = lineItem.ProductName
				},
				quantity = new VertexMeasure()
				{
					value = lineItem.Quantity
				},
				unitPrice = lineItem.UnitPrice,
				lineItemId = lineItem.LineItemID,
				discount = new VertexDiscount()
				{

				},
				extendedPrice = lineItem.LineTotal // this takes precedence over quanitity and unit price in determining tax cost
			};
		}

		public static VertexLineItem ToVertexShipLineItem(ShipEstimateSummaryForTax shipEstimate)
		{
			return new VertexLineItem()
			{
				customer = new VertexCustomer()
				{
					destination = ToVertexLocation(shipEstimate.ShipTo),
				},
				product = new VertexProduct()
				{
					productClass = ShippingLineCode,
					value = shipEstimate.Description
				},
				quantity = new VertexMeasure()
				{
					value = 1
				},
				unitPrice = shipEstimate.Cost,
				lineItemId = shipEstimate.ShipEstimateID,
			};
		}

		public static VertexLocation ToVertexLocation(Address address)
		{
			return new VertexLocation()
			{
				streetAddress1 = address.Street1,
				streetAddress2 = address.Street2,
				city = address.City,
				mainDivision = address.State,
				postalCode = address.Zip,
				country = address.Country
			};
		}
	}
}
