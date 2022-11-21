using Flurl.Http;
using OrderCloud.Catalyst;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrderCloud.Integrations.Tax.Avalara
{
	public class AvalaraClient
	{
		// provided by the Avalara team to indentify this specific integration with OrderCloud
		protected static string AppName = "a0o33000003vpfvAAA";
		protected static string AvataxClientHeaderName = "X-Avalara-Client";
		protected static IFlurlRequest BuildClient(AvalaraConfig config) => config.BaseUrl.WithBasicAuth(config.AccountID, config.LicenseKey);

		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Definitions/ListTaxCodes/
		/// </summary>
		public static async Task<List<AvalaraTaxCode>> ListTaxCodesAsync(string filterParam, AvalaraConfig config)
		{
			var tax = await BuildClient(config)
				.AppendPathSegments("definitions", "taxcodes")
				.SetQueryParam("$filter", filterParam)
				.WithHeader(AvataxClientHeaderName, GetAvataxHeader())
				.GetJsonWithErrorHandlingAsync<AvalaraFetchResult<AvalaraTaxCode>, AvalaraError>(config);
			return tax.value;
		}

		/// <summary>
		/// https://developer.avalara.com/api-reference/avatax/rest/v2/methods/Transactions/CreateTransaction/
		/// </summary>
		public static async Task<AvalaraTransactionModel> CreateTransaction(AvalaraCreateTransactionModel transaction, AvalaraConfig config)
		{
			var tax = await BuildClient(config)
				.AppendPathSegments("transactions", "create")
				.WithHeader(AvataxClientHeaderName, GetAvataxHeader())
				.PostJsonWithErrorHandlingAsync<AvalaraError>(transaction, config)
				.ReceiveJson<AvalaraTransactionModel>();
			return tax;
		}

		// See https://developer.avalara.com/avatax/client-headers/
		private static string GetAvataxHeader()
		{
			return $"{AppName};{GetNugetPackageVersion()};";
		}

		private static string GetNugetPackageVersion()
		{
			return Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
		}
	}
}
