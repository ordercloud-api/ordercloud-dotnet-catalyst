using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using OrderCloud.SDK;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace OrderCloud.Catalyst.TestApi
{
	[Route("demo")]
	public class DemoController : BaseController
	{
		private static IUserContextProvider _provider;

		public DemoController(IUserContextProvider provider)
		{
			_provider = provider;
		}

		[HttpGet("shop"), OrderCloudUserAuth(ApiRole.Shopper)]
		public object Shop() {
			return "hello shopper!";
		}

		[HttpGet("admin"), OrderCloudUserAuth(ApiRole.OrderAdmin)]
		public object Admin() => "hello admin!";

		[HttpGet("either"), OrderCloudUserAuth("Shopper", "OrderAdmin")]
		public object Either() => "hello either!";

		[HttpGet("custom"), OrderCloudUserAuth("CustomRole")]
		public object CustomRole() => "hello custom!";

		[HttpGet("username"), OrderCloudUserAuth]
		public object Username() => $"hello {UserContext.Username}!";

		[HttpGet("anybody"), OrderCloudUserAuth]
		public object Anybody() => "hello anybody!";

		[HttpGet("anon")]
		public object Anon() => "hello anon!";

		[HttpPost("token/{token}")]
		public async Task<SimplifiedUser> Token(string token)
		{
			var user = await _provider.VerifyAsync(token);
			return new SimplifiedUser() { 
				AvailableRoles = user.AvailableRoles.ToList(),
				Username = user.Username, 
				TokenClientID = user.TokenClientID 
			};
		}

		[HttpGet("notfound")]
		public object ThingNotFound() => throw new NotFoundException();

		[HttpGet("internalerror")]
		public object InternalError() => (new string[] { })[10];

		[HttpPost("modelvalidation")]
		public ExampleModel ModelValidation(ExampleModel model) => model;

		[HttpGet("listargs")]
		public IListArgs DeserializeListArgs(ListArgs<ExampleModel> args) => args;

		[HttpGet("listargspageonly")]
		public ListArgsPageOnly DeserializeListArgsPageOnly(ListArgsPageOnly args) => args;
	}

	public class SimplifiedUser
	{
		public List<string> AvailableRoles { get; set; }
		public string Username { get; set; }
		public string TokenClientID { get; set; }

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
