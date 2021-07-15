using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OrderCloud.Catalyst
{ 
    public interface IListArgs
    {
        string Search { get; set; }
        string SearchOn { get; set; }
        IList<string> SortBy { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        IList<ListFilter> Filters { get; set; }
		void ValidateAndNormalize();
        string ToFilterString();
		string ToSortString();
    }

	[ModelBinder(typeof(ListArgsModelBinder))]
	public class ListArgs<T> : IListArgs
	{
		public ListArgs()
		{
			Page = 1;
			PageSize = 20;
		}
		public string SearchOn { get; set; }
		public string Search { get; set; }
		public IList<string> SortBy { get; set; } = new List<string>();
		public int Page { get; set; }
		public int PageSize { get; set; }
		public IList<ListFilter> Filters { get; set; } = new List<ListFilter>();

		public void ValidateAndNormalize()
		{
			var newSortBy = new List<string>();
			foreach (var s in this.SortBy)
			{
				if (newSortBy.Contains(s, StringComparer.InvariantCultureIgnoreCase))
					continue;
				var desc = s.StartsWith("!");
				var name = s.TrimStart('!');
				var prop = FindSortableProp(typeof(T), name);
				newSortBy.Add(desc ? "!" + prop : prop);
			}
			this.SortBy = newSortBy;
		}

		private static string FindSortableProp(Type type, string path)
		{
			if (path.StartsWith("xp."))
				return path;

			var queue = new Queue<string>(path.Split('.'));
			var prop = type.GetProperty(queue.Dequeue(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
			if (prop == null)
			{
				throw new InvalidPropertyException(type, path);
			}
			//TODO: evaluate this requirement for reference sake
			//Require.That(prop.HasAttribute<SortableAttribute>(), ErrorCodes.List.InvalidSortProperty, new InvalidPropertyError(type, path));
			var result = prop?.Name;
			if (queue.Any())
				result += "." + FindSortableProp(prop.PropertyType, string.Join(".", queue));
			return result;
		}

		public string ToFilterString()
		{
			var filters = Filters.Select(t => $"{t.PropertyName}={t.FilterExpression.Replace("&", "%26")}");
			return string.Join("&", filters);
		}

		public string ToSortString()
		{
			return string.Join(",", SortBy);
		}
	}
}
