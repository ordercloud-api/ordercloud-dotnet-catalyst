// https://github.com/ordercloud-api/oc-codegen#hooks-
import {
  PostFormatOperationHook,
  Operation,
} from '@ordercloud/oc-codegen'


const postFormatOperation: PostFormatOperationHook = function(operation: Operation) {
  var notValidListAllParams = ["searchType", "search", "searchOn", "sortBy", "page", "pageSize", "accessToken"];
  var listAllParamMapping = {
    search: "null",
    searchOn: "null",
    sortBy: "ListAllHelper.GetSort<UserGroup>()",
    pageSize: "MAX_PAGE_SIZE",
    filters: "filters.AndFilter(filter)",
    searchType: "SearchType.AnyTerm"
  }

  var csharpTypeMapping = {
    PartyType: "PartyType?",
    CommerceRole: "CommerceRole?",
    boolean: "bool?"
  }

  operation["listAllParams"] = operation.allParams.filter(param => !notValidListAllParams.includes(param.name))

  operation["hasXP"] = !operation.name.includes("Assignment") && !["ImpersonationConfig", "OpenIdConnect", "Incrementor", "SecurityProfile", "XpIndex", "Webhook", "IntegrationEvent"].includes(operation.returnType ?? "")

  operation.allParams.forEach(param => {
    param["listAllValue"] = listAllParamMapping[param.name] ?? param.name
    param.type = csharpTypeMapping[param.type] ?? param.type
    if (param.name === "to" || param.name === "from") {
      param.type = "DateTimeOffset?"
    }
  });

  // RETURN MODIFIED OPERATION - THIS IS IMPORTANT
  return operation
}

module.exports = {
  postFormatOperation,
}
