using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class ApiError
	{
		[JsonIgnore]
		public HttpStatusCode StatusCode { get; set; }
		public string ErrorCode { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }
	}

	public class ApiErrorList : List<ApiError>
	{
		public void Add<TData>(ErrorCode<TData> errorCode, TData data)
		{
			Add(new ApiError
			{
				ErrorCode = errorCode.Code,
				StatusCode = (HttpStatusCode)errorCode.HttpStatus,
				Message = errorCode.DefaultMessage,
				Data = data
			});
		}

		public void AddIf<TData>(bool condition, ErrorCode<TData> errorCode, TData data)
		{
			if (condition) Add(errorCode, data);
		}
		public void AddIf(bool condition, ErrorCode errorCode)
		{
			if (condition)
				Add(new ApiError
				{
					ErrorCode = errorCode.Code,
					StatusCode = (HttpStatusCode)errorCode.HttpStatus,
					Message = errorCode.DefaultMessage
				});
		}

		public void ThrowIfAny()
		{
			if (this.Any()) throw new ApiErrorException(this);
		}
	}
}
