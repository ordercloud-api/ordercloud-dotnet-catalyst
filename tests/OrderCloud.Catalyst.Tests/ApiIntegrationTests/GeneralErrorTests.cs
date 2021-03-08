using Flurl.Http;
using NUnit.Framework;
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
	}
}
