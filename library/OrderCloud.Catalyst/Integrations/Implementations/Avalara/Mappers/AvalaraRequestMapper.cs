using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class AvalaraRequestMapper
	{
		public static AvalaraCreateTransactionModel ToAvalaraTransactionModel(OrderSummaryForTax order, string companyCode, AvalaraDocumentType docType)
		{
			var shippingLines = order.ShipEstimates.Select(ToLineItemModel);
			var productLines = order.LineItems.Select(ToLineItemModel);
			return new AvalaraCreateTransactionModel()
			{
				companyCode = companyCode,
				type = docType,
				customerCode = order.CustomerCode,
				date = DateTime.Now,
				discount = GetOrderOnlyTotalDiscount(order),
				lines = productLines.Concat(shippingLines).ToList(),
				purchaseOrderNo = order.OrderID
			};
		}

		private static AvalaraLineItemModel ToLineItemModel(LineItemSummaryForTax lineItem)
		{
			return new AvalaraLineItemModel()
			{
				amount = lineItem.LineTotal, // Total after line-item level promotions have been applied
				quantity = lineItem.Quantity,
				taxCode = lineItem.TaxCode,
				itemCode = lineItem.ProductID,
				discounted = true, // Assumption that all products are eligible for order-level promotions
				customerUsageType = null,
				number = lineItem.LineItemID,
				addresses = ToAddressesModel(lineItem.ShipFrom, lineItem.ShipTo)
			};
		}

		private static AvalaraLineItemModel ToLineItemModel(ShipEstimateSummaryForTax shipEstimate)
		{
			return new AvalaraLineItemModel()
			{
				amount = shipEstimate.Cost,
				taxCode = "FR",
				itemCode = shipEstimate.Description,
				customerUsageType = null,
				number = shipEstimate.ShipEstimateID,
				addresses = ToAddressesModel(shipEstimate.ShipFrom, shipEstimate.ShipTo)
			};
		}

		private static decimal GetOrderOnlyTotalDiscount(OrderSummaryForTax order)
		{
			var sumOfLineItemLevelDiscounts = order.LineItems.Sum(li => li.PromotionDiscount);
			var orderLevelDiscount = order.PromotionDiscount - sumOfLineItemLevelDiscounts;
			return orderLevelDiscount;
		}

		private static AvalaraAddressesModel ToAddressesModel(Address shipFrom, Address shipTo)
		{
			return new AvalaraAddressesModel()
			{
				shipFrom = shipFrom.ToAddressLocationInfo(),
				shipTo = shipTo.ToAddressLocationInfo(),
			};
		}

		private static AvalaraAddressLocationInfo ToAddressLocationInfo(this Address address)
		{
			return new AvalaraAddressLocationInfo()
			{
				line1 = address.Street1,
				line2 = address.Street2,
				city = address.City,
				region = address.State,
				postalCode = address.Zip,
				country = address.Country
			};
		}
	}
}
