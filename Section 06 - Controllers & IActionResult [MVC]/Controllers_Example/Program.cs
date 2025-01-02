using Controllers_Example.Controllers;

var builder = WebApplication.CreateBuilder(args);

// In asp.dot net core, Controllers are also services. and you are required to add all the services to the application builder.
//builder.Services.AddTransient<HomeController>(); 

// but in a large application, you will have lots of controller. so let do the short-cut
//ASP.NET Core automatically detects what are the controller present in the application

builder.Services.AddControllers(); //adds all the controller classes as service

var app = builder.Build();
app.UseStaticFiles();
//let's enable routing
/*
 
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    //it will detect all the controllers of your entire project and it will pickup all the action methods and for all the action methods, routing will be added at a time.
    endpoints.MapControllers();
});

*/

// let's simply the above process
app.MapControllers();

app.Run();
