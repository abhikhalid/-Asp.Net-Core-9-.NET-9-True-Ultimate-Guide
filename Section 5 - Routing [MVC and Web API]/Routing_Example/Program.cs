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
    endpoints.Map("products/details/{id:int?}", async context =>
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

    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
       DateTime reportDate =  Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
       await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();

