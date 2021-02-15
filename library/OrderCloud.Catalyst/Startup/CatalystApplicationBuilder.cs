using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public static class CatalystApplicationBuilder
	{
		public static IApplicationBuilder CreateApplicationBuilder(this IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseGlobalExceptionHandler();
			app.UseHttpsRedirection();
			app.UseCors("integrationcors");
			app.UseMvc();
			return app;
		}
	}
}
