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

		public void ValidateRequiredFields()
		{
			var props = GetType()
				.GetProperties();
			var missing = props.Where(prop =>
				{
					var value = (string)prop.GetValue(this);
					var isRequired = Attribute.IsDefined(prop, typeof(RequiredIntegrationFieldAttribute));
					return isRequired && (value == null || value == "");
				});

			if (missing.Any())
			{
				var names = missing.Select(p => p.Name).ToList();
				throw new IntegrationMissingConfigsException(this, names);
			}
		}
	}


}
