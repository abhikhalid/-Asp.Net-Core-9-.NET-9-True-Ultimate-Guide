using ContactsManager.Core.Domain.Entities;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Entities
{
    //public class ApplicationDbContext : DbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Person> Persons { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { 
            
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


            //Fluent API
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
                .HasColumnName("TaxIdentificationNumber") //instad of TIN, this will be the name of the database table column
                .HasColumnType("varchar(8)")
                 .HasDefaultValue("ABC12345");

            //modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

            //modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

            //Table Relations
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasOne<Country>(c => c.Country)
                .WithMany(p => p.Persons)
                .HasForeignKey(p => p.CountryID);
            });
        }

        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReciveNewsLetters", person.ReciveNewsLetters),
            };

            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID,@PersonName,@Email,@DateOfBirth,@Gender,@CountryID,@Address,@ReciveNewsLetters"
                ,parameters);
        }

    }
}
