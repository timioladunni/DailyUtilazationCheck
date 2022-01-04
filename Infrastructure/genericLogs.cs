using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Infrastructure
{
    public class genericLogs
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
        public string Response { get; set; }
        public string Request { get; set; }
        public DateTime? Date { get; set; }
        public string IpAddress { get; set; }
        public string UniqueIdentifier { get; set; }
        public string Vendor { get; set; }
        public string TransactionReference { get; set; }
    }
}
