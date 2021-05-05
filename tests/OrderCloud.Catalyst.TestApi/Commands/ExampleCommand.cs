using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.TestApi
{
	public class ExampleCommand
	{
		private readonly OrderCloudUserAuthProvider _userProvider;
		public ExampleCommand(OrderCloudUserAuthProvider userProvider)
		{
			_userProvider = userProvider;
		}

		public string GetClientID()
		{
			return _userProvider.GetToken().ClientID;
		}
	}
}
