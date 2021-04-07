using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OrderCloud.Catalyst
{
	// TODO - is this still doing anything?
	[EnableCors("integrationcors")]
	[Produces("application/json")]
	public class BaseController : Controller
	{
		public BaseController() { }
	}
}
