var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app object is used to created Middleware

app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello");
});

//now, can we create multiple multiple middleware like this? 
app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello again");
});

//NO!! because the nature of Run() method is that it does not forward the request to the subsequent middleware.

app.Run();