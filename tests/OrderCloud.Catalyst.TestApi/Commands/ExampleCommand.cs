using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.TestApi
{
	public class ExampleCommand
	{
		private readonly OrderCloudUserAuthProvider _userAuth;
		public ExampleCommand(OrderCloudUserAuthProvider userAuth)
		{
			_userAuth = userAuth;
		}

		public string GetClientID()
		{
			return _userAuth.GetToken().ClientID;
		}
	}
}
