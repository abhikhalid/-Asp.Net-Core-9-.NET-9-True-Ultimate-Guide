using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//we have added an options object as a service.
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("WeatherApi"));

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
