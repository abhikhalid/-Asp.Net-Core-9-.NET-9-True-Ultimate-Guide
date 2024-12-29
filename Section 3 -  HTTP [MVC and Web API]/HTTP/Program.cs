using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//When a browser sends a request to the Kestrel and Kestrl forwards the same request to the application code, then asp dot net core automatically creates an object of type http context
app.Run(async (HttpContext context) =>
{
   System.IO.StreamReader reader = new StreamReader(context.Request.Body);
   string body = await reader.ReadToEndAsync();

   Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(body);

    if (queryDict.ContainsKey("firstName"))
    {
        string firstName = queryDict["firstName"][0];
        await context.Response.WriteAsync(firstName);
    }

   
});

app.Run();
 