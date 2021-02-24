using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace OrderCloud.Catalyst
{
	public static class CatalystWebHostBuilder
	{
		public static IWebHostBuilder CreateWebHostBuilder<TStartup, TAppSettings>(string[] args) where TStartup : class where TAppSettings : class, new() =>
			WebHost.CreateDefaultBuilder(args)
					.UseDefaultServiceProvider(options => options.ValidateScopes = false)
					.UseStartup<TStartup>()

				.ConfigureServices((ctx, services) =>
				{
					services.Configure<TAppSettings>(ctx.Configuration);
					services.AddTransient(sp => sp.GetService<IOptionsSnapshot<TAppSettings>>().Value);
				});
	}
}
