using MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<MyCustomMiddleware>();
var app = builder.Build();

//app object is used to created Middleware

//middleware 1
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello");
    await next(context);
});

//middleware 2
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    await context.Response.WriteAsync("Hello again");
//    await next(context);
//});

//app.UseMiddleware<MyCustomMiddleware>();
//app.UseMyCustomMiddleware();
app.UseMiddleware();

//middleware 3 (terminating middleware)
app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello again");
});

app.Run();