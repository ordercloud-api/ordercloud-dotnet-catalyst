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
			// Links to an Azure App Configuration resource that holds the app settings.
			// For local development, set this in your visual studio Env Variables.
			var connectionString = Environment.GetEnvironmentVariable("APP_CONFIG_CONNECTION");

			CatalystWebHostBuilder
				.CreateWebHostBuilder<Startup, AppSettings>(args, connectionString)
				// If you do not wish to use Azure App Configuration, replace the line above and bind AppSettings as you choose.
				//.CreateWebHostBuilder<Startup>(args)
				.Build()
				.Run();
		}
	}

	public class Startup
	{
		private readonly AppSettings _settings;

		public Startup(AppSettings settings) {
			_settings = settings;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services) {
			services
				.ConfigureServices()
				.AddOrderCloudUserAuth<AppSettings>()
				.AddOrderCloudWebhookAuth(opts => opts.HashKey = _settings.OrderCloudSettings.WebhookHashKey)
				.AddSingleton<ISimpleCache, LazyCacheService>() // Replace LazyCacheService with RedisService if you have multiple server instances.
				.AddSingleton<IOrderCloudClient>(new OrderCloudClient(new OrderCloudClientConfig() {
					ApiUrl = _settings.OrderCloudSettings.ApiUrl,
					AuthUrl = _settings.OrderCloudSettings.ApiUrl,
					ClientId = _settings.OrderCloudSettings.MiddlewareClientID,
					ClientSecret = _settings.OrderCloudSettings.MiddlewareClientSecret,
					Roles = new[] { ApiRole.FullAccess }
				}));
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

		public TestStartup() : base(new AppSettings()) { }

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
