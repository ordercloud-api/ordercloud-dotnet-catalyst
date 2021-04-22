var postFormatOperation = function (operation) {
    var _a;
    var notValidListAllParams = ["searchType", "search", "searchOn", "sortBy", "page", "pageSize", "accessToken"];
    var listAllParamMapping = {
        search: "null",
        searchOn: "null",
        sortBy: "ListAllHelper.GetSort<UserGroup>()",
        pageSize: "MAX_PAGE_SIZE",
        filters: "filters.AndFilter(filter)",
        searchType: "SearchType.AnyTerm"
    };
    var csharpTypeMapping = {
        PartyType: "PartyType?",
        CommerceRole: "CommerceRole?",
        boolean: "bool?"
    };
    operation["listAllParams"] = operation.allParams.filter(function (param) { return !notValidListAllParams.includes(param.name); });
    operation["hasXP"] = !operation.name.includes("Assignment") && !["ImpersonationConfig", "OpenIdConnect", "Incrementor", "SecurityProfile", "XpIndex", "Webhook", "IntegrationEvent"].includes((_a = operation.returnType) !== null && _a !== void 0 ? _a : "");
    operation.allParams.forEach(function (param) {
        var _a, _b;
        param["listAllValue"] = (_a = listAllParamMapping[param.name]) !== null && _a !== void 0 ? _a : param.name;
        param.type = (_b = csharpTypeMapping[param.type]) !== null && _b !== void 0 ? _b : param.type;
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
