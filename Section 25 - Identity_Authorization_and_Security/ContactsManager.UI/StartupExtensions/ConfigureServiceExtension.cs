using ContactsManager.Core.Domain.IdentityEntities;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDExample.StartupExtensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddTransient<ResponseHeaderActionFilter>();

            //it adds controllers and views as services
            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add<ResponseHeaderActionFilter>(5); // we can also pass Order as a parameter in this way, then we do not need to implement 'IOrderedFilter' interface. This would only work if our filter does not have any parameter. 

                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

                options.Filters.Add(new ResponseHeaderActionFilter(logger)
                {
                    Key = "My-_key-From-Global",
                    Value = "My-Value-From-Global",
                    Order = 2
                });
            });

            //add services into IoC container
            services.AddScoped<ICountriesGetterService, CountriesGetterService>();
            services.AddScoped<ICountriesAdderService, CountriesAdderService>();
            services.AddScoped<ICountriesUploaderService, CountriesUploaderService>();

            //builder.Services.AddScoped<IPersonsGetterService,PersonsGetterServiceWithFewExcelFields>();
            services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();


            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();

            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //means, hey asp dot net core, we are trying to use asp dot net core for database connection
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddTransient<PersonsListActionFilter>();
            //Enable Identity in this project
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders() 
                //add repository
                .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();

            return services;
        }
    }
}
