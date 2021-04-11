using Microsoft.EntityFrameworkCore;
using System;
using WebApi;
using WebApi.Model;

namespace WebApi
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<CreditCardDetails> CreditCardDetails { get; set; }
        public DbSet<PaymentState> PaymentState { get; set; }
    }
}
