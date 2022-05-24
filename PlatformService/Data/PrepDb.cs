using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb{
        public static void PrepPopulation(IApplicationBuilder app,bool prod)
        {
            using (var serviceScope=app.ApplicationServices.CreateScope())
            {
              SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),prod);  
            }


        }

        private static void SeedData(AppDbContext context,bool isprod)
        {
            if(isprod){
                Console.WriteLine("Applying Migration....>");

                try
                {
                    context.Database.Migrate();
                }
                catch (System.Exception ex)
                {
                    
                   Console.WriteLine($"Could Not Run Migration {ex}....>");
                }
                
            }

            if(!context.Platforms.Any())
            {

            Console.WriteLine("Seeding Data");
            context.Platforms.AddRange(
                new Platform() {Name="Dot Net",Publisher="Microsoft",Cost="Free"},
                new Platform() {Name="Sql Server",Publisher="Microsoft",Cost="Free"},
                new Platform() {Name="Kubernetes",Publisher="Cloud",Cost="Free"}
            );

            context.SaveChanges();

            }
            else{
                Console.WriteLine("We Already Have Data");
            }
        }
    }
}