using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace OrderCloud.Catalyst.TestApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Links to an Azure App Configuration resource that holds the app settings.
			// For local development, set this in your visual studio Env Variables.
			var connectionString = Environment.GetEnvironmentVariable("APP_CONFIG_CONNECTION");

			if (connectionString == null)
				throw new Exception("App settings not configured. Pass an Azure Config connection string into CreateWebHostBuilder().");

			WebHost.CreateDefaultBuilder(args)
				.UseDefaultServiceProvider(options => options.ValidateScopes = false)
				.ConfigureAppConfiguration((context, config) =>
				{
					config.AddAzureAppConfiguration(connectionString);
				})
				.UseStartup<Startup>()
				.ConfigureServices((ctx, services) =>
				{
					services.Configure<TestSettings>(ctx.Configuration);
					services.AddTransient(sp => sp.GetService<IOptionsSnapshot<TestSettings>>().Value);
				})
				.Build()
				.Run();
		}
	}
}
