using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Catalyst
{ 
    [ModelBinder(typeof(ListArgsPageOnlyModelBinder))]
    public class ListArgsPageOnly
    {
        public ListArgsPageOnly()
        {
            Page = 1;
            PageSize = 20;
        }

        public ListArgsPageOnly(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
