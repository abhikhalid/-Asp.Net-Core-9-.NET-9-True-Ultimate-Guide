using Microsoft.AspNetCore.Routing;
using Routing_Example.CustomConstraints;
using System;

var builder = WebApplication.CreateBuilder(args);
//hey builder, I would like to add a service called 'Routing' and as a part of the routing, I would like to register a constraint.
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months",typeof(MonthsCustomConstraint));
});

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
    //endpoints.Map("employee/profile/{employeeName:minlength(3):maxlength(7)=khalid}", async (context) =>
    endpoints.Map("employee/profile/{employeeName:length(4,7):alpha=khalid}", async (context) =>
    {
       string? employeeName =  Convert.ToString(context.Request.RouteValues["employeename"]); // by default it returns object type. We have converted into string type
        await context.Response.WriteAsync($"In Employee profile - {employeeName}");
    });

    ///Eg: products/details/1
    endpoints.Map("products/details/{id:int:range(1,1000)?}", async context =>
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

    // Eg: cities/cityid
    endpoints.Map("cities/{cityid:guid}", async context =>
    {
       Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"]));
        await context.Response.WriteAsync($"City information - {cityId}");
    });

    //sales-report/2030/apr

    //But in general, as per Microsoft Docs, you should not use route constraints to validate values.

    //The better approach is to accept invalid values into the route and then validate them in your code using "if" statements.If the value is found to be invalid, you can provide an appropriate response.

    //For example, you can return a status code 400(Bad Request), which informs the client about the expected or accepted values for this route.This approach provides more clarity and flexibility in handling invalid inputs.

    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async context =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);
        await context.Response.WriteAsync($"sales report - {year} - {month}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"No route matched at {context.Request.Path}");
});

app.Run();

