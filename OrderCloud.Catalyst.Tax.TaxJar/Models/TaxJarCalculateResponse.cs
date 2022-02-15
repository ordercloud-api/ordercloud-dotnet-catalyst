using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tax.TaxJar
{
	public class TaxJarCalcResponse
	{
		public TaxJarTax tax { get; set; }
	}

	public class TaxJarTax
	{
		public decimal order_total_amount { get; set; }
		public decimal shipping { get; set; }
		public decimal taxable_amount { get; set; }
		public decimal amount_to_collect { get; set; }
		public decimal rate { get; set; }
		public bool has_nexus { get; set; }
		public bool freight_taxable { get; set; }
		public string tax_source { get; set; }
		public string exemption_type { get; set; }
		public TaxJarJurisdiction jurisdictions { get; set; }
		public TaxJarBreakdown breakdown { get; set; }
	} 

	public class TaxJarJurisdiction
	{
		public string country { get; set; }
		public string state { get; set; }
		public string county { get; set; }
		public string city { get; set; }
	}

	public class TaxJarBreakdown
	{
		public decimal state_tax_rate { get; set; }
		public decimal state_tax_collectable { get; set; }
		public decimal state_taxable_amount { get; set; }
		public decimal county_tax_collectable { get; set; }
		public decimal county_taxable_amount { get ; set; }
		public decimal city_tax_collectable { get; set; }
		public decimal city_taxable_amount { get; set; }
		public decimal special_tax_rate { get; set; }
		public decimal special_district_tax_collectable { get; set; }
		public decimal special_district_taxable_amount { get; set; }
		public TaxBreakdownShipping shipping { get; set; }
		public List<TaxBreakdownLineItem> line_items { get; set; }
	}

	public class TaxBreakdownLineItem
	{
		public string id { get; set; }
		public decimal state_sales_tax_rate { get; set; }
		public decimal state_taxable_amount { get; set; }
		public decimal state_amount { get; set; }
		public decimal county_amount { get; set; }
		public decimal county_taxable_amount { get; set; }
		public decimal city_amount { get; set; }
		public decimal city_taxable_amount { get; set; }
		public decimal special_district_taxable_amount { get; set; }
		public decimal special_tax_rate { get; set; }
		public decimal special_district_amount { get; set; }
	}

	public class TaxBreakdownShipping
	{
		public decimal state_sales_tax_rate { get; set; }
		public decimal state_taxable_amount { get; set; }
		public decimal state_amount { get; set; }
		public decimal county_amount { get; set; }
		public decimal county_taxable_amount { get; set; }
		public decimal city_amount { get; set; }
		public decimal city_taxable_amount { get; set; }
		public decimal special_taxable_amount { get; set; }
		public decimal special_tax_rate { get; set; }
		public decimal special_district_amount { get; set; }
	}
}
