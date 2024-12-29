var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//When a browser sends a request to the Kestrel and Kestrl forwards the same request to the application code, then asp dot net core automatically creates an object of type http context
app.Run(async (HttpContext context) =>
{
    context.Response.Headers["Content-type"] = "text/html";

    if (context.Request.Headers.ContainsKey("User-Agent"))
    {
        string userAgent = context.Request.Headers["User-Agent"];
        await context.Response.WriteAsync(userAgent);
    }

});

app.Run();
 