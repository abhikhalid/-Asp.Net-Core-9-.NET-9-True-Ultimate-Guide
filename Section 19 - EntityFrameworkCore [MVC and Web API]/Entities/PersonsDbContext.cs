using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class PersonsDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }

        //we have to bind these Dbsets to corresponding table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Country>() >> Hey modelbuilder, I am trying to talk about an entity of the 'Country' type. 
            //This method will get the country type of dbSet that is 'Countries' (line no 9)
            // and that is mapped to the table called 'Countries'

            modelBuilder.Entity<Country>().ToTable("Countries"); //I want the table name to be 'Countries'. you can define any other name.
            modelBuilder.Entity<Person>().ToTable("Persons"); 
        }
    }
}
