using DailyUtilizationCheck.Enum;
using DailyUtilizationCheck.Infrastructure;
using DailyUtilizationCheck.Interface;
using DailyUtilizationCheck.Response;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Controllers
{
    public class DailyUtilizationController : Controller
    {
        private readonly IUtilization _utilization;
        public DailyUtilizationController( IUtilization utilization)
        {
            _utilization = utilization;
        }


        [Route("api/v1/DailyUtilization")]
        [HttpPost]
        public async Task<IActionResult> GetDailyUtilization(string institutionToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(institutionToken))
                {
                    var dailyUtilzation = await _utilization.GetUtilization(institutionToken);
                    if (dailyUtilzation == Status.CustomerDoesNotExist.ToString() || dailyUtilzation == Status.NoDailyBalanceFound.ToString())
                    {
                        
                        return new ModelResponse().ReturnResponse(dailyUtilzation);
                    }
                    return new ModelResponse().SuccessResponse(dailyUtilzation);
                }
                else
                {
                    return new ModelResponse().ReturnResponse("Input Correct Token");
                }
            }
            catch (Exception ex)
            {
                return new ModelResponse().ReturnResponse(ex.Message.ToString());
            }

        }
       
    }
}
