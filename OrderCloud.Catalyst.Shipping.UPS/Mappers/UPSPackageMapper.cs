using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Shipping.UPS
{
	public static class UPSPackageMapper
	{
        public static UPSRestRequestBody ToRateRequest(ShipPackage shipPackage)
        {
            return new UPSRestRequestBody
            {
                RateRequest = new UPSRateRequest
                {
                    Request = new UPSRequestParameters
                    {
                        SubVersion = "1703" // version of the rates api 
                    },
                    Shipment = new UPSShipment
                    {
                        Package = new UPSPackage
                        {
                            Dimensions = new UPSDimensions
                            {
                                Height = shipPackage.Height.ToString(),
                                Length = shipPackage.Length.ToString(),
                                Width = shipPackage.Width.ToString(),
                                UnitOfMeasurement = new UPSCodeDescription
                                {
                                    Code = "IN",
                                    Description = "Inches"
                                }
                            },
                            PackageWeight = new UPSWeightMeasurement
                            {
                                Weight = shipPackage.Weight.ToString(),
                                UnitOfMeasurement = new UPSCodeDescription
                                {
                                    Code = "LBS",
                                    Description = "Pounds"
                                }
                            },
                            PackagingType = new UPSCodeDescription
                            {
                                Code = "02",
                                Description = "Package"
                            }
                        },
                        Shipper = ToUPSAddress(shipPackage.ReturnAddress),
                        ShipFrom = ToUPSAddress(shipPackage.ShipFrom),
                        ShipTo = ToUPSAddress(shipPackage.ShipFrom),
                        ShipmentTotalWeight = new UPSWeightMeasurement
                        {
                            Weight = shipPackage.Weight.ToString(),
                            UnitOfMeasurement = new UPSCodeDescription
                            {
                                Code = "LBS",
                                Description = "Pounds"
                            }
                        },
                        Service = null,
                        ShipmentRatingOptions = new UPSShipmentRatingOptions
                        {
                            UserLevelDiscountIndicator = bool.TrueString
                        }
                    }
                }
            };
        }

        public static UPSAddress ToUPSAddress(Address address)
        {
            return new UPSAddress
            {
                Name = address.FirstName,
                //UPS account number on 'Shipper' object
                ShipperNumber = null,
                Address = new UPSAddressDetails
                {
                    City = address.City,
                    CountryCode = address.Country,
                    StateProvinceCode = address.State,
                    AddressLine = address.Street1,
                    PostalCode = address.Zip
                }
            };
        }
    }
}
