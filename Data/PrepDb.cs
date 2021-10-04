using System;
using System.Linq;
using Eis.Identity.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Eis.Identity.Api.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            Console.WriteLine("--> Attempting apply migrations.");

            try
            {
                context.Database.Migrate();
            }
            catch
            {
                Console.WriteLine("--> Did not run migrations.");
            }

            if (!context.Users.Any())
            {
                Console.WriteLine("--> Seeding data...");

                context.Users.AddRange(
                    new AppUser() { Name = "Joe", ObjectId = "1001" },
                    new AppUser() { Name = "Jane", ObjectId = "1002" },
                    new AppUser() { Name = "Bob", ObjectId = "1004" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data.");
            }
        }
    }
}