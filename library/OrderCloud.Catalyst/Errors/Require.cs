using System;

namespace OrderCloud.Catalyst
{
    public static class Require
    {
        /// <summary>
        /// Throws an error if condition is false. HTTP status is defined in the ErrorCode object. 
        /// Example usage: Require.That(check == true, new NotFoundException()); 
        /// See ErrorCodes.txt for error definitions 
        /// </summary>
        public static void That(bool condition, Exception ex)
        {
            if (!condition)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Throws an error if condition is false. Error data model is built lazily. HTTP status is defined in the ErrorCode object. 
        /// Example usage: Require.That(check == true, () => new CatalystBaseException("NotFound", $"Not found.", null, 404)); 
        /// </summary>
        public static void That(bool condition, Func<Exception> buildException)
        {
            if (!condition)
            {
                throw buildException();
            }
        }
    }
}
