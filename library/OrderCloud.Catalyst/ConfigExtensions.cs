using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace OrderCloud.Catalyst
{
	public static class ConfigExtensions
	{
		/// <summary>
		/// Register all services in a given assembly and (optionally) namespace by naming convention: IMyService -> MyService
		/// </summary>
		/// <param name="asm">Assembly to scan for interfaces and implementations.</param>
		/// <param name="namespace">Namespace to scan for interfaces (otional).</param>
		public static IServiceCollection AddServicesByConvention(this IServiceCollection services, Assembly asm, string @namespace = null) {
			var mappings =
				from impl in asm.GetTypes()
				let iface = impl.GetInterface($"I{impl.Name}")
				where iface != null
				where @namespace == null || iface.Namespace == @namespace
				select new { iface, impl };

			foreach (var m in mappings)
				services.AddTransient(m.iface, m.impl);

			return services;
		}

		/// <summary>
		/// Chain to AddMvc() (typically in Startup.ConfigureServices) if you want to respond to multiple webhooks from a single URL.
		/// This allows you to add the same [Route] attribute to several action methods, and it will will choose the correct one
		/// based on payload type. For example, if you have an action method with a [FromBody] parameter of type WebhookPayloads.Orders.Submit,
		/// then order submit webhooks will be correctly routed to this method.
		/// </summary>
		//public static IMvcBuilder DisambiguateWebhooks(this IMvcBuilder builder) {
		//	builder.Services.AddSingleton<IActionSelector, WebhookActionSelector>();
		//	return builder;
		//}

		/// <summary>
		/// Binds your appsettings.json file (or other config source, such as App Settings in the Azure portal) to the AppSettings class
		/// so values can be accessed in a strongly typed manner. Call in your Program.cs off of WebHost.CreateDefaultBuilder(args).
		/// If called before UseStartup, then AppSettings can be injected into your Startup class.
		/// </summary>
		public static IWebHostBuilder UseAppSettings<TAppSettings>(this IWebHostBuilder hostBuilder) where TAppSettings : class, new() {
			return hostBuilder.ConfigureServices((ctx, services) => {
				// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options
				services.Configure<TAppSettings>(ctx.Configuration);

				// Breaks from the Options pattern (link above) by allowing AppSettings to be injected directly
				// into services, rather than injecting IOptions<AppSettings>.
				services.AddTransient(sp => sp.GetService<IOptionsSnapshot<TAppSettings>>().Value);
			});
		}

		/// <summary>
		/// Chain to services.AddAuthentication() (typically in Startup.ConfigureServices) to enable authenticating by passing a valid
		/// OrderCloud access token in the Authorization header. Add [OrderCloudUserAuth] attribute to specific controllers or actions
		/// where this should be enforced. Typical use case is custom endpoints for front-end user apps.
		/// </summary>
		public static AuthenticationBuilder AddOrderCloudUser(this IServiceCollection services) {
			return services
				.AddAuthentication()
				.AddScheme<OrderCloudUserAuthOptions, OrderCloudUserAuthHandler>("OrderCloudUser", null);
		}

		/// <summary>
		/// Call inside of services.AddAuthorization(...) (typically in Startup.ConfigureServices) to enable validation of incoming webhooks.
		/// </summary>
		public static AuthenticationBuilder AddOrderCloudWebhooks(this IServiceCollection services, Action<OrderCloudWebhookAuthOptions> configureOptions) {
			return services
				.AddAuthentication()
				.AddScheme<OrderCloudWebhookAuthOptions, OrderCloudWebhookAuthHandler>("OrderCloudWebhook", null, configureOptions);
		}
	}
}
