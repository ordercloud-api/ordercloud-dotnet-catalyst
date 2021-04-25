var postFormatOperation = function (operation) {
    var _a;
    var notValidListAllParams = ["searchType", "search", "searchOn", "sortBy", "page", "pageSize", "accessToken"];
    var listArgsParams = ["searchType", "search", "searchOn", "sortBy", "page", "pageSize", "filters", "accessToken"];
    var listArgsParamMapping = {
        search: "args.Search",
        searchOn: "args.SearchOn",
        sortBy: "args.ToSortString()",
        page: "args.Page",
        pageSize: "args.PageSize",
        filters: "args.ToFilterString()",
        searchType: "args.SearchType"
    };
    var listAllParamMapping = {
        search: "null",
        searchOn: "null",
        sortBy: "ListAllHelper.GetSort<UserGroup>()",
        pageSize: "MAX_PAGE_SIZE",
        filters: "filters.AndFilter(filter)",
        searchType: "SearchType.AnyTerm"
    };
    var listAllBatchedParamMapping = {
        search: "null",
        searchOn: "null",
        sortBy: "ListAllHelper.GetSort<UserGroup>()",
        pageSize: "MAX_PAGE_SIZE",
        searchType: "SearchType.AnyTerm"
    };
    var csharpTypeMapping = {
        PartyType: "PartyType?",
        CommerceRole: "CommerceRole?",
        boolean: "bool?"
    };
    operation["listAllName"] = operation.name.replace("List", "ListAll");
    operation["listAllParams"] = operation.allParams.filter(function (param) { return !notValidListAllParams.includes(param.name); });
    operation["listArgsParams"] = operation.allParams.filter(function (param) { return !listArgsParams.includes(param.name); });
    operation["hasXP"] = !operation.name.includes("Assignment") && !["ImpersonationConfig", "OpenIdConnect", "Incrementor", "SecurityProfile", "XpIndex", "Webhook", "IntegrationEvent"].includes((_a = operation.returnType) !== null && _a !== void 0 ? _a : "");
    operation.allParams.forEach(function (param) {
        var _a, _b, _c, _d;
        param["listAllValue"] = (_a = listAllParamMapping[param.name]) !== null && _a !== void 0 ? _a : param.name;
        param["listAllBatchedValue"] = (_b = listAllBatchedParamMapping[param.name]) !== null && _b !== void 0 ? _b : param.name;
        param["listArgsValue"] = (_c = listArgsParamMapping[param.name]) !== null && _c !== void 0 ? _c : param.name;
        param.type = (_d = csharpTypeMapping[param.type]) !== null && _d !== void 0 ? _d : param.type;
        if (param.name === "to" || param.name === "from") {
            param.type = "DateTimeOffset?";
        }
    });
    // RETURN MODIFIED OPERATION - THIS IS IMPORTANT
    return operation;
};
module.exports = {
    postFormatOperation: postFormatOperation,
};
