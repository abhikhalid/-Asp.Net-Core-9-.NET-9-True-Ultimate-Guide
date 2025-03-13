using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog;
using CRUDExample.Filters.ActionFilters;


var builder = WebApplication.CreateBuilder(args);

//Logging
//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    //loggingProvider.AddDebug();
//    //loggingProvider.AddEventLog();
//});

//replace existing logging mechanism with serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services,
   LoggerConfiguration loggerConfiguration) =>
 {
     loggerConfiguration
     .ReadFrom.Configuration(context.Configuration) // means, reading the configuration from appSettings.json
     .ReadFrom.Services(services); // this statemenet makes our service collection available to Siri log. As a part of that, any serilog sync can access the services of our application.
 });

//it adds controllers and views as services
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ResponseHeaderActionFilter>(5); // we can also pass Order as a parameter in this way, then we do not need to implement 'IOrderedFilter' interface. This would only work if our filter does not have any parameter. 

    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

    options.Filters.Add(new ResponseHeaderActionFilter(logger,"My-_key-From-Global","My-Value-From-Global",2));
});

//add services into IoC container
builder.Services.AddScoped<ICountriesGetterService, CountriesGetterService>();
builder.Services.AddScoped<ICountriesAdderService, CountriesAdderService>();
builder.Services.AddScoped<ICountriesUploaderService,CountriesUploaderService>();

//builder.Services.AddScoped<IPersonsGetterService,PersonsGetterServiceWithFewExcelFields>();
builder.Services.AddScoped<IPersonsGetterService,PersonsGetterServiceChild>();
builder.Services.AddScoped<PersonsGetterService, PersonsGetterService>();


builder.Services.AddScoped<IPersonsAdderService, PersonsAdderService>();
builder.Services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
builder.Services.AddScoped<IPersonsSorterService, PersonsSorterService>();
builder.Services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();

builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    //means, hey asp dot net core, we are trying to use asp dot net core for database connection
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<PersonsListActionFilter>();

//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

//Integrated Security=True (means, we are using windows authentication for sql server. that means you do not need separate user id and password)

var app = builder.Build();

//it enables the endpoint completion log, means it adds an extra log message as soon as the requsest resposne is completed
app.UseSerilogRequestLogging(); 

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}



app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");

if (!builder.Environment.IsEnvironment("Test"))
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseHttpLogging();
app.Run();

public partial class Program  //make the auto-generated Program accessible programmatically
{

}