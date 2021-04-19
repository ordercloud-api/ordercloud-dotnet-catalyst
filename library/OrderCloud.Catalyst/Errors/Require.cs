using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OrderCloud.Catalyst
{
    public static class Require
    {
        /// <summary>
        /// Throws an error if condition is false. HTTP status is defined in the ErrorCode object. 
        /// Example usage: Require.That(check == true, CatalystException.NotFound, SomeObject); 
        /// See ErrorCodes.txt for error definitions 
        /// </summary>
        public static void That<TModel>(bool condition, CatalystBaseException error, TModel model)
        {
            if (!condition)
            {
                throw new CatalystBaseException(error.ApiError.ErrorCode, error.ApiError.Message, model);
            }
        }

        /// <summary>
        /// Throws an error if condition is false. Error data model is built lazily. HTTP status is defined in the ErrorCode object. 
        /// Example usage: Require.That(check == true, CatalystException.NotFound, () => new { key = "value" }); 
        /// </summary>
        public static void That<TModel>(bool condition, CatalystBaseException error, Func<TModel> buildModel)
        {
            if (!condition)
            {
                throw new CatalystBaseException(error.ApiError.ErrorCode, error.ApiError.Message, buildModel());
            }
        }
        /// <summary>
        /// Overload for when you don't need to pass back an object
        /// </summary>
        public static void That(bool condition, CatalystBaseException error, object data = null)
        {
            if (!condition)
            {
                throw new CatalystBaseException(error.ApiError.ErrorCode, error.ApiError.Message, data);
            }
        }
    }
}
