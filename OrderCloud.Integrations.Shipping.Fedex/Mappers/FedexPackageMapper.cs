using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.Fedex
{
	public class FedexPackageMapper
	{
		public static FedexRateRequestBody ToRateRequest(ShippingPackage shipPackage, string accountNumber)
		{
			return new FedexRateRequestBody()
			{
				accountNumber = new FedexAccountNumber()
				{
					value = accountNumber
				},
				rateRequestControlParameters = new FedexRateRequestControlParameters()
				{
					returnTransitTimes = true
				},
				requestedShipment = new FedexRequestedShipment()
				{
					shipper = ToAddress(shipPackage.ShipFrom),
					recipient = ToAddress(shipPackage.ShipTo),
					// this means get rates based on the account number provided. See https://developer.fedex.com/api/en-us/catalog/rate/v1/docs.html
					rateRequestType = new List<string>() { "ACCOUNT" },
					// This is intended to be used for shipping cost estimates, not actual fufillment, so we don't know what the pickup type will be.
					// It is required to provide a value so "DROPOFF_AT_FEDEX_LOCATION" is a reasonable default.
					// See https://developer.fedex.com/api/en-us/guides/api-reference.html#pickuptypes
					pickupType = "DROPOFF_AT_FEDEX_LOCATION",
					requestedPackageLineItems = new List<FedexRequestedPackageLineItem>()
					{
						new FedexRequestedPackageLineItem()
						{
							weight = new FedexWeight()
							{
								value = (double)shipPackage.Weight,
								units = "LB"
							},
							dimensions = new FedexDimensions()
							{
								length = (int)shipPackage.Length,
								height = (int)shipPackage.Height,
								width = (int)shipPackage.Width,
								units = "IN"
							},
							declaredValue = new FedexMoney()
							{
								amount = shipPackage.Insurance.Amount,
								currency = shipPackage.Insurance.Currency
							}
						}
					},
					customsClearanceDetail = new FedexCustomsClearanceDetails()
					{
						commodities = shipPackage.Customs.Items.Select(ToCustomsCommodity).ToList()
					}
				}		
			};
		}

		public static FedexCustomsCommodity ToCustomsCommodity(CustomsItem item)
		{
			return new FedexCustomsCommodity()
			{
				description = item.Description,
				weight = new FedexWeight()
				{
					units = "LB",
					value = (double)(item.UnitWeight * item.Quantity)
				},
				quantity = item.Quantity,
				customsValue = new FedexMoney()
				{
					amount = (item.UnitPrice * item.Quantity),
					currency = item.Currency
				}
			};
		}

		public static FedexAddressWrapper ToAddress(Address address)
		{
			return new FedexAddressWrapper()
			{
				address = new FedexAddress()
				{
					city = address.City,
					stateOrProvinceCode = address.State,
					postalCode = address.Zip,
					countryCode = address.Country
				}
			};
		}
	}
}
