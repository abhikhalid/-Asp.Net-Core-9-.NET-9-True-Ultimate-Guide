using CitiesManager.WebAPI.DatabaseContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

//Swager
//it enables the swagger to read the endpoints of our application. (It enables swagger to read metadata (HTTP method, URL, attribute etc) of our application)
builder.Services.AddEndpointsApiExplorer(); // Generates description for all the endpoints in the application.
//It configures swagger to generate documentation for API's endpoints.
//It is responsible to add swagger documentation in the current project.
builder.Services.AddSwaggerGen(); //generates OpenAPI specification.


var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//Swagger
app.UseSwagger(); //creates endpoint for swagger.json file
app.UseSwaggerUI(); //creates swagger UI for testing all web API endpoints / action methods

app.UseAuthorization();

app.MapControllers();

app.Run();
