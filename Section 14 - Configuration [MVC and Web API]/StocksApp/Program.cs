var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();


//Hey 'builder.Services', I would like to add 'HTTP Client Service'
builder.Services.AddHttpClient();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
