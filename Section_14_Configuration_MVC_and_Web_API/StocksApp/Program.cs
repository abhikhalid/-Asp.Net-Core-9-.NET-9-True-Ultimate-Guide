using StocksApp.ServiceContracts;
using StocksApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


//Hey 'builder.Services', I would like to add 'HTTP Client Service'
builder.Services.AddHttpClient();
//add our custom service
builder.Services.AddScoped<IFinhubService,FinhubService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
