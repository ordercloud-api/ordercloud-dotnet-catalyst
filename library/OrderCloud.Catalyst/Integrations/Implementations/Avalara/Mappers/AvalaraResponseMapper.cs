using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class AvalaraResponseMapper
	{
		public static OrderTaxCalculation ToOrderTaxCalculation(AvalaraTransactionModel avalaraTransaction)
		{
			var shippingLines = avalaraTransaction.lines?.Where(line => line.taxCode == "FR") ?? new List<AvalaraTransactionLineModel>();
			var itemLines = avalaraTransaction.lines?.Where(line => line.taxCode != "FR") ?? new List<AvalaraTransactionLineModel>();
			return new OrderTaxCalculation()
			{
				OrderID = avalaraTransaction.purchaseOrderNo,
				ExternalTransactionID = avalaraTransaction.code,
				TotalTax = avalaraTransaction.totalTax ?? 0,
				LineItems = itemLines.Select(ToItemTaxDetails).ToList(),
				OrderLevelTaxes = shippingLines.SelectMany(ToShippingTaxDetails).ToList()
			};
		}

		public static IEnumerable<TaxDetails> ToShippingTaxDetails(AvalaraTransactionLineModel transactionLineModel)
		{
			return transactionLineModel.details?.Select(detail => ToTaxDetails(detail, transactionLineModel.lineNumber)) ?? new List<TaxDetails>();
		}

		public static LineItemTaxCalculation ToItemTaxDetails(AvalaraTransactionLineModel transactionLineModel)
		{
			return new LineItemTaxCalculation()
			{
				LineItemID = transactionLineModel.lineNumber,
				LineItemTotalTax = transactionLineModel.taxCalculated ?? 0,
				LineItemLevelTaxes = transactionLineModel.details?.Select(detail => ToTaxDetails(detail, null)).ToList() ?? new List<TaxDetails>()
			};
		}

		public static TaxDetails ToTaxDetails(AvalaraTransactionLineDetailModel detail, string shipEstimateID)
		{
			return new TaxDetails()
			{
				Tax = detail.tax ?? 0,
				Taxable = detail.taxableAmount ?? 0,
				Exempt = detail.exemptAmount ?? 0,
				TaxDescription = detail.taxName,
				JurisdictionLevel = detail.jurisdictionType.ToString(),
				JurisdictionValue = detail.jurisName,
				ShipEstimateID = shipEstimateID
			};
		}
	}
}
