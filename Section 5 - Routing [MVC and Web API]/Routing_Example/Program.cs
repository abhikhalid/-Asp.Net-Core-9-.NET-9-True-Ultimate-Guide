var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();


//creating end points
app.UseEndpoints(endpoints =>
{
    //add your end points
    //this does not forward req to next middleware. so it is a short-circuiting middleware.
    endpoints.Map("map1", async (context) =>
    {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.Map("map2", async (context) =>
    {
        await context.Response.WriteAsync("In Map 2");
    });
});

//app.Run();
app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});
