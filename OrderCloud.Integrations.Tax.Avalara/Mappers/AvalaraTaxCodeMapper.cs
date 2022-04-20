using OrderCloud.SDK;
using System;
using System.Linq;
using OrderCloud.Catalyst;
using System.Collections.Generic;

namespace OrderCloud.Integrations.Tax.Avalara
{
	public static class AvalaraTaxCodeMapper
	{
		// Tax Codes for lines on Transactions
		public static List<TaxCategorization> MapTaxCodes(List<AvalaraTaxCode> codes)
		{
			return codes.Select(MapTaxCode).ToList();
		}

		public static TaxCategorization MapTaxCode(AvalaraTaxCode code)
		{
			return new TaxCategorization
			{
				Code = code.taxCode,
				Description = code.description
			};
		}

		public static string MapFilterTerm(string searchTerm)
		{
			var searchString = $"isActive eq true";
			if (searchTerm != "")
			{
				searchString = $"{searchString} and (taxCode contains '{searchTerm}' OR description contains '{searchTerm}')";
			}
			return searchString;
		}
	}
}
