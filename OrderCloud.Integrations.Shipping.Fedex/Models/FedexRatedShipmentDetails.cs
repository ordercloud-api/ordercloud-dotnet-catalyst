using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Shipping.Fedex
{
	public class FedexRatedShipmentDetails
	{
		public string rateType { get; set; }
		public string rateWeightMathod { get; set; }
		public decimal totalDiscounts { get; set; }
		public decimal totalBaseCharge { get; set; }
		public decimal totalNetCharge { get; set; }
		public decimal totalVatCharge { get; set; }
		public decimal totalNetFedExCharge { get; set; }
		public decimal totalDutiesAndTaxes { get; set; }
		public decimal totalNetChargeWithDutiesAndTaxes { get; set; }
		public decimal totalDutiesTaxesAndFees { get; set; }
		public decimal totalAncillaryFeesAndTaxes { get; set; }
		public string currency { get; set; }

	}

	public class FedexShipmentRateDetails
	{
		public string rateZone { get; set; }
		public decimal dimDivisor { get; set; }
		public decimal fuelSurchargePercent { get; set; }
		public decimal totalSurcharges { get; set; }
		public decimal totalFreightDiscount { get; set; }
		public string pricingCode { get; set; }
		public string currency { get; set; }
		public FedexWeight totalBillingWeight = new FedexWeight();
		public FedexCurrencyExchange currencyExchangeRate { get; set; } = new FedexCurrencyExchange();
		public List<FedexSurCharge> surCharges { get; set; } = new List<FedexSurCharge>();
	}

	public class FedexCurrencyExchange
	{
		public string fromCurrency { get; set; }
		public string intoCurrency { get; set; }
		public double rate { get; set; }
	}

	public class FedexSurCharge
	{
		public string type { get; set; }
		public string description { get; set; }
		public decimal amount { get; set; }
	}
}
