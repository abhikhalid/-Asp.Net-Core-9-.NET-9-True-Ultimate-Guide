using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//we have added an options object as a service.
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("WeatherApi"));

//Load MyOwnConfig.json
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    //Optional: true (means at runtime even if the json file is missing, it will not throw an exception)
    //reloadOnChange: true (means if the json file is changed, it will restart the application)
    config.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);
});


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
