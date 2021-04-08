using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace OrderCloud.Catalyst.Startup
{
    public static class FunctionHostBuilderExtensions
    {
        public static TSettings BuildSettingsFromAzureAppConfig<TSettings>(this IFunctionsHostBuilder host, string connectionString)
           where TSettings : class, new()
        {
            var settings = new TSettings();

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddAzureAppConfiguration(connectionString)
                .Build();

            config.Bind(settings);
            return settings;
        }
    }
}
