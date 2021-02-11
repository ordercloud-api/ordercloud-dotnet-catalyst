using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderCloud.Catalyst;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ordercloud.integrations.common;
using OrderCloud.Catalyst.Startup;
using OrderCloud.SDK;
using NSubstitute;

namespace OrderCloud.TestWebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CatalystWebHostBuilder.CreateWebHostBuilder<Startup>(args).Build().Run();
		}
	}

	public class Startup
	{
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services) {
			services.ConfigureServices();
			services.AddOrderCloudUser();
			services.AddSingleton<IOrderCloudClient>(new OrderCloudClient(new OrderCloudClientConfig()));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			CatalystApplicationBuilder.CreateApplicationBuilder(app, env);
		}
	}

	// This would make more sense in the OrderCloud.DemoWebApi.Tests Project. 
	// But moving it there break all the tests and its unclear why.
	public class TestStartup : Startup
	{
		public TestStartup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services)
		{
			base.ConfigureServices(services);

			// then replace some of them with fakes
			var oc = Substitute.For<IOrderCloudClient>();
			oc.Me.GetAsync(Arg.Any<string>()).Returns(new MeUser { Username = "joe", AvailableRoles = new[] { "Shopper" } });
			services.AddSingleton(oc);
		}
	}
}
