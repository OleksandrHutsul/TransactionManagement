using Microsoft.EntityFrameworkCore;
using TransactionManagement.DAL.Entities;

namespace TransactionManagement.DAL.Context
{
    public class ApiDbContext: DbContext
    {
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options) { }
    }
}