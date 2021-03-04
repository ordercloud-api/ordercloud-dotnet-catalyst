using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.DemoWebApi.Tests
{
	[TestFixture]
	public class GeneralErrorTests
	{
		[Test]
		public async Task not_found_error()
		{
			var result = await TestFramework.Client.GetResponseAsync("demo/notfound");
			result.ShouldBeApiError("NotFound", 404, "Not found.");
		}

		[Test]
		public async Task internal_server_error()
		{
			var result = await TestFramework.Client.GetResponseAsync("demo/internalerror");
			result.ShouldBeApiError("InternalServerError", 500, "Unknown error has occured.");
		}
	}
}
