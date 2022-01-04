using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyUtilizationCheck.Infrastructure
{
    public class HangfireContext : DbContext
    {
        public HangfireContext(DbContextOptions<HangfireContext> opt) : base(opt)
        {

        }
        public DbSet<genericLogs> genericLog { get; set; }
    }
}
