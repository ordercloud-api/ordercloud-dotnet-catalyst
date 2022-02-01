using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class VertexRequestMapper
	{
		public static VertexCalculateTaxRequest ToVertexCalculateTaxRequest(this OrderWorksheet order, List<OrderPromotion> promosOnOrder, string companyCode, VertexSaleMessageType type)
		{
			var itemLines = order.LineItems.Select(li => ToVertexLineItem(li));
			var shippingLines = order.ShipEstimateResponse.ShipEstimates.Select(se =>
			{
				var firstLi = order.LineItems.FirstOrDefault(li => li.ID == se.ShipEstimateItems.First().LineItemID);
				Require.That(firstLi != null, new CatalystBaseException("InvalidOrderWorksheet", $"Invalid OrderWorksheet. Based on ShipEstimateItems, expected to find a LineItem with ID {se.ShipEstimateItems.First().LineItemID}", null, 400));
				return ToVertexLineItem(se, firstLi.ShippingAddress);
			});

			return new VertexCalculateTaxRequest()
			{
				postingDate = DateTime.Now.ToString("yyyy-MM-dd"),
				saleMessageType = type.ToString(),
				transactionType = VertexTransactionType.SALE.ToString(),
				transactionId = order.Order.ID,
				seller = new VertexSeller()
				{
					company = companyCode
				},
				customer = new VertexCustomer()
				{
					customerCode = new VertexCustomerCode()
					{
						classCode = order.Order.FromUserID,
						value = order.Order.FromUser.Email
					},
				},
				lineItems = itemLines.Concat(shippingLines).ToList()
			};
		}

		public static VertexLineItem ToVertexLineItem(LineItem lineItem)
		{
			return new VertexLineItem()
			{
				customer = new VertexCustomer()
				{
					destination = lineItem.ShippingAddress.ToVertexLocation(),
				},
				product = new VertexProduct()
				{
					productClass = lineItem.Product.ID,
					value = lineItem.Product.Name
				},
				quantity = new VertexMeasure()
				{
					value = lineItem.Quantity
				},
				unitPrice = (double) lineItem.UnitPrice,
				lineItemId = lineItem.ID,
				extendedPrice = (double) lineItem.LineTotal // this takes precedence over quanitity and unit price in determining tax cost
			};
		}

		public static VertexLineItem ToVertexLineItem(ShipEstimate shipEstimate, Address shipTo)
		{
			var selectedMethod = shipEstimate.ShipMethods.FirstOrDefault(m => m.ID == shipEstimate.SelectedShipMethodID);
			Require.That(selectedMethod != null, new CatalystBaseException("InvalidOrderWorksheet", $"Invalid OrderWorksheet. Based on SelectedShipMethodID, expected to find a ShipMethod with ID {shipEstimate.SelectedShipMethodID}", null, 400));
			return new VertexLineItem()
			{
				customer = new VertexCustomer()
				{
					destination = shipTo.ToVertexLocation(),
				},
				product = new VertexProduct()
				{
					productClass = "shipping_code",
					value = selectedMethod.Name
				},
				quantity = new VertexMeasure()
				{
					value = 1
				},
				unitPrice = (double) selectedMethod.Cost,
				lineItemId = shipEstimate.ID,
			};
		}

		public static VertexLocation ToVertexLocation(this Address address)
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
