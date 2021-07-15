using OrderCloud.SDK;
using System;
using System.Collections;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;

namespace OrderCloud.Catalyst.Tests
{
	// OrderCloudException from OrderCloud.SDK has no public constructor.
	// See https://github.com/ordercloud-api/ordercloud-dotnet-sdk/issues/63
	// This factory is a work-around
	public class OrderCloudExceptionFactory
	{
		private static Type _ocExceptionType = typeof(OrderCloudException);
		private static Type _exceptionType = typeof(Exception);
		private static FieldInfo _httpStatusField = _ocExceptionType.GetField("<HttpStatus>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo _errorsField = _ocExceptionType.GetField("<Errors>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo _messageField = _exceptionType.GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo _dataField = _exceptionType.GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic);

		public static OrderCloudException Create(HttpStatusCode status, string message = null, ApiError[] errors = null, IDictionary data = null)
		{
			var ex = (OrderCloudException)FormatterServices.GetUninitializedObject(typeof(OrderCloudException));
			_httpStatusField.SetValue(ex, status);
			_messageField.SetValue(ex, message);
			_errorsField.SetValue(ex, errors);
			_dataField.SetValue(ex, data);
			return ex;
		}
	}
}
