using AutoFixture;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst.Tests
{
	public class OrderWorksheetBuilder
	{
		private static Fixture _fixture = new Fixture();

		public OrderWorksheet Build()
		{
			var workSheet = _fixture.Create<OrderWorksheet>();

			// The object fixture auto-generates is invalid because some ID references don't exist.
			var lineItemID = workSheet.LineItems.First().ID;
			foreach (var shipEstimate in workSheet.ShipEstimateResponse.ShipEstimates)
			{
				shipEstimate.SelectedShipMethodID = shipEstimate.ShipMethods.First().ID; 
				foreach (var item in shipEstimate.ShipEstimateItems)
				{
					item.LineItemID = lineItemID;
				}
			}

			return workSheet;
		}
	}
}
