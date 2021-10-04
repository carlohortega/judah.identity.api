using Eis.Identity.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Eis.Identity.Api.Data
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}