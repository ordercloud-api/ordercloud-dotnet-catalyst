using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.EasyPost
{
	public static class EasyPostServiceMapper
	{
		public static EasyPostShipment ToEasyPostShipment(ShipPackage package, List<string> carrierAccountIDs)
		{
			return new EasyPostShipment()
			{
				from_address = ToEasyPostAddress(package.ShipFrom),
				to_address = ToEasyPostAddress(package.ShipTo),
				parcel = new EasyPostParcel()
				{
					length = (double)package.Length,
					width = (double)package.Width,
					height = (double)package.Height,
					weight = (double)package.Weight
				},
				customs_info = ToEasyPostCustomsInfo(package.Customs, package.ShipFrom.Country),
				insurance = ToEasyPostInsurance(package),
				options = new EasyPostOptions()
				{
					delivery_confirmation = package.SignatureRequired ? "SIGNATURE" : "NO_SIGNATURE",
				},
				carrier_accounts = carrierAccountIDs.Select(id => new EasyPostCarrierAccount() { id = id }).ToList()
			};
		}

		private static EasyPostInsurance ToEasyPostInsurance(ShipPackage shipment)
		{
			return new EasyPostInsurance()
			{
				amount = shipment.Insurance.Amount.ToString(),
				from_address = ToEasyPostAddress(shipment.ShipFrom),
				to_address = ToEasyPostAddress(shipment.ShipTo)
			};
		}


		private static EasyPostCustomsInfo ToEasyPostCustomsInfo(CustomsInfo info, string originCountry)
		{
			return new EasyPostCustomsInfo()
			{
				contents_type = info.ContentType,
				restriction_type = "none",
				eel_pfc = info.EEL_PFC,
				customs_certify = true,
				customs_items = info.Items.Select(item => ToEasyPostCustomsItem(item, originCountry)).ToList()
			};
		}

		private static EasyPostCustomsItem ToEasyPostCustomsItem(CustomsItem item, string originCountry)
		{
			return new EasyPostCustomsItem()
			{
				description = item.Description,
				origin_country = originCountry,
				value = shipEstimateItem.Quantity * (double)lineItem.UnitPrice,
				quantity = shipEstimateItem.Quantity,
				weight = shipEstimateItem.Quantity * (double)lineItem.Product.ShipWeight
			};
		}

		public static IList<ShipMethod> ToOrderCloudShipMethods(EasyPostShipment shipment)
		{
			if (shipment.rates != null) // if no rates are returned
			{
				return new List<ShipMethod> { };
			}
			return shipment.rates.Select(rate =>
			{
				return new ShipMethod()
				{
					ID = rate.id,
					Name = rate.service,
					Cost = decimal.Parse(rate.rate),
					EstimatedTransitDays = rate.delivery_days ?? 5,
					xp = new ShipMethodXp()
					{
						Carrier = rate.carrier,
						// These additional fields were useful for SEB, but we should compare to other shipping models to see how they can be integrated. 
						//CarrierAccountID = rate.carrier_account_id,
						//OriginalCost = decimal.Parse(rate.rate),
						//ListCost = decimal.Parse(rate.list_rate),
						//IsDeliveryDateGuaranteed = rate.delivery_date_guaranteed
					}
				};
			}).ToList();
		}
		public static EasyPostAddress ToEasyPostAddress(Address address)
		{
			return new EasyPostAddress()
			{
				street1 = address.Street1,
				street2 = address.Street2,
				city = address.City,
				state = address.State,
				zip = address.Zip,
				country = address.Country,
			};
		}
	}
}
