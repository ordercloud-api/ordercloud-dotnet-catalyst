﻿using System;
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
            IList<ApiError> body;
            int status = (int) HttpStatusCode.InternalServerError; // 500 if unexpected

			switch (ex)
            {
                case CatalystBaseException intException:
                    status = intException.HttpStatus;
                    body = intException.Errors;
                    break;
                case OrderCloudException ocException:
                    var isClientSerializationError = ocException?.Errors == null;
					if (isClientSerializationError) // 500 error with interal error message, this is a bug in the client API
					{
                        body = new List<ApiError>() {
                            new ApiError() {
                                Data = ocException?.InnerException?.InnerException?.Message, // "Response could not be deserialized...."
                                ErrorCode = "OrderCloudSDKDeserializationError",
                                Message = ocException.Message // Specific path and value of error
                            }
                        };
					}
					else // forward status code and errors from OrderCloud API
					{
						status = (int) ocException.HttpStatus;
						body = ocException.Errors;
					}
                    break;
                default:
                    // this is only to be hit IF it's not handled properly in the stack. It's considered a bug if ever hits this. that's why it's a 500
                    body = new List<ApiError>() {
                        new ApiError() {
                            Data = ex.Message,
                            ErrorCode = "InternalServerError",
                            Message = $"Unknown error has occured."
                        }
                    };
                    break;
            }

            context.Response.StatusCode = status;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorList(body)));
        }
    }

    public class ErrorList 
    {
        public IList<ApiError> Errors { get; set; }
        public ErrorList(IList<ApiError> errors)
        {
            Errors = errors;
        }
    }
}
