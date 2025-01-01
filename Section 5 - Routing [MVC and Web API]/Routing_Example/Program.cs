var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    Endpoint? endpoint = context.GetEndpoint(); //null
    if(endpoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endpoint.DisplayName}]n");
    }

    await next(context);
});

//enable routing
app.UseRouting();

app.Use(async (context, next) =>
{
    Endpoint? endpoint = context.GetEndpoint();
    if (endpoint != null)
    {
        await context.Response.WriteAsync($"Endpoint: {endpoint.DisplayName}]n");
    }
    await next(context);
});


//creating end points
app.UseEndpoints(endpoints =>
{
    //add your end points
    //this does not forward req to next middleware. so it is a short-circuiting middleware.
    endpoints.MapGet("map1", async (context) =>
    {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.MapPost("map2", async (context) =>
    {
        await context.Response.WriteAsync("In Map 2");
    });
});

app.Run();
//app.Run(async context =>
//{
//    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
//});
