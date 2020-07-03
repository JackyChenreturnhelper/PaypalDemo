using Microsoft.EntityFrameworkCore;
using PaypalDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Infrastructure.DB
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<PaypalOrderParamterModel> PaypalOrder { get; set; }

        public DbSet<PaypalRecord> PaypalRecord { get; set; }

        public DbSet<ApiTransaction> ApiTransactions { get; set; }
        public DbSet<Api> Apis { get; set; }

        public DbSet<PaypalTokenId> PaypalTokenId { get; set; }
    }
}