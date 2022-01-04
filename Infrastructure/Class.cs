using Hangfire;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Infrastructure
{
    public class Class : ActionFilterAttribute
    {
        public readonly HangfireContext _context;
        public Class(HangfireContext context )
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
               var response =  SavetoDB(context);
               
                BackgroundJob.Enqueue(() => tranferLogs(response));
               
                
            }
            catch (Exception ex)
            {

                Trace.TraceInformation(ex.Message);
               
            }

        }

        public genericLogs SavetoDB(ActionExecutedContext context)
        {
            try
            {
                var clientLogs = new genericLogs();
                clientLogs.Token = context.HttpContext.Request.Headers["Heyy"].ToString() ?? "";
                clientLogs.Request = context.HttpContext.Request.Path.ToUriComponent() ?? "";
                clientLogs.Date = DateTime.Now;
                // var setResult = JObject.Parse(JsonConvert.SerializeObject(context.Result))["Value"];
                clientLogs.TransactionReference = "transactionReference" ?? "";
                clientLogs.Response = JsonConvert.SerializeObject(context.Result) ?? "";
                //get the status code from the context.result property
                clientLogs.Status = "StatusCode";
                clientLogs.IpAddress = context.HttpContext.Request.Host.ToUriComponent() ?? "";
                clientLogs.UniqueIdentifier = context.HttpContext.Request.QueryString.Value ?? "";
                if (string.IsNullOrEmpty(clientLogs.UniqueIdentifier))
                    clientLogs.UniqueIdentifier = "";
                if (clientLogs.Request == "/api/v1/VerifyRcNumber")
                {
                    clientLogs.Vendor = "CredCAC";
                }
                else
                {
                    clientLogs.Vendor = "CredStatement";
                }

               
                return clientLogs;

            }
            catch (Exception ex)
            {

                Trace.TraceInformation(ex.Message);
                return null;

            }
        }

        public bool tranferLogs(genericLogs genericLogs)
        {
            _context.genericLog.Add(genericLogs);
            _context.SaveChanges();

            return true;
        }
    }
}
