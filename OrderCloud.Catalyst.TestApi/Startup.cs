using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrderCloud.SDK;
using NSubstitute;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace OrderCloud.Catalyst.TestApi
{
	public class Startup
	{
		private readonly TestSettings _settings;
		public const string UnitTestClientID = "mYcLiEnTiD";

		public Startup(TestSettings settings)
		{
			_settings = settings;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services)
		{
			services
				.AddControllers()
				.ConfigureApiBehaviorOptions(o =>
				{
					o.SuppressModelStateInvalidFilter = true;
				}).AddNewtonsoftJson(options =>
				{
					options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
				});
			services.AddMvc(o =>
				{
					o.Filters.Add(new ValidateModelAttribute());
					o.EnableEndpointRouting = false;
				})
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
				.AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

			services.AddCors(o => o.AddPolicy("integrationcors",
				builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
			services
				.AddOrderCloudUserAuth(opts => opts.AddValidClientIDs(UnitTestClientID))
				.AddOrderCloudWebhookAuth(opts => opts.HashKey = _settings.OrderCloudSettings.WebhookHashKey)
				.AddSingleton<ISimpleCache, LazyCacheService>() // Replace LazyCacheService with RedisService if you have multiple server instances.
				.AddSingleton<IOrderCloudClient>(new OrderCloudClient(new OrderCloudClientConfig()
				{
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
		public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.Use((context, next) =>
			{
				context.Request.EnableBuffering(); // allows the body to be read twice for verifying webhooks in the controller route.
				return next();
			});
			app.UseCatalystExceptionHandler();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors("integrationcors");
			app.UseAuthorization();
			app.UseEndpoints(endpoints => endpoints.MapControllers());
			app.UseSwagger();
			app.UseSwaggerUI(c =>
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
			oc.Config.ReturnsForAnyArgs(new OrderCloudClientConfig() 
			{
				ApiUrl = "mockdomain.com",
				AuthUrl = "mockdomain.com",
			});
			oc.Me.GetAsync(Arg.Any<string>()).Returns(new MeUser { Username = "joe", Active = true, AvailableRoles = new[] { "Shopper" } });
			services.AddSingleton(oc);
		}

		public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// Same as real Startup above except for UseCatalystExceptionHandler(false)
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.Use((context, next) =>
			{
				context.Request.EnableBuffering(); // allows the body to be read twice for verifying webhooks in the controller route.
				return next();
			});
			app.UseCatalystExceptionHandler(false);
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors("integrationcors");
			app.UseAuthorization();
			app.UseEndpoints(endpoints => endpoints.MapControllers());
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"Catalyst Test API v1");
				c.RoutePrefix = string.Empty;
			});
		}
	}
}
