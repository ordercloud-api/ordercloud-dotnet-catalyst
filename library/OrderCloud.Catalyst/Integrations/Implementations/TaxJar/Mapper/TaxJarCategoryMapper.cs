using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderCloud.Catalyst
{
	public static class TaxJarCategoryMapper
	{
		public static TaxCategorizationResponse ToTaxCategorization(TaxJarCategories list, string filterTerm)
		{
			var categories = list.categories
				.Where(c => MatchesSearch(c, filterTerm))
				.Select(ToTaxCategorization)
				.ToList();

			return new TaxCategorizationResponse() { Categories = categories };
		}

		public static bool MatchesSearch(TaxJarCategory category, string filterTerm)
		{
			if (filterTerm.IsNullOrEmpty()) { return true; }
			var filter = filterTerm.ToLower();
			return category.name.ToLower().Contains(filter) || category.description.ToLower().Contains(filter);
		}

		public static TaxCategorization ToTaxCategorization(TaxJarCategory category)
		{
			return new TaxCategorization()
			{
				Code = category.product_tax_code,
				Description = category.name,
				LongDescription = category.description
			};
		}

	}
}
