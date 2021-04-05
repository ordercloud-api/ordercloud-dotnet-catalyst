using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class CatalystApplicationBuilder
	{
		public static IApplicationBuilder DefaultCatalystAppBuilder(this IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCatalystExceptionHandler();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors("integrationcors");
			app.UseAuthorization();
			app.UseEndpoints(endpoints => endpoints.MapControllers());

			return app;
		}
	}
}
