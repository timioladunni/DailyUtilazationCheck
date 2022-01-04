using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Model
{
    public class reportModel
    {
    }
    public class DailyReportSummaryModel
    {
        public decimal GrandTotalDebit { get; set; }
        public decimal GrandTotalCredit { get; set; }
        public decimal GrandBalance { get; set; }
        public List<DailyApiReportModel> DailtyReports { get; set; }
    }
    public class DailyApiReportModel
    {
        public DateTime Date { get; set; }
        public decimal TotalDebit { get; set; }
        public long TotalNoOfApiCalls { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal CurrentBalance { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
    }

    public class ReportAPIResponse
    {
        public string apiVersion { get; set; }
        public string referenceId { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public DailyReportSummaryModel data { get; set; }
        public Error error { get; set; }
    }
}
