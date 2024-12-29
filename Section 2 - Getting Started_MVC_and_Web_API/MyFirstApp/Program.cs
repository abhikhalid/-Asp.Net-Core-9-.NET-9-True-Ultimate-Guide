//this will create a builder for the web application.
// So what this builder can do? 
// A web application Builder loads the configuration, environment and default services.
// Environment is your environment settings. For example API urls and server name and configuration is your configuration setttings such as connection strings etc.
// and services is pre-defined and user defined services.

//once this is called, you will get the instance of web application builder.
var builder = WebApplication.CreateBuilder(args);

//using this intance, you can access the configuration and services and your environemnt variables.
//builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
//builder.Services.AddMvc();
//builder.Environment.ApplicationName

//after configuring the builder, you are required to call the Build. it will return the instance of web application.
var app = builder.Build();

//using this app, you will be able to configure the middleware of your application.

app.MapGet("/", () => "Hello World!");

//start the server
app.Run();
