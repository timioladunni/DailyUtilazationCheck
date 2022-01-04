using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Interface
{
    public interface IUtilization
    {
        public Task<string> GetUtilization(string token);
    }
}
