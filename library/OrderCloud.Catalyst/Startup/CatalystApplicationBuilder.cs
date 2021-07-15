using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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
