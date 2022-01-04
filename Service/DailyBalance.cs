using DailyUtilizationCheck.Infrastructure;
using DailyUtilizationCheck.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Service
{
    public class DailyBalance
    {
        private readonly server _server;
        public DailyBalance(IOptions<server> server)
        {
            _server = server.Value;
        }
        public static async Task<DailyReportSummaryModel> DailyApiCostUtilization(SearchInstitutionInvoiceModel model)
        {
            var client = new RestClient("https://credequitydb.com/aeon/api/v1/Account/DailyUtilization");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = "{\"institutionCode\": \"" + model.InstitutionCode + "\",\"startDate\": \"" + model.StartDate.ToString() + "\",\"endDate\":\"" + model.EndDate.ToString() + "\"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var reportResponse =  response.Content;
            if (reportResponse != "")
            {
                var report = JsonConvert.DeserializeObject<ReportAPIResponse>(reportResponse);
                var reportSummary = report.data ?? null;
                return reportSummary;
            }
            else
            {
                return null;
            }
        }
    }
}
