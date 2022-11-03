using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	public class OpenIDConnectIEPayload
	{
		public User ExistingUser { get; set; }
		public OpenIdConnect OpenIdConnect { get; set; }
		public OpenIdConnectTokenResponse TokenResponse { get; set; }
		public string Environment { get; set; }
		public string OrderCloudAccessToken { get; set; }
		public dynamic ConfigData { get; set; }
	}

	public class OpenIDConnectIEPayload<TConfigData, TUser> : OpenIDConnectIEPayload
		where TUser: User
	{
		public new TUser ExistingUser { get; set; }
		public new TConfigData ConfigData { get; set; }
	}

	public class OpenIdConnectTokenResponse
	{
		public string id_token { get; set; }
		public string access_token { get; set; }
	}

	public class OpenIdConnectCreateUserResponse
	{
		public string Username { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class OpenIdConnectSyncUserResponse
	{
		public string ErrorMessage { get; set; }
	}
}