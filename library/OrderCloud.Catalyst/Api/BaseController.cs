using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderCloud.Catalyst
{
	[EnableCors("marketplacecors")]
	public class BaseController : Controller
	{
		public VerifiedOrderCloudUser VerifiedUser;

		public BaseController() { }

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			VerifiedUser = new VerifiedOrderCloudUser(User);
			base.OnActionExecuting(context);
		}
	}
}
