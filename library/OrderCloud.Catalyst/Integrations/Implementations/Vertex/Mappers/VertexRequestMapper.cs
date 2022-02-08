﻿using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class VertexRequestMapper
	{
		public const string ShippingLineCode = "shipping_code";

		public static VertexCalculateTaxRequest ToVertexCalculateTaxRequest(this OrderWorksheet order, List<OrderPromotion> promosOnOrder, string companyCode, VertexSaleMessageType type)
		{
			var itemLines = order.LineItems.Select(li => ToVertexLineItem(li));
			var shippingLines = order.ShipEstimateResponse.ShipEstimates.Select(se =>
			{
				var firstLi = order.GetShipEstimateLineItems(se.ID).First().LineItem;
				return ToVertexShipLineItem(se, firstLi.ShippingAddress);
			});

			return new VertexCalculateTaxRequest()
			{
				postingDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
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
				unitPrice = lineItem.UnitPrice ?? 0,
				lineItemId = lineItem.ID,
				extendedPrice = lineItem.LineTotal // this takes precedence over quanitity and unit price in determining tax cost
			};
		}

		public static VertexLineItem ToVertexShipLineItem(ShipEstimate shipEstimate, Address shipTo)
		{
			var selectedMethod = shipEstimate.GetSelectedShipMethod();
			return new VertexLineItem()
			{
				customer = new VertexCustomer()
				{
					destination = shipTo.ToVertexLocation(),
				},
				product = new VertexProduct()
				{
					productClass = ShippingLineCode,
					value = selectedMethod.Name
				},
				quantity = new VertexMeasure()
				{
					value = 1
				},
				unitPrice = selectedMethod.Cost,
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
