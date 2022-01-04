using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Infrastructure
{
    
    public class ApiResponse
    {
        public string Message { get; set; }
        public string apiVersion { get; set; }
        public string transactionReference { get; set; }
        public object data { get; set; }
        public ApiError error { get; set; }
    }

    public class ApiError
    {
        public int code { get; set; }
        public string message { get; set; }

    }

    public class SearchInstitutionInvoiceModel
    {
        public string InstitutionCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
