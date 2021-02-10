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

namespace OrderCloud.TestWebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services) {
			services.AddAuthentication()
				.AddOrderCloudUser(opts => opts.AddValidClientIDs("myclientid"))
				.AddOrderCloudWebhooks(opts => opts.HashKey = "myhashkey");

			services
				.AddMvc()
				.DisambiguateWebhooks()
				// don't mess with casing https://github.com/aspnet/Announcements/issues/194
				.AddJsonOptions(opts => opts.SerializerSettings.ContractResolver = new DefaultContractResolver());
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
