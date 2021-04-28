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
        public void catalsyst_exception_should_have_message(ErrorCode errorCode, string message)
        {
            var ex1 = new CatalystBaseException(new ApiError() { ErrorCode = errorCode.Code, Message = message });
            var ex2 = new CatalystBaseException(errorCode);

            Assert.AreEqual(message, ex1.Errors[0].Message);
            Assert.AreEqual(errorCode.DefaultMessage, ex2.Errors[0].Message);
            Assert.AreEqual(errorCode.Code, ex1.Errors[0].ErrorCode);
            Assert.AreEqual(errorCode.Code, ex2.Errors[0].ErrorCode);
        }

        [Test]
		[AutoData]
		public void require_that_does_not_throw_catalyst_exception(ErrorCode errorCode)
        {
			Assert.DoesNotThrow(() => Require.That(true, null));
			Assert.DoesNotThrow(() => Require.That(true, errorCode));
			Assert.DoesNotThrow(() => Require.That(true, null));
			Assert.DoesNotThrow(() => Require.That(true, null));
		}

		[Test]
		[AutoData]
		public void require_that_throws_catalyst_exception(ErrorCode<dynamic> errorCode, object data)
        {
            var lazyData = new { Lazily = "Built" };
            var ex1 = Assert.Throws<CatalystBaseException>(() => Require.That(false, errorCode));
			var ex2 = Assert.Throws<CatalystBaseException>(() => Require.That(false, errorCode, data));
			var ex3 = Assert.Throws<CatalystBaseException>(() => Require.That(false, errorCode, () => new { Lazily = "Built" }));
			// Assert on Exception 1
			Assert.AreEqual(errorCode.DefaultMessage, ex1.Message);
			Assert.AreEqual(errorCode.DefaultMessage, ex1.Errors[0].Message);
			Assert.AreEqual(errorCode.Code, ex1.Errors[0].ErrorCode);
			Assert.AreEqual(null, ex1.Errors[0].Data);
			// Assert on Exception 2
			Assert.AreEqual(errorCode.DefaultMessage, ex2.Message);
			Assert.AreEqual(errorCode.DefaultMessage, ex2.Errors[0].Message);
			Assert.AreEqual(errorCode.Code, ex2.Errors[0].ErrorCode);
			Assert.AreEqual(data, ex2.Errors[0].Data);
			// Assert on Exception 3
			Assert.AreEqual(errorCode.DefaultMessage, ex3.Message);
			Assert.AreEqual(errorCode.DefaultMessage, ex3.Errors[0].Message);
			Assert.AreEqual(errorCode.Code, ex3.Errors[0].ErrorCode);
			Assert.AreEqual(lazyData, ex3.Errors[0].Data);
		}

		[Test]
		[AutoData]
		public void throw_multiple_errors(ErrorCode<dynamic> errorCode1, ErrorCode<dynamic> errorCode2)
        {
			var exceptionData1 = new { This = "That" };
			var exceptionData2 = new { That = "This" };
			var errors = new ApiErrorList();
			errors.Add<dynamic>(errorCode1, exceptionData1);
			errors.Add<dynamic>(errorCode2, exceptionData2);
			var ex = Assert.Throws<CatalystBaseException>(() => errors.ThrowIfAny());
			// Assert on errorCode.Code
			Assert.AreEqual(errorCode1.Code, ex.Errors[0].ErrorCode);
			Assert.AreEqual(errorCode2.Code, ex.Errors[1].ErrorCode);
			// Assert on errorCode.StatusCode
			Assert.AreEqual(errorCode1.HttpStatus, (int)ex.Errors[0].StatusCode);
			Assert.AreEqual(errorCode2.HttpStatus, (int)ex.Errors[1].StatusCode);
			// Assert on errorCode.Data
			Assert.AreEqual(exceptionData1, ex.Errors[0].Data);
			Assert.AreEqual(exceptionData2, ex.Errors[1].Data);
		}
	}
}
