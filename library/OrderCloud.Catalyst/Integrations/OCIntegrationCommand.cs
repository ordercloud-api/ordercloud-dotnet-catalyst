using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// A base class that all Integration Command classes should extend. Exposes methods that are the behaviors of the integration.
	/// </summary>
	public abstract class OCIntegrationCommand
	{
		public OCIntegrationCommand(OCIntegrationConfig config)
		{
			config.ValidateRequiredFields();
		}
	}
}
