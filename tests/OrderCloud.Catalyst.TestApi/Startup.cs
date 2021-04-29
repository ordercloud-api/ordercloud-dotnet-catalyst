using OrderCloud.Catalyst;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderCloud.SDK;
using System.Threading.Tasks;
using System;
using NSubstitute;
using Microsoft.OpenApi.Models;

namespace OrderCloud.Catalyst.TestApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Links to an Azure App Configuration resource that holds the app settings.
			// For local development, set this in your visual studio Env Variables.
			var connectionString = Environment.GetEnvironmentVariable("APP_CONFIG_CONNECTION");

			CatalystWebHostBuilder
				.CreateWebHostBuilder<Startup, TestSettings>(args, connectionString)
				// If you do not wish to use Azure App Configuration, replace the line above and bind AppSettings as you choose.
				//.CreateWebHostBuilder<Startup>(args)
				.Build()
				.Run();
		}
	}

	public class Startup
	{
		private readonly TestSettings _settings;

		public Startup(TestSettings settings) {
			_settings = settings;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services) {
			services
				.ConfigureServices()
				.AddScoped<VerifiedUserContext, VerifiedUserContext>()
				.AddOrderCloudUserAuth()
				.AddOrderCloudWebhookAuth(opts => opts.HashKey = _settings.OrderCloudSettings.WebhookHashKey)
				.AddSingleton<ISimpleCache, LazyCacheService>() // Replace LazyCacheService with RedisService if you have multiple server instances.
				.AddSingleton<IOrderCloudClient>(new OrderCloudClient(new OrderCloudClientConfig() {
					ApiUrl = _settings.OrderCloudSettings.ApiUrl,
					AuthUrl = _settings.OrderCloudSettings.ApiUrl,
					ClientId = _settings.OrderCloudSettings.ClientID,
					ClientSecret = _settings.OrderCloudSettings.ClientSecret,
				}))
				.AddSwaggerGen(c =>
				 {
					 c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cataylst Test API", Version = "v1" });
				 });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			CatalystApplicationBuilder.DefaultCatalystAppBuilder(app, env)
				.UseSwagger()
				.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Catalyst Test API v1");
					c.RoutePrefix = string.Empty;
				});
		}
	}

	public class TestStartup : Startup
	{
		public static IOrderCloudClient oc;
		public TestStartup() : base(new TestSettings()) { }

		public override void ConfigureServices(IServiceCollection services)
		{
			// first do real service registrations
			base.ConfigureServices(services);

			// then replace some of them with fakes
			oc = Substitute.For<IOrderCloudClient>();
			oc.Me.GetAsync(Arg.Any<string>()).Returns(new MeUser { Username = "joe", Active = true, AvailableRoles = new[] { "Shopper" } });
			services.AddSingleton(oc);
		}
	}
}
