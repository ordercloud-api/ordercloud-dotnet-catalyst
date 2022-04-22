using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Tax.TaxJar
{
	public class TaxJarCategories
	{
		public List<TaxJarCategory> categories { get; set; } = new List<TaxJarCategory> { };
	}

	public class TaxJarCategory
	{
		public string name { get; set; }
		public string product_tax_code { get; set; }
		public string description { get; set; }
	}
}
