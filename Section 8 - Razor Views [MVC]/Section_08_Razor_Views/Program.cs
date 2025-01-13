var builder = WebApplication.CreateBuilder(args);
//add all controller and views as a services, so they will be instantiated when you send a request.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
