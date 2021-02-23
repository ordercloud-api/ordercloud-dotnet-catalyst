using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using OrderCloud.SDK;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace SampleApp.WebApi.Controllers
{
	[Route("demo")]
	public class DemoController : BaseController
	{
		[HttpGet("shop"), OrderCloudUserAuth(ApiRole.Shopper)]
		public object Shop() => "hello shopper!";

		[HttpGet("admin"), OrderCloudUserAuth(ApiRole.OrderAdmin)]
		public object Admin() => "hello admin!";

		[HttpGet("either"), OrderCloudUserAuth(ApiRole.Shopper, ApiRole.OrderAdmin)]
		public object Either() => "hello either!";

		[HttpGet("username"), OrderCloudUserAuth]
		public object Username() => $"hello {Context.User.Username}!";

		[HttpGet("anybody"), OrderCloudUserAuth]
		public object Anybody() => "hello anybody!";

		[HttpGet("anon")]
		public object Anon() => "hello anon!";

		[HttpGet("notfound")]
		public object ThingNotFound() => throw new NotFoundException();

		[HttpGet("internalerror")]
		public object InternalError() => (new string[] { })[10];

		[HttpPost("modelvalidation")]
		public ExampleModel ModelValidation(ExampleModel model) => model;

        [HttpGet("listargs")]
        public IListArgs DeserializeListArgs(ListArgs<ExampleModel> args) => args;
    }




	public class ExampleModel
	{
		[Required]
		public string RequiredField { get; set; }
		[StringLength(100, MinimumLength = 10)]
		public string BoundedString { get; set; }
		[Range(0.01, 100.00, ErrorMessage = "100 is the max, friend")]
		public decimal BoundedDecimal { get; set; }
	}

	public class MyConfigData
	{
		public string Foo { get; set; }
		public int Bar { get; set; }
	}
}
