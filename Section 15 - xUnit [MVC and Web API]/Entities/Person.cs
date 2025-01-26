using System;

namespace Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        public Guid PersonID { get; set; }

        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public string? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        //It's a foreign key. that means it refer to the Country Table.
        public Guid? CountryID { get; set; }

        public string? Address { get; set; }

        public bool? ReciveNewsLetters { get; set; }
    }
}
