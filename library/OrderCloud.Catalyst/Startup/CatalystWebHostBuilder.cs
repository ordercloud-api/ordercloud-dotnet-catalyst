using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class CatalystWebHostBuilder
	{
		public static IWebHostBuilder CreateWebHostBuilder<TStartup, TAppSettings>(string[] args) where TStartup : class where TAppSettings : class, new() =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<TStartup>()
				.UseIISIntegration()

				.ConfigureServices((ctx, services) =>
				{
					services.Configure<TAppSettings>(ctx.Configuration);
					services.AddTransient(sp => sp.GetService<IOptionsSnapshot<TAppSettings>>().Value);
				});

		public static IWebHostBuilder CreateWebHostBuilder<TStartup, TAppSettings>(string[] args, string azureConfigConnectionString)
			where TStartup : class where TAppSettings : class, new()
		{
			if (azureConfigConnectionString == null)
				throw new Exception("App settings not configured. Pass an Azure Config connection string into CreateWebHostBuilder().");

			return WebHost.CreateDefaultBuilder(args)
						.UseDefaultServiceProvider(options => options.ValidateScopes = false)
						.ConfigureAppConfiguration((context, config) =>
						{
							config.AddAzureAppConfiguration(azureConfigConnectionString);
						})
						.UseStartup<TStartup>().ConfigureServices((ctx, services) =>
						{
							services.Configure<TAppSettings>(ctx.Configuration);
							services.AddTransient(sp => sp.GetService<IOptionsSnapshot<TAppSettings>>().Value);
						});
		}
	}
}
