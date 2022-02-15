using Flurl.Http;
using NUnit.Framework;
using OrderCloud.Catalyst.TestApi;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.Tests
{
    [TestFixture]
    public class DataAnnotationTests
    {
        private ExampleModel GetExampleModel()
        {
            return new ExampleModel()
            {
                RequiredField = "RequiredValue",
                BoundedString = "BoundedStringValue",
                BoundedDecimal = 10M,
                BoundedInteger = 25,
                Email = "example@email.com",
                CreditCardNumber = "4111111111111111",
                RegexExample = "RegexExampleValue123"
            };
        }

        private IFlurlRequest GetModelValidationRoute()
        {
            return TestFramework.Client.Request($"demo/modelvalidation");
        }
        
        [Test]
        public async Task valid_example_triggers_no_errors()
        {
            var modelExample = GetExampleModel();
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample);
            Assert.AreEqual(200, response.StatusCode);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task required_field_triggers_error(string requiredValue)
        {
            var modelExample = GetExampleModel();
            modelExample.RequiredField = requiredValue;
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample)
                .ReceiveJson();
            var errors = response.Errors;
            var error = response.Errors[0];
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("RequiredField", error.ErrorCode);
            Assert.AreEqual("This field is required, please try again.", error.Message);
        }

        [TestCase("")]
        [TestCase("abcdefghi")]
        [TestCase("abcdefghijabcdefghijabcdef")]
        public async Task bounded_string_field_triggers_error(string boundedStringValue)
        {
            var modelExample = GetExampleModel();
            modelExample.BoundedString = boundedStringValue;
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample)
                .ReceiveJson();
            var errors = response.Errors;
            var error = response.Errors[0];
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("BoundedString", error.ErrorCode);
            Assert.AreEqual("This value must be at least 10 characters and no more than 25 characters.", error.Message);
        }

        [TestCase(null)]
        [TestCase("0.00")]
        [TestCase("100.01")]
        [TestCase("-1.11")]
        public async Task bounded_decimal_field_triggers_error(decimal boundedDecimalValue)
        {
            var modelExample = GetExampleModel();
            modelExample.BoundedDecimal = boundedDecimalValue;
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample)
                .ReceiveJson();
            var errors = response.Errors;
            var error = response.Errors[0];
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("BoundedDecimal", error.ErrorCode);
            Assert.AreEqual("This value must be between 0.01 and 100.", error.Message);
        }

        [TestCase(null)]
        [TestCase("0")]
        [TestCase("101")]
        [TestCase("-1")]
        public async Task bounded_integer_field_triggers_error(int boundedIntegerValue)
        {
            var modelExample = GetExampleModel();
            modelExample.BoundedInteger = boundedIntegerValue;
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample)
                .ReceiveJson();
            var errors = response.Errors;
            var error = response.Errors[0];
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("BoundedInteger", error.ErrorCode);
            Assert.AreEqual("This value must be between 1 and 100.", error.Message);
        }

        [TestCase("612-555-1234")]
        [TestCase("noemail")]
        [TestCase("example at email dot com")]
        public async Task email_field_triggers_error(string email)
        {
            var modelExample = GetExampleModel();
            modelExample.Email = email;
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample)
                .ReceiveJson();
            var errors = response.Errors;
            var error = response.Errors[0];
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Email", error.ErrorCode);
            Assert.AreEqual("The email address provided is not valid.", error.Message);
        }

        [TestCase("7777")]
        [TestCase("anycreditcardnumber")]
        [TestCase("414798765432198765")]
        [TestCase("3792123456789012")]
        [TestCase("60111234567890121")]
        [TestCase("414798765432198")]
        [TestCase("37921234567890")]
        [TestCase("601112345678901")]
        public async Task credit_card_field_triggers_error(string creditCardNumber)
        {
            var modelExample = GetExampleModel();
            modelExample.CreditCardNumber = creditCardNumber;
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample)
                .ReceiveJson();
            var errors = response.Errors;
            var error = response.Errors[0];
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("CreditCardNumber", error.ErrorCode);
            Assert.AreEqual("The credit card number provided is not valid.", error.Message);
        }

        [TestCase("hellowor!d")]
        [TestCase("hello world")]
        public async Task regex_example_field_triggers_error(string regexExampleValue)
        {
            var modelExample = GetExampleModel();
            modelExample.RegexExample = regexExampleValue;
            var url = GetModelValidationRoute();
            var response = await url
                .PostJsonAsync(modelExample)
                .ReceiveJson();
            var errors = response.Errors;
            var error = response.Errors[0];
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("RegexExample", error.ErrorCode);
            Assert.AreEqual("Invalid characters (special characters and spaces are not allowed).", error.Message);
        }
    }
}
