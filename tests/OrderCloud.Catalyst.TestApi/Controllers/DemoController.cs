using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCloud.SDK;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace OrderCloud.Catalyst.TestApi
{
	[Route("demo")]
	public class DemoController : CatalystController
	{
		private static UserAuthContextProvider _tokenProvider;
		private static ExampleCommand _exampleCommand;

		public DemoController(UserAuthContextProvider tokenProvider, ExampleCommand exampleCommand)
		{
			_tokenProvider = tokenProvider;
			_exampleCommand = exampleCommand;
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

		[HttpGet("anybody"), OrderCloudUserAuth]
		public object Anybody() => "hello anybody!";

		[HttpGet("anon")]
		public object Anon() => "hello anon!";

		[HttpGet("usercontext"), OrderCloudUserAuth]
		public SimplifiedUser Username()
		{
			return new SimplifiedUser()
			{
				AvailableRoles = UserAuthToken.Roles.ToList(),
				Username = UserAuthToken.Username,
				TokenClientID = UserAuthToken.ClientID
			};
		}

		[HttpGet("clientid"), OrderCloudUserAuth]
		public string GetClientID()
		{
			Thread.Sleep(1000); // pause for 1 sec
			return _exampleCommand.GetClientID();
		}

		[HttpPost("usercontext/{token}")]
		public async Task<SimplifiedUser> SetUserContext(string token)
		{
			var user = await _tokenProvider.VerifyTokenAsync(token);
			return new SimplifiedUser() { 
				AvailableRoles = user.Roles.ToList(),
				Username = user.Username, 
				TokenClientID = user.ClientID 
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

		[HttpGet("searchargs")]
		public SearchArgs<ExampleModel> DeserializeSearchArgs(SearchArgs<ExampleModel> args) => args;

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
