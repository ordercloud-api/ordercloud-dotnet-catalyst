using Microsoft.AspNetCore.Mvc;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
	/// <summary>
	/// ListArgs for Premium Search-enabled endpoints
	/// </summary>
	[ModelBinder(typeof(ListArgsModelBinder))]
	public class SearchArgs<T> : ListArgs<T>
	{
		public SearchType SearchType { get; set; } = SearchType.AnyTerm;
	}
}
