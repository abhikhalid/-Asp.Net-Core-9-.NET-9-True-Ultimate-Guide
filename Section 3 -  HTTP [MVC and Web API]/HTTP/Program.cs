var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//When a browser sends a request to the Kestrel and Kestrl forwards the same request to the application code, then asp dot net core automatically creates an object of type http context
app.Run(async (HttpContext context) =>
{
    context.Response.Headers["MyKey"] = "my value"; //Response.Headers is a Dictionary in C#
    context.Response.Headers["Server"] = "My server"; //Response.Headers is a Dictionary in C#
    context.Response.Headers["Content-Type"] = "text/html"; //Response.Headers is a Dictionary in C#
    //this will be added to the response body
    await context.Response.WriteAsync("<h1>Hello</h1>");
    await context.Response.WriteAsync("<h2>World</h2>");
});

app.Run();
 