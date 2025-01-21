var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("/", async context =>
    {
        //await context.Response.WriteAsync(app.Configuration["MyKey"]); 
        //await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey"));

        //or (I am trying to look for key x, if it is missing then 10 should be used as default value)
        await context.Response.WriteAsync(app.Configuration.GetValue<int>("x", 10)+"\n");
    });
});

app.MapControllers();

app.Run();
