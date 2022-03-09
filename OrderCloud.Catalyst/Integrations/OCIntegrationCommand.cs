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
		protected readonly OCIntegrationConfig _defaultConfig;

		public OCIntegrationCommand(OCIntegrationConfig defaultConfig)
		{
			ValidateConfigData(defaultConfig);
			_defaultConfig = defaultConfig;
		}

		protected void ValidateConfigData(OCIntegrationConfig config)
		{
			if (config == null) return;
			var type = config.GetType();
			var missing = type
				.GetProperties()
				.Where(prop =>
				{
					var value = prop.GetValue(config);
					var isRequired = Attribute.IsDefined(prop, typeof(RequiredIntegrationFieldAttribute));
					return isRequired && value == null;
				});

			if (missing.Any())
			{
				var names = missing.Select(p => p.Name).ToList();
				throw new IntegrationMissingConfigsException(config, names);
			}
		}

		protected void ValidateConfigType<T>(OCIntegrationConfig config) where T : OCIntegrationConfig
		{
			if (config == null) return;
			var type = config.GetType();
			if (type != typeof(T))
			{
				throw new ArgumentException($"Integration configuration must be of type {typeof(T).Name} to match this command. Found {type.Name} instead.", "configOverride");
			}
		}

		protected T ValidateConfig<T>(OCIntegrationConfig config) where T : OCIntegrationConfig
		{
			ValidateConfigType<T>(config);
			ValidateConfigData(config);
			return config as T;
		}
	}
}
