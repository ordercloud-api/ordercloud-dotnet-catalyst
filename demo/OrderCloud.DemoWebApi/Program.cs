using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OrderCloud.TestWebApi
{
	public class Program
	{
		public static void Main(string[] args) {
			ConfigureWebHostBuilder<Startup>(args).Build().Run();
		}

		public static IWebHostBuilder ConfigureWebHostBuilder<TStartup>(string[] args) where TStartup : class =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<TStartup>();
	}
}
