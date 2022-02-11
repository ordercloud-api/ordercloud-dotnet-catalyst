using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst.Tax.Avalara
{ 
	public class AvalaraFetchResult<T>
	{
		public int @recordsetCount { get; set; }
		public List<T> value { get; set; } = new List<T> { };
	}
}
