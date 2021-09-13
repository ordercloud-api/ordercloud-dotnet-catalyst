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
			await result.ShouldHaveFirstApiError("NotFound", 404, "Not found.");
		}

		[Test]
		public async Task internal_server_error()
		{
			var result = await TestFramework.Client.Request("demo/internalerror").GetAsync();
			await result.ShouldHaveFirstApiError("InternalServerError", 500, "Unknown error has occured.");
		}

        [Test]
        [AutoData]
        public void catalsyst_exception_should_have_message(ApiError error)
        {
            var ex1 = new CatalystBaseException(error);

            Assert.AreEqual(error.Message, ex1.Errors[0].Message);
        }

        [Test]
		[AutoData]
		public void require_that_does_not_throw_catalyst_exception(ApiError error)
        {
			Assert.DoesNotThrow(() => Require.That(true, new CatalystBaseException(error)));
		}

		[Test]
		[AutoData]
		public void require_that_throws_catalyst_exception(ApiError error)
        {
            var ex1 = Assert.Throws<CatalystBaseException>(() => Require.That(false, new CatalystBaseException(error)));
			// Assert on Exception 1
			Assert.AreEqual(error.Message, ex1.Message);
			Assert.AreEqual(error.ErrorCode, ex1.Errors[0].ErrorCode);
			Assert.AreEqual(error.Data, ex1.Errors[0].Data);
		}
	}
}
