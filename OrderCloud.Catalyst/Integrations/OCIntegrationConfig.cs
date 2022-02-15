using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// A base class that all Integration Config classes should extend. Contains environment variables needed for that integration.
	/// </summary>
	public abstract class OCIntegrationConfig
	{
		public abstract string ServiceName { get; } 
	}
}
