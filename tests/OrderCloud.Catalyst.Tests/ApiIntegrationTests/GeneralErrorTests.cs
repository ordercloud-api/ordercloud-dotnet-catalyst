using Flurl.Http;
using NUnit.Framework;
using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
	[TestFixture]
	public class GeneralErrorTests
	{
		[Test]
		public async Task not_found_error()
		{
			var result = await TestFramework.Client.Request("demo/notfound").GetAsync();
			result.ShouldBeApiError("NotFound", 404, "Not found.");
		}

		[Test]
		public async Task internal_server_error()
		{
			var result = await TestFramework.Client.Request("demo/internalerror").GetAsync();
			result.ShouldBeApiError("InternalServerError", 500, "Unknown error has occured.");
		}

		[TestCase("something")]
		public async Task catalsyst_exception_should_have_message(string message)
		{
			var ex1 = new CatalystBaseException(new ApiError() { Message = message });
			var ex2 = new CatalystBaseException("code", message);

			Assert.AreEqual(message, ex1.Message);
			Assert.AreEqual(message, ex2.Message);
		}
	}
}
