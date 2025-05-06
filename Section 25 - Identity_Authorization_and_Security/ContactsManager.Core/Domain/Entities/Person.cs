using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactsManager.Core.Domain.Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        //default data type in database is : nvarchar(max)

        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
        [StringLength(10)]
        public string? Gender { get; set; }

        //data type: uniqueidentifier
        //It's a foreign key. that means it refer to the Country Table.
        public Guid? CountryID { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        //data type: bit (no need to mention any other data type)
        public bool ReciveNewsLetters { get; set; }

        public string? TIN { get; set;}

        [ForeignKey("CountryID")] //It will take up the realtionship automatically, so it internally applies the joins to load the corresponding related data from the other table.
        public virtual Country? Country { get; set; } //virtual so child class can override that. not mandatory.

        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Date of Birth: {DateOfBirth?.ToString("MM/dd/yyyy")} " +
                $"Gender : {Gender}, Country ID: {CountryID}, Country: {Country?.CountryName}, Address: {Address}, Receive News Letters: {ReciveNewsLetters}";
        }
    }
}
