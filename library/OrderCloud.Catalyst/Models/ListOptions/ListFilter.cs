using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderCloud.Catalyst
{
    public enum ListFilterOperator { Equal, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual, NotEqual }

    public class ListFilterValue
    {
        public string Term { get; set; } = "";
        public ListFilterOperator Operator { get; set; } = ListFilterOperator.Equal;
        public IList<int> WildcardPositions { get; set; } = new List<int>();
        public bool HasWildcard => WildcardPositions.Any();
    }

    /// <summary>
    /// A filter applied to an Ordercloud list request. Represents one key/value query parameter in the raw http request. 
    /// </summary>
    public class ListFilter
    {
        /// <summary>
        /// The name of the property to filter on. The key of the query parameter.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// A raw expression of the filter logic. The value of the query parameter.
        /// </summary>
        public string FilterExpression { get; set; }

        /// <summary>
        /// A parsed expression of the filter logic. Broken into a list by separating on logical OR ( "|" ).
        /// </summary>
        public IList<ListFilterValue> FilterValues { get; set; } = new List<ListFilterValue>();

        internal ListFilter() { }

        /// <param name="propertyName">The name of the property to filter on. The key of the query parameter.</param>
		/// <param name="filterExpression">A raw expression of the filter logic. The value of the query parameter.</param>
        public ListFilter(string propertyName, string filterExpression)
        {
            PropertyName = propertyName;
            FilterExpression = filterExpression;
            FilterValues = ParseFilterExpression(filterExpression);
        }

        public static IList<ListFilterValue> ParseFilterExpression(string filterExpression)
		{
            var values = new List<ListFilterValue>();
            var value = new ListFilterValue();
            var escape = false;
            var negate = false;

            foreach (var c in filterExpression)
            {
                if (escape)
                {
                    value.Term += c.ToString();
                    escape = false;
                }
                else if (c == '\\')
                {
                    escape = true;
                }
                else if (c == '!' && value.Term == "")
                {
                    // 2 wrongs make a right
                    negate = !negate;
                    value.Operator = negate ? ListFilterOperator.NotEqual : ListFilterOperator.Equal;
                }
                else if (c == '>' && value.Term == "")
                {
                    value.Operator = negate ? ListFilterOperator.LessThanOrEqual : ListFilterOperator.GreaterThan;
                }
                else if (c == '<' && value.Term == "")
                {
                    value.Operator = negate ? ListFilterOperator.GreaterThanOrEqual : ListFilterOperator.LessThan;
                }
                else if (c == '=' && value.Operator == ListFilterOperator.GreaterThan && value.Term == "")
                {
                    value.Operator = ListFilterOperator.GreaterThanOrEqual;
                }
                else if (c == '=' && value.Operator == ListFilterOperator.LessThan && value.Term == "")
                {
                    value.Operator = ListFilterOperator.LessThanOrEqual;
                }
                else if (c == '=' && value.Term == "")
                {
                    value.Operator = ListFilterOperator.Equal;
                }
                else if (c == '*')
                {
                    value.WildcardPositions.Add(value.Term.Length);
                }
                else if (c == '|')
                {
                    values.Add(value);
                    value = new ListFilterValue();
                    escape = false;
                    negate = false;
                }
                else
                {
                    value.Term += c.ToString();
                }
            }
            values.Add(value);
            return values; 
        }
    }
}
