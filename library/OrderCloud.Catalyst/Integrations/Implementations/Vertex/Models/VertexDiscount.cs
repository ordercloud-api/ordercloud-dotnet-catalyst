using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
	public class VertexDiscount
	{
		public decimal discountValue { get; set; }
		public VertexDiscountType discountType { get; set; }
		public string userDefinedDiscountCode { get; set; }
	}

	public enum VertexDiscountType { DiscountAmount, DiscountPercent }
}
