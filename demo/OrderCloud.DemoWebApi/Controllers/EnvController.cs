using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.DemoWebApi.Controllers
{
	[Route("env")]
	public class EnvController : BaseController
	{
		private readonly AppSettings _settings;
		public EnvController(AppSettings settings)
		{
			_settings = settings;
		}

		[HttpGet("")]
		public object GetEnvironment()
		{
			return new { _settings.EnvironmentSettings.BuildNumber };
		}
	}
}
