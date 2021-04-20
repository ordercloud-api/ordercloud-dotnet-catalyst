using AutoFixture.NUnit3;
using Flurl.Http;
using NUnit.Framework;
using OrderCloud.SDK;
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

		[Test]
		[AutoData]
		public void catalsyst_exception_should_have_message(string code, string message)
		{
			var ex1 = new CatalystBaseException(new ApiError() { ErrorCode = code, Message = message });
			var ex2 = new CatalystBaseException(code, message);

			Assert.AreEqual(message, ex1.Message);
			Assert.AreEqual(message, ex2.Message);
			Assert.AreEqual(code, ex1.ApiError.ErrorCode);
			Assert.AreEqual(code, ex2.ApiError.ErrorCode);
		}

		[Test]
		public void require_that_does_not_throw_catalyst_exception()
        {
			Assert.DoesNotThrow(() => Require.That(true, new CatalystBaseException("code", "message")));
		}

		[Test]
		[AutoData]
		public void require_that_throws_catalyst_exception(string code, string message)
        {
			var data = new { key = "value" };
			var ex1 = Assert.Throws<CatalystBaseException>(() => Require.That(false, new CatalystBaseException(code, message)));
			var ex2 = Assert.Throws<CatalystBaseException>(() => Require.That(false, new CatalystBaseException(new ApiError() { ErrorCode = code, Message = message })));
			var ex3 = Assert.Throws<CatalystBaseException>(() => Require.That(false, new CatalystBaseException(new ApiError() { ErrorCode = code, Message = message }), data));
			var ex4 = Assert.Throws<CatalystBaseException>(() => Require.That(false, new CatalystBaseException(code, message), () => data));
			Assert.AreEqual(message, ex1.Message);
			Assert.AreEqual(message, ex2.Message);
			Assert.AreEqual(message, ex3.Message);
			Assert.AreEqual(message, ex4.Message);
			Assert.AreEqual(code, ex1.ApiError.ErrorCode);
			Assert.AreEqual(code, ex2.ApiError.ErrorCode);
			Assert.AreEqual(code, ex3.ApiError.ErrorCode);
			Assert.AreEqual(code, ex4.ApiError.ErrorCode);
			Assert.AreEqual(data, ex3.ApiError.Data);
			Assert.AreEqual(data, ex4.ApiError.Data);
		}
	}
}
