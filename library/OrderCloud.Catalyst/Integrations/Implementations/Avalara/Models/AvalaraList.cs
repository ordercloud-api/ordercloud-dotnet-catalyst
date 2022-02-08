using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{ 
	public class AvalaraList<T>
	{
		public int @recordsetCount { get; set; }
		public List<T> value { get; set; } = new List<T> { };
	}
}
