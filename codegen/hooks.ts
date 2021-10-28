// https://github.com/ordercloud-api/oc-codegen#hooks-
import {
  PostFormatOperationHook,
  Operation,
  Param,
} from '@ordercloud/oc-codegen'


const postFormatOperation: PostFormatOperationHook = function(operation: Operation) {
  var canSortByID = operation.allParams.find(p => p.name === "sortBy")?.enumValues?.includes("ID");

  var notValidListAllParams = ["searchType", "search", "searchOn", "sortBy", "page", "pageSize", "accessToken"];
  var notValidListByIDParams = ["searchType", "search", "searchOn", "sortBy", "page", "pageSize", "filters", "accessToken"];
  var listArgsParams = ["searchType", "search", "searchOn", "sortBy", "page", "pageSize", "filters", "accessToken"];

  var listArgsParamMapping = {
    search: "args.Search",
    searchOn: "args.SearchOn",
    sortBy: "args.ToSortString()",
    page: "args.Page",
    pageSize: "args.PageSize",
    filters: "args.ToFilterString()",
    searchType: "args.SearchType"
  }

  var listAllParamMapping = {
    search: "null",
    searchOn: "null",
    sortBy: canSortByID ? "SORT_BY_ID": "null",
    pageSize: "MAX_PAGE_SIZE",
    filters: "filters.AndFilter(filter)",
    searchType: "SearchType.AnyTerm"
  }

  var listByIDParamMapping = {
    search: "null",
    searchOn: "null",
    sortBy: "null",
    pageSize: "MAX_PAGE_SIZE",
    page: "PAGE_ONE",
    filters: "$\"ID={filterValue}\"",
    searchType: "SearchType.AnyTerm"
  }

  var listAllBatchedParamMapping = {
    search: "null",
    searchOn: "null",
    sortBy: canSortByID ? "SORT_BY_ID": "null",
    filters: "filters.AndFilter(filter)",
    page: "PAGE_ONE",
    pageSize: "MAX_PAGE_SIZE",
    searchType: "SearchType.AnyTerm"
  }

  var csharpTypeMapping = {
    PartyType: "PartyType?",
    CommerceRole: "CommerceRole?",
    boolean: "bool?"
  }

  operation["listAllName"] = operation.name.replace("List", "ListAll");
  operation["listByIDName"] = operation.name.concat("ByID");

  operation["listAllParams"] = operation.allParams.filter(param => !notValidListAllParams.includes(param.name))
  operation["listByIDParams"] = operation.allParams.filter(param => !notValidListByIDParams.includes(param.name))
  operation["listArgsParams"] = operation.allParams.filter(param => !listArgsParams.includes(param.name))

  operation["hasXP"] = !operation.name.includes("Assignment") && !["ImpersonationConfig", "OpenIdConnect", "Incrementor", "SecurityProfile", "XpIndex", "Webhook", "IntegrationEvent", "SupplierBuyer", "BuyerSupplier"].includes(operation.returnType ?? "")

  operation.allParams.forEach(param => {
    param["listAllValue"] = listAllParamMapping[param.name] ?? param.name
    param["listByIDValue"] = listByIDParamMapping[param.name] ?? param.name
    param["listAllBatchedValue"] = listAllBatchedParamMapping[param.name] ?? param.name
    param["listArgsValue"] = listArgsParamMapping[param.name] ?? param.name
    param.type = csharpTypeMapping[param.type] ?? param.type
    if (param.name === "to" || param.name === "from") {
      param.type = "DateTimeOffset?"
    }
  });

  if (operation.returnType === "XpIndex") {
    var sortBy = operation.allParams.find(x => x.name === "sortBy") as Param; 
    sortBy["listAllBatchedValue"] = "null";
    sortBy["listAllValue"] = "null";
  }

  // RETURN MODIFIED OPERATION - THIS IS IMPORTANT
  return operation
}

module.exports = {
  postFormatOperation,
}
