using OrderCloud.SDK;

public class OpenIDConnectUserPayload<TConfig>
{
	public User ExistingUser { get; set; }
	public OpenIdConnect OpenIdConnect { get; set; }
	public OpenIdConnectTokenResponse TokenResponse { get; set; }
	public string Environment { get; set; }
	public string OrderCloudAccessToken { get; set; }
	public TConfig ConfigData { get; set; }
}

public class OpenIDConnectUserPayload<TConfig, TUser> { 
	public User<TUser> ExistingUser { get; set; }
	public OpenIdConnect OpenIdConnect { get; set; }
	public OpenIdConnectTokenResponse TokenResponse { get; set; }
	public string Environment { get; set; }
	public string OrderCloudAccessToken { get; set; }
	public TConfig ConfigData { get; set; }
}

public class OpenIdConnectTokenResponse
{
	public string id_token { get; set; }
	public string access_token { get; set; }
}