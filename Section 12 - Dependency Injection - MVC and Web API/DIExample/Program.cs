using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//builder.Services.Add(new ServiceDescriptor(
//    typeof(ICitiesService), // whenever some class for ICitiesService object
//    typeof(CitiesService), // Create and supply the object of CitiesService
//    ServiceLifetime.Scoped //we will learn about it in the next chapter.
//));


//short-hand

//builder.Services.AddTransient<ICitiesService, CitiesService>();
builder.Services.AddScoped<ICitiesService, CitiesService>();
//builder.Services.AddSingleton<ICitiesService, CitiesService>();



var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
