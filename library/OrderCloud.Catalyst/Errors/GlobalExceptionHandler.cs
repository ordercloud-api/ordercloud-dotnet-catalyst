using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public static class GlobalExceptionHandler 
    {
        public static IApplicationBuilder UseCatalystExceptionHandler(this IApplicationBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex);
                }
            });
            return builder;
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            const HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case CatalystBaseException intException:
                    context.Response.StatusCode = intException.HttpStatus;
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(intException.ApiError));
                case OrderCloudException ocException:
                    context.Response.StatusCode = (int) ocException.HttpStatus;
                    return context.Response.WriteAsync(JsonConvert.SerializeObject(ocException.Errors[0]));
            }

            // this is only to be hit IF it's not handled properly in the stack. It's considered a bug if ever hits this. that's why it's a 500
            var result = JsonConvert.SerializeObject(new ApiError()
            {
                Data = ex.Message,
                ErrorCode = code.ToString(),
                Message = $"Unknown error has occured."
            });
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
