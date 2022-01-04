using DailyUtilizationCheck.Enum;
using DailyUtilizationCheck.Infrastructure;
using DailyUtilizationCheck.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Response
{
   
    public class ModelResponse
    {
        public IActionResult ReturnResponse(string response)
        {
            ApiResponse resp = new ApiResponse();
            resp.Message = "UnSuccessful";
            resp.data = null;
            ApiError error = new ApiError();
            error.code = 400;
            error.message = response;
            resp.error = error;
            return new BadRequestObjectResult(resp);
        }

        public IActionResult SuccessResponse(string response)
        {
            try
            {
                ApiResponse successResp = new ApiResponse();
                successResp.apiVersion = "v1";
                successResp.transactionReference = GenerateReciept();
                successResp.Message = "Successful";
                var report = JsonConvert.DeserializeObject<DailyReportSummaryModel>(response);
                successResp.data = report;
                successResp.error = null;
                return new OkObjectResult(successResp);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(Status.NoDailyBalanceFound.ToString());
            }   
        
        }

        public static string GenerateReciept()
        {
            string numbers = "1234567890";

            string characters = numbers;
            int length = 10;
            string id = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (id.IndexOf(character) != -1);
                id += character;
            }
            var text = "CE" + id;
            return text;
        }
    }   
}
