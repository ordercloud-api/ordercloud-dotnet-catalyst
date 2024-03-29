﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OrderCloud.Catalyst;

namespace OrderCloud.Integrations.Tax.Vertex
{
	public static class VertexResponseMapper
	{
		public static bool IsShippingLineItem(this VertexResponseLineItem li) => li.product.productClass == VertexRequestMapper.ShippingLineCode;

		public static OrderTaxCalculation ToOrderTaxCalculation(this VertexCalculateTaxResponse response)
		{
			var shippingLines = response.lineItems?.Where(li => li.IsShippingLineItem()) ?? new List<VertexResponseLineItem>();
			var itemLines = response.lineItems?.Where(li => !li.IsShippingLineItem()) ?? new List<VertexResponseLineItem>();

			return new OrderTaxCalculation()
			{
				OrderID = response.transactionId,
				ExternalTransactionID = response.transactionId,
				TotalTax = response.totalTax,
				LineItems = itemLines.Select(ToItemTaxDetails).ToList(),
				OrderLevelTaxes = shippingLines.SelectMany(ToShippingTaxDetails).ToList()
			};
		}

		public static IEnumerable<TaxDetails> ToShippingTaxDetails(this VertexResponseLineItem transactionLineModel)
		{
			return transactionLineModel.taxes?.Select(detail => detail.ToTaxDetails(transactionLineModel.lineItemId)) ?? new List<TaxDetails>();
		}

		public static LineItemTaxCalculation ToItemTaxDetails(this VertexResponseLineItem transactionLineModel)
		{
			return new LineItemTaxCalculation()
			{
				LineItemID = transactionLineModel.lineItemId,
				LineItemTotalTax = transactionLineModel.totalTax,
				LineItemLevelTaxes = transactionLineModel.taxes?.Select(detail => detail.ToTaxDetails(null)).ToList() ?? new List<TaxDetails>()
			};
		}

		public static TaxDetails ToTaxDetails(this VertexTax detail, string shipEstimateID)
		{
			return new TaxDetails()
			{
				Tax = detail.calculatedTax,
				Taxable = detail.taxable,
				Exempt = 0, // we don't get a property back for exempt
				TaxDescription = detail.impositionType.value,
				JurisdictionLevel = detail.jurisdiction.jurisdictionLevel.ToString(),
				JurisdictionValue = detail.jurisdiction.value,
				ShipEstimateID = shipEstimateID
			};
		}
	}
}
