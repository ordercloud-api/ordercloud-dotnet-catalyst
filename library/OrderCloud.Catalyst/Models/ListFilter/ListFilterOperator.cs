using System;
using System.Collections.Generic;
using System.Text;

namespace OrderCloud.Catalyst
{
    public class OperatorSymbol : Attribute
    {
        public OperatorSymbol(string symbol)
        {
            this.Symbol = symbol;
        }

        public string Symbol { get; set; }
    }

    public enum ListFilterOperator
    {
        [OperatorSymbol("=")]
        Equal,
        [OperatorSymbol(">")]
        GreaterThan,
        [OperatorSymbol("<")]
        LessThan,
        [OperatorSymbol(">=")]
        GreaterThanOrEqual,
        [OperatorSymbol("<=")]
        LessThanOrEqual,
        [OperatorSymbol("<>")]
        NotEqual
    }
}
