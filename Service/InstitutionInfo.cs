using DailyUtilizationCheck.AeonData;
using DailyUtilizationCheck.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Service
{
    public class InstitutionInfo
    {
        private Aeon2Context _aeon2Context;
        public InstitutionInfo(Aeon2Context aeon2Context)
        {
            _aeon2Context = aeon2Context;
        }


        public  async Task<Institution> GetInstitutionInfo(string token)
        {
            try
            {
                var institution = await _aeon2Context.Institutions.FirstOrDefaultAsync(t => t.Token == token);
                if (institution == null)
                {
                    return null;
                }
                else
                {
                    return institution;
                }
            }
            catch (Exception)
            {

                return null;
            }
         
        }
    }
}
