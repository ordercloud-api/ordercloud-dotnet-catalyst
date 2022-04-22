using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Shipping.Fedex
{
	public class FedexRateResponse
	{
		public string transactionId { get; set; }
		public string customerTransactionId { get; set; }
		public FedexRateResponseOutput output { get; set; } = new FedexRateResponseOutput();
	}

	public class FedexRateResponseOutput
	{
		public List<FedexRateReplyDetails> rateReplyDetails { get; set; } = new List<FedexRateReplyDetails>();
		public string quoteDate { get; set; }
		public bool encoded { get; set; }
		public List<FedexAlert> alerts { get; set; } = new List<FedexAlert>();
	}

	public class FedexAlert
	{
		public string code { get; set; }
		public string message { get; set; }
		public string alertType { get; set; } 
	}

	public class FedexRateReplyDetails
	{
		public string serviceType { get; set; }
		public string serviceName { get; set; }
		public string packagingType { get; set; }
		public List<FedexCustomerMessage> customerMessages { get; set; } = new List<FedexCustomerMessage>();
		public List<FedexRatedShipmentDetails> ratedShipmentDetails { get; set; } = new List<FedexRatedShipmentDetails>();
		public bool anonymousAllowable { get; set; }
		public FedexOperationalDetails operationalDetail { get; set; } = new FedexOperationalDetails();
		public string signatureOptionType { get; set; }
		public FedexServiceDescription serviceDescription { get; set; } = new FedexServiceDescription();
		public FedexCommit commit { get; set; } = new FedexCommit();

	}

	public class FedexCustomerMessage
	{
		public string code { get; set; }
		public string message { get; set; }
	}

	public class FedexCommit
	{
		public FedexDateDetail dateDetail { get; set; } = new FedexDateDetail(); 
	}

	public class FedexDateDetail
	{
		public string dayOfWeek { get; set; }
		public DateTime dayCxsFormat { get; set; }
	}

	public class FedexOperationalDetails
	{
		public string originLocationIds { get; set; }
		public string commitDays { get; set; }
		public string serviceCode { get; set; }
		public string airportId { get; set; }
		public string scac { get; set; }
		public string originSerivceAreas { get; set; }
		public string deliveryDay { get; set; }
		public int originLocationNumbers { get; set; }
		public string destinationPostalCode { get; set; }
		public string commitDate { get; set; }
		public string astraDescription { get; set; }
		public string deliveryDate { get; set; }
		public string deliveryEligibilities { get; set; }
		public bool ineligibleForMoneyBackGuarantee { get; set; }
		public string maximmumTransitTime { get; set; }
		public string astraPLannedServiceLevel { get; set; }
		public string destinationLocationIds { get; set; }
		public string destinationLocationStateOrProvinceCodes { get; set; }
		public string transitTime { get; set; }
		public string packagingCode { get; set; }
		public int destinationLocationNumbers { get; set; }
		public string publishedDeliveryTime { get; set; }
		public string countryCodes { get; set; }
		public string stateOrProvinceCodes { get; set; }
		public string ursaPrefixCode { get; set; }
		public string urdaSuffixCode { get; set; }
		public string destinationServiceAreas { get; set; }
		public string originPostalCodes { get; set; }
		public string customTransitTime { get; set; }
	}
}
