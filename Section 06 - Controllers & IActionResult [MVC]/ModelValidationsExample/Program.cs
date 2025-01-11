using ModelValidationsExample.CustomModelBinders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // We have commented here, because we are going to use default model binder provider.
    //options.ModelBinderProviders.Insert(0, new PersonBinderProvider());
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
