using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using System;

namespace OrderCloud.Catalyst
{
	public static class CatalystConfigureWebApiServices
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services)
		{ 
			services.AddControllers()
			.ConfigureApiBehaviorOptions(o =>
			{
				o.SuppressModelStateInvalidFilter = true;
			}).AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
			});
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
			services.AddMvc(o =>
			{
				o.Filters.Add(new ValidateModelAttribute());
				o.EnableEndpointRouting = false;
			});
			services.AddCors(o => o.AddPolicy("integrationcors",
				builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

			return services;
		}

		/// <summary>
		/// Chain to IServiceCollection (typically in Startup.ConfigureServices) to enable authenticating by passing a valid
		/// OrderCloud access token in the Authorization header. Add [OrderCloudUserAuth] attribute to specific controllers or actions
		/// where this should be enforced. Typical use case is custom endpoints for front-end user apps.
		/// </summary>
		public static IServiceCollection AddOrderCloudUserAuth(this IServiceCollection services)
		{
			services
				.AddHttpContextAccessor()
				.AddSingleton<OrderCloudUserAuthProvider>()
				.AddSingleton<ISimpleCache, LazyCacheService>() // Can override by registering own implmentation
				.AddAuthentication()
				.AddScheme<OrderCloudUserAuthOptions, OrderCloudUserAuthHandler>("OrderCloudUser", null);
			return services;
		}

		/// <summary>
		/// Chain to IServiceCollection (typically in Startup.ConfigureServices) to enable validation of incoming webhooks.
		/// </summary>
		public static IServiceCollection AddOrderCloudWebhookAuth(this IServiceCollection services, Action<OrderCloudWebhookAuthOptions> configureOptions)
		{
			services.AddAuthentication()
				.AddScheme<OrderCloudWebhookAuthOptions, OrderCloudWebhookAuthHandler>("OrderCloudWebhook", null, configureOptions);
			return services;
		}
	}
}
