using System.Collections.Generic;
using System.Linq;

namespace OrderCloud.Catalyst.Tax.TaxJar
{
	public static class TaxJarResponseMapper
	{
		public static OrderTaxCalculation ToOrderTaxCalculation(IEnumerable<(TaxJarOrder request, TaxJarCalcResponse response)> taxJarOrders)
		{
			var itemLines = taxJarOrders.Where(r => r.request.line_items.First().product_identifier != TaxJarRequestMapper.ShippingLineCode);
			var shippingLines = taxJarOrders.Where(r => r.request.line_items.First().product_identifier == TaxJarRequestMapper.ShippingLineCode);

			return new OrderTaxCalculation()
			{
				OrderID = GetOrderID(taxJarOrders.First().request),
				ExternalTransactionID = null, // There are multiple external transactionIDs 
				TotalTax = taxJarOrders.Select(r => r.response.tax.amount_to_collect).Sum(),
				LineItems = itemLines.Select(ToItemTaxDetails).ToList(),
				OrderLevelTaxes = shippingLines.SelectMany(ToShippingTaxDetails).ToList()
			};
		}

		private static string GetOrderID(TaxJarOrder order) => order.transaction_id.Split('|')[1];
		private static string GetLineOrShipID(TaxJarOrder order) => order.transaction_id.Split('|')[3];


		private static LineItemTaxCalculation ToItemTaxDetails((TaxJarOrder request, TaxJarCalcResponse response) taxJarOrder)
		{
			return new LineItemTaxCalculation()
			{
				LineItemID = GetLineOrShipID(taxJarOrder.request),
				LineItemTotalTax = taxJarOrder.response.tax.amount_to_collect,
				LineItemLevelTaxes = ToTaxDetails(taxJarOrder.response)
			};
		}

		private static List<TaxDetails> ToShippingTaxDetails((TaxJarOrder request, TaxJarCalcResponse response) taxJarOrder)
		{
			var taxes = ToTaxDetails(taxJarOrder.response);
			var shipEstimateID = GetLineOrShipID(taxJarOrder.request);
			foreach (var tax in taxes)
			{
				tax.ShipEstimateID = shipEstimateID;
 			}
			return taxes;
		}

		private static List<TaxDetails> ToTaxDetails(TaxJarCalcResponse taxResponse)
		{
			var breakdown = taxResponse.tax.breakdown.line_items.First();
			var jurisidctions = new List<TaxDetails>();
			if (breakdown.county_amount> 0)
			{
				jurisidctions.Add(new TaxDetails()
				{
					Tax = breakdown.county_amount,
					Taxable = breakdown.county_taxable_amount,
					Exempt = 0,
					JurisdictionLevel = "County",
					JurisdictionValue = taxResponse.tax.jurisdictions.county,
					TaxDescription = $"{taxResponse.tax.jurisdictions.county} County tax",
					ShipEstimateID = null
				});
			}
			if (breakdown.city_amount > 0)
			{
				jurisidctions.Add(new TaxDetails()
				{
					Tax = breakdown.city_amount,
					Taxable = breakdown.city_taxable_amount,
					Exempt = 0,
					JurisdictionLevel = "City",
					JurisdictionValue = taxResponse.tax.jurisdictions.city,
					TaxDescription = $"{taxResponse.tax.jurisdictions.city} City tax",
					ShipEstimateID = null
				});
			}
			if (breakdown.state_amount > 0)
			{
				jurisidctions.Add(new TaxDetails()
				{
					Tax = breakdown.state_amount,
					Taxable = breakdown.state_taxable_amount,
					Exempt = 0,
					JurisdictionLevel = "State",
					JurisdictionValue = taxResponse.tax.jurisdictions.state,
					TaxDescription = $"{taxResponse.tax.jurisdictions.state} State tax",
					ShipEstimateID = null
				});
			}
			if (breakdown.special_district_amount > 0)
			{
				jurisidctions.Add(new TaxDetails()
				{
					Tax = breakdown.special_district_amount,
					Taxable = breakdown.special_district_taxable_amount,
					Exempt = 0,
					JurisdictionLevel = "Special District",
					JurisdictionValue = "Special District",
					TaxDescription = "Special District tax",
					ShipEstimateID = null
				});
			}
			return jurisidctions;
		}
	}
}
