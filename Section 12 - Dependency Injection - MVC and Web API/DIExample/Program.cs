using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.Add(new ServiceDescriptor(
    typeof(ICitiesService), // whenever some class for ICitiesService object
    typeof(CitiesService), // Create and supply the object of CitiesService
    ServiceLifetime.Singleton //we will learn about it in the next chapter.
));




var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
