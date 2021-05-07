using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.TestApi
{
	public class ExampleCommand
	{
		private readonly RequestAuthenticationService _userProvider;
		public ExampleCommand(RequestAuthenticationService userProvider)
		{
			_userProvider = userProvider;
		}

		public string GetClientID()
		{
			return _userProvider.GetUserContext().ClientID;
		}
	}
}
