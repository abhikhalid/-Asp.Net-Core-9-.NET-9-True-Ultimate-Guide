var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//When a browser sends a request to the Kestrel and Kestrl forwards the same request to the application code, then asp dot net core automatically creates an object of type http context
app.Run(async (HttpContext context) =>
{
    //then the context contains the information related to your request, response and many more other details.
    context.Response.StatusCode = 400;
    //this will be added to the response body
    await context.Response.WriteAsync("Hello");
    await context.Response.WriteAsync("World");
});

app.Run();
