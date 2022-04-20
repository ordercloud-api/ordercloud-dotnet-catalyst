using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Tax.TaxJar
{
	public class TaxJarError
	{
		public string error { get; set; }
		public string detail { get; set; }
		public int status { get; set; }
	}
}
