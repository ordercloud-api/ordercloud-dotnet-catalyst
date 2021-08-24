using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderCloud.SDK;
using OrderCloud.Catalyst;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace OrderCloud.Catalyst.TestApi
{
	[Route("demo")]
	public class DemoController : CatalystController
	{
		private readonly RequestAuthenticationService _tokenProvider;
		private readonly ExampleCommand _exampleCommand;
		private readonly IOrderCloudClient _oc; 

		public DemoController(RequestAuthenticationService tokenProvider, ExampleCommand exampleCommand, IOrderCloudClient oc)
		{
			_tokenProvider = tokenProvider;
			_exampleCommand = exampleCommand;
			_oc = oc;
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

		[HttpGet("supplier"), OrderCloudUserAuth, UserTypeRestrictedTo(CommerceRole.Supplier)]
		public object Supplier() => "hello supplier!";

		[HttpGet("supplierorseller"), OrderCloudUserAuth, UserTypeRestrictedTo(CommerceRole.Supplier, CommerceRole.Seller)]
		public object SupplierOrSeller() => "hello supplier or seller!";

		[HttpGet("supplieradmin"), OrderCloudUserAuth(ApiRole.SupplierAdmin), UserTypeRestrictedTo(CommerceRole.Supplier)]
		public object SupplierAdmin() => "hello supplier admin!";

		// User type doesn't apply without OrderCloudUserAuth
		[HttpGet("unrestricted"), UserTypeRestrictedTo(CommerceRole.Supplier)]
		public object Unrestricted() => "hello anyone!";

		[HttpGet("anybody"), OrderCloudUserAuth]
		public object Anybody() => "hello anybody!";

		[HttpGet("anon")]
		public object Anon() => "hello anon!";

		[HttpGet("listall")]
		public async Task<object> ListALl()
		{
			var list = await _oc.Products.ListAllAsync();
			var c = list.Count;
			return list;
		}

		[HttpGet("usercontext"), OrderCloudUserAuth]
		public SimplifiedUser Username()
		{
			return new SimplifiedUser()
			{
				AvailableRoles = UserContext.Roles.ToList(),
				Username = UserContext.Username,
				TokenClientID = UserContext.ClientID
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
		public ExampleModel ModelValidation([FromBody] ExampleModel model) => model;

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
		[Required(ErrorMessage = "This field is required, please try again.")]
		public string RequiredField { get; set; }

		[StringLength(25, MinimumLength = 10, ErrorMessage = "This value must be at least 10 characters and no more than 25 characters.")]
		public string BoundedString { get; set; }

		[Range(0.01, 100.00, ErrorMessage = "This value must be between {1} and {2}.")]
		public decimal BoundedDecimal { get; set; }

		[Range(1, 100, ErrorMessage = "This value must be between {1} and {2}.")]
		public int BoundedInteger { get; set; }

		[EmailAddress(ErrorMessage = "The email address provided is not valid.")]
		public string Email { get; set; }

		[CreditCard(ErrorMessage = "The credit card number provided is not valid.")]
		public string CreditCardNumber { get; set; }

		// Regex Example - Alphanumeric, no special characters or spaces
		[RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Invalid characters (special characters and spaces are not allowed).")]
		public string RegexExample { get; set; }
	}

	public class MyConfigData
	{
		public string Foo { get; set; }
		public int Bar { get; set; }
	}
}
