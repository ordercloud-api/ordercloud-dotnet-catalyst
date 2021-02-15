using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class CatalystWebHostBuilder
	{
		public static IWebHostBuilder CreateWebHostBuilder<TStartup>(string[] args) where TStartup : class =>
			WebHost.CreateDefaultBuilder(args)
					.UseDefaultServiceProvider(options => options.ValidateScopes = false)
					.UseStartup<TStartup>();
	}
}
