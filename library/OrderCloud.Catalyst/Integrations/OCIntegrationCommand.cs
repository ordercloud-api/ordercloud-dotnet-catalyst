using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// A base class that all Integration Command classes should extend. Exposes methods that are the behaviors of the integration.
	/// </summary>
	public abstract class OCIntegrationCommand
	{
		protected readonly OCIntegrationConfig _configDefault;

		public OCIntegrationCommand(OCIntegrationConfig configDefault)
		{
			_configDefault = configDefault;
		}

		public T GetValidatedConfig<T>(OCIntegrationConfig configOverride) where T : OCIntegrationConfig
		{
			var config = configOverride ?? _configDefault; 
			var type = config.GetType();
			if (type != typeof(T))
			{
				throw new ArgumentException($"Integration configuration must be of type {typeof(T).Name} to match this command. Found {type.Name} instead.", "configOverride");
			}

			var missing = type
				.GetProperties()
				.Where(prop =>
				{
					var value = (string)prop.GetValue(config);
					var isRequired = Attribute.IsDefined(prop, typeof(RequiredIntegrationFieldAttribute));
					return isRequired && value.IsNullOrEmpty();
				});

			if (missing.Any())
			{
				var names = missing.Select(p => p.Name).ToList();
				throw new IntegrationMissingConfigsException(config, names);
			}

			return config as T;
		}
	}
}
