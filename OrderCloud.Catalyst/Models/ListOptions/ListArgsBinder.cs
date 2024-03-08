using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrderCloud.SDK;

namespace OrderCloud.Catalyst
{
    public class ListArgsModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Require.That(bindingContext != null, new ArgumentNullException(nameof(bindingContext)));

            if (bindingContext.ModelType.WithoutGenericArgs() != typeof(ListArgs<>) && bindingContext.ModelType.WithoutGenericArgs() != typeof(SearchArgs<>))
                return Task.CompletedTask;

            var listArgs = (IListArgs)Activator.CreateInstance(bindingContext.ModelType);
            LoadFromQueryString(bindingContext.HttpContext.Request.Query, listArgs);
            listArgs.ValidateAndNormalize();
            bindingContext.Model = listArgs;
            bindingContext.Result = ModelBindingResult.Success(listArgs);
            return Task.CompletedTask;
        }

        public virtual void LoadFromQueryString(IQueryCollection query, IListArgs listArgs)
        {
            listArgs.Filters = new List<ListFilter>();
            foreach (var param in query)
            {
                int i;
                var (key, value) = (param.Key, param.Value);
                switch (key.ToLower())
                {
                    case "sortby":
                        listArgs.SortBy = value.ToString().Split(',').Distinct().ToArray();
                        break;
                    case "page":
                        if (int.TryParse(value, out i) && i >= 1)
                            listArgs.Page = i;
                        else
                            throw new UserErrorException("page must be an integer greater than or equal to 1.");
                        break;
                    case "pagesize":
                        if (int.TryParse(value, out i) && i >= 1 && i <= 100)
                            listArgs.PageSize = i;
                        else
                            throw new UserErrorException($"pageSize must be an integer between 1 and 100.");
                        break;
                    case "search":
                        listArgs.Search = value.ToString();
                        break;
                    case "searchon":
                        listArgs.SearchOn = value.ToString();
                        break;
                    case "searchtype":
                        var prop = listArgs.GetType().GetProperty(nameof(SearchType));
                        if (prop != null)
                        {
                            if (!Enum.TryParse(value, true, out SearchType searchType))
                            {
                                var options = string.Join(", ", Enum.GetNames(typeof(SearchType)));
                                throw new UserErrorException($"searchType must be one of: {options}");
                            }
                            prop.SetValue(listArgs, searchType);
                        }
                        else
                        {
                            // model has no SearchType. it's a ListArgs<T> and not a SearchArgs<T>
                            if (value.Count > 1)
                            {
                                foreach (var val in value)
                                {
                                    listArgs.Filters.Add(new ListFilter(key, val));
                                }
                            }
                            else
                            {
                                listArgs.Filters.Add(new ListFilter(key, value));
                            }
                        }
                        break;
                    default:
                        if (value.Count > 1)
                        {
                            foreach (var val in value)
                            {
                                listArgs.Filters.Add(new ListFilter(key, val));
                            }
                        }
                        else
                        {
                            listArgs.Filters.Add(new ListFilter(key, value));
                        }
                        break;
                }
            }
        }
    }

    public class ListArgsPageOnlyModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Require.That(bindingContext != null, new ArgumentNullException(nameof(bindingContext)));

            if (bindingContext.ModelType.WithoutGenericArgs() != typeof(ListArgsPageOnly))
                return Task.CompletedTask;

            var listArgs = (ListArgsPageOnly)Activator.CreateInstance(bindingContext.ModelType);
            LoadFromQueryString(bindingContext.HttpContext.Request.Query, listArgs);
            bindingContext.Model = listArgs;
            bindingContext.Result = ModelBindingResult.Success(listArgs);
            return Task.CompletedTask;
        }

        public virtual void LoadFromQueryString(IQueryCollection query, ListArgsPageOnly listArgs)
        {
            foreach (var param in query)
            {
                int i;
                var (key, value) = (param.Key, param.Value);
                switch (key.ToLower())
                {

                    case "page":
                        if (int.TryParse(value, out i) && i >= 1)
                            listArgs.Page = i;
                        else
                            throw new UserErrorException("page must be an integer greater than or equal to 1.");
                        break;
                    case "pagesize":
                        if (int.TryParse(value, out i) && i >= 1 && i <= 100)
                            listArgs.PageSize = i;
                        else
                            throw new UserErrorException($"pageSize must be an integer between 1 and 100.");
                        break;
                }
            }
        }
    }
}
