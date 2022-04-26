using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Integrations.Tax.Avalara
{ 
	public class AvalaraFetchResult<T>
	{
		public int @recordsetCount { get; set; }
		public List<T> value { get; set; } = new List<T> { };
	}
}
