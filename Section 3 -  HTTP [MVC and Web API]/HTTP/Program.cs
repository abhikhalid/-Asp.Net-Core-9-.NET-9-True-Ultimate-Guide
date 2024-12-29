var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//When a browser sends a request to the Kestrel and Kestrl forwards the same request to the application code, then asp dot net core automatically creates an object of type http context
app.Run(async (HttpContext context) =>
{
    string path = context.Request.Path;
    string method = context.Request.Method;

    context.Response.Headers["Content-type"] = "text/html";
    await context.Response.WriteAsync(path);
    await context.Response.WriteAsync(method);
});

app.Run();
 