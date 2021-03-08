using Microsoft.AspNetCore.Mvc;
using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst.TestApi
{
	[Route("env")]
	public class EnvController : BaseController
	{
		private readonly TestSettings _settings;
		public EnvController(TestSettings settings)
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
