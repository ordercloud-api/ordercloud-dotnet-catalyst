using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderCloud.Catalyst
{
	[EnableCors("marketplacecors")]
	public class BaseController : Controller
	{
		public VerifiedUserContext Context;

		public BaseController() { }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			Context = new VerifiedUserContext(User);
			base.OnActionExecuting(context);
		}
	}
}
