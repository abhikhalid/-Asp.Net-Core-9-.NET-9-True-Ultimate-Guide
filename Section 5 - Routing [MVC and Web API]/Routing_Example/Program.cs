var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();

//creating end points
app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename}.{extension}", async (context) =>
    {
        string? fileName = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In files - {fileName} - {extension}");
    });

    //route parameter name are case insensitive
    endpoints.Map("employee/profile/{employeeName=khalid}", async (context) =>
    {
       string? employeeName =  Convert.ToString(context.Request.RouteValues["employeename"]); // by default it returns object type. We have converted into string type
        await context.Response.WriteAsync($"In Employee profile - {employeeName}");
    });

    ///Eg: products/details/1
    /// if the user does not supply then id=null as it is optional parameter
    endpoints.Map("products/details/{id?}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details - {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Products details - id is not supplied");
        }
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();

