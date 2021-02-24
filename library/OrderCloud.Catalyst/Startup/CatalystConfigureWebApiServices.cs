using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace OrderCloud.Catalyst
{
	public static class CatalystConfigureWebApiServices
	{
		public static IServiceCollection ConfigureServices<TSettings>(this IServiceCollection services, TSettings settings, OrderCloudWebhookAuthOptions webhookConfig = null)
			where TSettings : class, new()
		{
			services.AddControllers()
			.ConfigureApiBehaviorOptions(o =>
			{
				o.SuppressModelStateInvalidFilter = true;
			}).AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
			});

			services.AddSingleton(settings ?? new TSettings());
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
			services.AddMvc(o =>
			{
				o.Filters.Add(new ValidateModelAttribute());
				o.EnableEndpointRouting = false;
			});
			services.AddCors(o => o.AddPolicy("integrationcors",
				builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

			services
				.AddAuthentication()
				.AddScheme<OrderCloudUserAuthOptions, OrderCloudUserAuthHandler<TSettings>>("OrderCloudUser", null)
				.AddScheme<OrderCloudWebhookAuthOptions, OrderCloudWebhookAuthHandler>("OrderCloudWebhook", null, opts => opts.HashKey = webhookConfig.HashKey);

			return services;
		}
	}
}
