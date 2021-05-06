using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace OrderCloud.Catalyst
{
	[EnableCors("integrationcors")]
	[Produces("application/json")]
	public class CatalystController : Controller
	{
		/// <summary>
		/// Will be null unless [OrderCloudUserAuth] is added to the route.
		/// </summary>
		public UserContext UserContext { get; private set; }

		public CatalystController() {}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var token = User.Claims.FirstOrDefault(claim => claim.Type == "AccessToken")?.Value;
			if (token != null)
			{
				UserContext = new UserContext(token);
			}
			base.OnActionExecuting(context);
		}
	}
}
