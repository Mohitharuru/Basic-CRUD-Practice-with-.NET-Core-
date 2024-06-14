using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDTests
{
    //to make this web application gfactory working we need to Install a nuget package called Microsoft.aspnetcore.Mvc.testing
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        //this code will execute after executing the program.cs in CRUD Example project completed so everything used there will be available here
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseFortesting");
                });
            }); //in this lambda expression we can config our own services that are needed to run the test  

        }
    }
}
