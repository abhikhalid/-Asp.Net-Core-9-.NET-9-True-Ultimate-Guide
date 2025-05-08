using Serilog;
//using CRUDExample.StartupExtensions;
using CRUDExample.Middleware;
using CRUDExample;


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
else
{
    app.UseExceptionHandlingMiddleware();
}



app.UseStaticFiles();

app.UseRouting(); //Identifying action method based on routing
app.UseAuthentication(); //Reading Identity cookie
//it adds the authorization middleware to the request pipeline. It checks whether the user is authorized to access the resource or not.
// but how do you specify what action methods are allowed and what not? By default we will restrict access to all action methods.
//But we will allow the action method of Account Controller to be accessed by anonymous users.

//The fundamental difference between 'useAuthentication' and 'useAuthorization' is that authentication is about identifying the user, while authorization is,
// useAuthetication is about checking whether the user is authenticated or not
// useAuthorization is about checking whether the user is authorized to access the resource (particular action method) or not.
app.UseAuthorization();

app.MapControllers(); //Execute the filter pipeline (action + filters)

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