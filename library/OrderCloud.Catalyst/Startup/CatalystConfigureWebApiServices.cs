using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using ordercloud.integrations.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Startup
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

			//services.AddSingleton(settings ?? new TSettings());
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddMvc().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));
			services.AddMvc(o =>
			{
				o.Filters.Add(new ValidateModelAttribute());
				o.EnableEndpointRouting = false;
			});
			services.AddCors(o => o.AddPolicy("integrationcors",
				builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));

			//services.AddAuthenticationScheme<OrderCloudAuthOptions, OrderCloudAuthHandler<TSettings>>("OrderCloudIntegrations");
			return services;
		}
	}
}
