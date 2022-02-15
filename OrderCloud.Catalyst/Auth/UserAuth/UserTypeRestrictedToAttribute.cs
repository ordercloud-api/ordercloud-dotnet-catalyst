using OrderCloud.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// Apply to actions. Restricts the commerce roles (Buyer, Supplier, Seller) that can access. Only has an effect when paired with the OrderCloudUserAuth attribute. 
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class UserTypeRestrictedToAttribute : Attribute
	{
		public List<CommerceRole> AllowedUserTypes { get; set; }
		public UserTypeRestrictedToAttribute(params CommerceRole[] allowedUserTypes)
		{
			AllowedUserTypes = allowedUserTypes.ToList();
		}
	}
}
