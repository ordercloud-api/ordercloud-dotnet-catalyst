using OrderCloud.Catalyst;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderCloud.SDK;
using NSubstitute;
using OrderCloud.DemoWebApi;
using System.Threading.Tasks;
using System;
using OrderCloud.DemoWebApi.Services;

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
			services
				.ConfigureServices(new AppSettings())
				.AddSingleton<ISimpleCache, LazyCacheService>() // Replace LazyCacheService with RedisService if you have multiple server instances.
				.AddSingleton<IOrderCloudClient>(new OrderCloudClient(new OrderCloudClientConfig()));
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
		public static IOrderCloudClient OC;

		public TestStartup(IConfiguration configuration) : base(configuration) { }

		public override void ConfigureServices(IServiceCollection services)
		{
			// Inject services as normal
			base.ConfigureServices(services);

			// then replace some of them with fakes
			OC = Substitute.For<IOrderCloudClient>();
			OC.Me.GetAsync(Arg.Any<string>()).Returns(new MeUser { Username = "joe", ID = "", Active = true, AvailableRoles = new[] { "Shopper" } });
			services.AddSingleton(OC);
		}
	}
}
