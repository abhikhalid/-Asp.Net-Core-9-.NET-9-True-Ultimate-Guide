var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//enable routing
app.UseRouting();


//creating end points
app.UseEndpoints(endpoints =>
{
    //add your end points
});

app.Run();
