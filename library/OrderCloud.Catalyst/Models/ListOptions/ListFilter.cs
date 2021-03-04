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

    public class ListFilter
    {
        public string PropertyName { get; set; }
        public string FilterExpression { get; set; }

        /// <summary>
        /// If multiple, OR them together
        /// </summary>
        public IList<ListFilterValue> FilterValues { get; set; } = new List<ListFilterValue>();

        public static ListFilter Parse(string name, string expression)
        {
            var result = new ListFilter { PropertyName = name, FilterExpression = expression };
            var value = new ListFilterValue();
            var escape = false;
            var negate = false;

            foreach (var c in expression)
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
                    result.FilterValues.Add(value);
					value = new ListFilterValue();
                    escape = false;
                    negate = false;
                }
                else
                {
                    value.Term += c.ToString();
                }
            }
            result.FilterValues.Add(value);
            return result;
        }
    }
}
