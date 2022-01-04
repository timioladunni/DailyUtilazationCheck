using DailyUtilizationCheck.Enum;
using DailyUtilizationCheck.Infrastructure;
using DailyUtilizationCheck.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Service
{
    public class UtilizationService : IUtilization
    {
        public readonly AeonData.Aeon2Context _context;
        public UtilizationService(AeonData.Aeon2Context context)
        {
            _context = context;
        }
        public async Task<string> GetUtilization(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Status.CustomerDoesNotExist.ToString();
                }
                else
                {
                    var institution = new InstitutionInfo(_context);
                    var response = await institution.GetInstitutionInfo(token);
                    if (response == null)
                    {
                        return Status.CustomerDoesNotExist.ToString();
                    }
                    else
                    {
                        SearchInstitutionInvoiceModel model = new SearchInstitutionInvoiceModel();
                        var startDate = "2020-11-30";
                        if (DateTime.Now.Day < 10)
                        {
                            var endDate = $"{DateTime.Now.Year}-{DateTime.Now.Month}-0{DateTime.Now.Day}";
                            model.EndDate = endDate;
                        }
                        else
                        {
                            var endDate = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
                            model.EndDate = endDate;
                        }
                        
                        model.InstitutionCode = response.Code;
                        model.StartDate = startDate;
                        
                        var dailyBalance = await DailyBalance.DailyApiCostUtilization(model);
                        if (dailyBalance == null)
                        {
                            return Status.NoDailyBalanceFound.ToString();
                        }
                        else
                        {
                            var result = JsonConvert.SerializeObject(dailyBalance);
                            return result;
                        }

                    }
                }
            }
            catch (Exception)
            {

                return Status.CustomerDoesNotExist.ToString();
            }
            
        }
    }
}
