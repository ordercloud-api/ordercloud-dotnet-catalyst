using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderCloud.Catalyst
{
	[EnableCors("integrationcors")]
	[Produces("application/json")]
	public class BaseController : Controller
	{
		public VerifiedUserContext UserContext;

		public BaseController() { }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			UserContext = new VerifiedUserContext(User);
			base.OnActionExecuting(context);
		}
	}
}
