var postFormatOperation = function (operation) {
    var _a, _b, _c;
    var canSortByID = (_b = (_a = operation.allParams.find(function (p) { return p.name === "sortBy"; })) === null || _a === void 0 ? void 0 : _a.enumValues) === null || _b === void 0 ? void 0 : _b.includes("ID");
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
    };
    var listAllParamMapping = {
        search: "null",
        searchOn: "null",
        sortBy: canSortByID ? "SORT_BY_ID" : "null",
        pageSize: "MAX_PAGE_SIZE",
        filters: "filters.AndFilter(filter)",
        searchType: "SearchType.AnyTerm"
    };
    var listByIDParamMapping = {
        search: "null",
        searchOn: "null",
        sortBy: "null",
        pageSize: "MAX_PAGE_SIZE",
        page: "PAGE_ONE",
        filters: "$\"ID={filterValue}\"",
        searchType: "SearchType.AnyTerm"
    };
    var listAllBatchedParamMapping = {
        search: "null",
        searchOn: "null",
        sortBy: canSortByID ? "SORT_BY_ID" : "null",
        filters: "filters.AndFilter(filter)",
        page: "PAGE_ONE",
        pageSize: "MAX_PAGE_SIZE",
        searchType: "SearchType.AnyTerm"
    };
    var csharpTypeMapping = {
        PartyType: "PartyType?",
        CommerceRole: "CommerceRole?",
        boolean: "bool?"
    };
    operation["listAllName"] = operation.name.replace("List", "ListAll");
    operation["listByIDName"] = operation.name.concat("ByID");
    operation["listAllParams"] = operation.allParams.filter(function (param) { return !notValidListAllParams.includes(param.name); });
    operation["listByIDParams"] = operation.allParams.filter(function (param) { return !notValidListByIDParams.includes(param.name); });
    operation["listArgsParams"] = operation.allParams.filter(function (param) { return !listArgsParams.includes(param.name); });
    operation["hasXP"] = !operation.name.includes("Assignment") && !["ImpersonationConfig", "OpenIdConnect", "Incrementor", "SecurityProfile", "XpIndex", "Webhook", "IntegrationEvent", "SupplierBuyer", "BuyerSupplier"].includes((_c = operation.returnType) !== null && _c !== void 0 ? _c : "");
    operation.allParams.forEach(function (param) {
        var _a, _b, _c, _d, _e;
        param["listAllValue"] = (_a = listAllParamMapping[param.name]) !== null && _a !== void 0 ? _a : param.name;
        param["listByIDValue"] = (_b = listByIDParamMapping[param.name]) !== null && _b !== void 0 ? _b : param.name;
        param["listAllBatchedValue"] = (_c = listAllBatchedParamMapping[param.name]) !== null && _c !== void 0 ? _c : param.name;
        param["listArgsValue"] = (_d = listArgsParamMapping[param.name]) !== null && _d !== void 0 ? _d : param.name;
        param.type = (_e = csharpTypeMapping[param.type]) !== null && _e !== void 0 ? _e : param.type;
        if (param.name === "to" || param.name === "from") {
            param.type = "DateTimeOffset?";
        }
    });
    if (operation.returnType === "XpIndex") {
        var sortBy = operation.allParams.find(function (x) { return x.name === "sortBy"; });
        sortBy["listAllBatchedValue"] = "null";
        sortBy["listAllValue"] = "null";
    }
    // RETURN MODIFIED OPERATION - THIS IS IMPORTANT
    return operation;
};
module.exports = {
    postFormatOperation: postFormatOperation,
};
