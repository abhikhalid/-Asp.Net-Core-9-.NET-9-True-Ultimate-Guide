using Serilog;
using CRUDExample.StartupExtensions;


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

builder.Services.ConfigureServices(builder.Configuration);

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