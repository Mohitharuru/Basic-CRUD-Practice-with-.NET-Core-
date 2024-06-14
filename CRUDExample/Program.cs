using Services;
using ServiceContracts;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositaryContracts;
using Repositries;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); //tells .net to use SQL Server for database connection
});

builder.Services.AddScoped<ICountriesRepositary, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepositary, PersonsRepository>();
if(builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

//this piece of code make the program class to acts as partial class so that we can generate the automatic generated program class automatically any where 
public partial class Program{ }