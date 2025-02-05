using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        public PersonsDbContext(DbContextOptions options) : base(options) { 
            
        }

        //we have to bind these Dbsets to corresponding table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Country>() >> Hey modelbuilder, I am trying to talk about an entity of the 'Country' type. 
            //This method will get the country type of dbSet that is 'Countries' (line no 9)
            // and that is mapped to the table called 'Countries'

            modelBuilder.Entity<Country>().ToTable("Countries"); //I want the table name to be 'Countries'. you can define any other name.
            modelBuilder.Entity<Person>().ToTable("Persons"); 

            //To add the seed data we have to use this on model creating method in the dbcontext class.
            //so after you map your model class to the table. here is the place for that.

            //Seed to Countries
            //modelBuilder.Entity<Country>().HasData(new Country() { CountryID = Guid.NewGuid(), CountryName = "Bangladesh"});

            //but there is a more shortcut way that this.

            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (Country country in countries)
            {
                modelBuilder.Entity<Country>().HasData(country);
            } 
            
            //Seed Persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (Person pesron in persons)
            {
                modelBuilder.Entity<Person>().HasData(pesron);
            }

        }
    }
}
