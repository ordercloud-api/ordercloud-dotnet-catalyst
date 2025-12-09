using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace OrderCloud.Catalyst
{
	[EnableCors("integrationcors")]
	[Produces("application/json")]
	public class CatalystController : Controller
	{
		/// <summary>
		/// Will be null unless [OrderCloudUserAuth] is added to the route.
		/// </summary>
		public DecodedToken UserContext { get; private set; }

		/// <summary>
		/// Will be null unless [OrderCloudUserInfoAuth] is added to the route.
		/// </summary>
		public DecodedUserInfoToken UserInfoContext { get; private set; }

		public CatalystController() {}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var accessToken = User.Claims.FirstOrDefault(claim => claim.Type == "AccessToken")?.Value;
			if (accessToken != null)
			{
				UserContext = new DecodedToken(accessToken);
			}

			var userInfoToken = User.Claims.FirstOrDefault(claim => claim.Type == "UserInfoToken")?.Value;
			if (userInfoToken != null)
			{
                UserInfoContext = new DecodedUserInfoToken(userInfoToken);
			}
			base.OnActionExecuting(context);
		}
	}
}
