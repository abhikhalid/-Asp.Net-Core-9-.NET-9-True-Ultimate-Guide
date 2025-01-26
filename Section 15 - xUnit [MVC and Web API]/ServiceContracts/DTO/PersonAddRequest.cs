using ServiceContracts.Enums;
using System;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting 
    /// </summary>
    public class PersonAddRequest
    {
        public string? PersonName { get; set; }

        public string? Email { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        public GenderOptions? Gender{ get; set; }

        public Guid? CountryID{ get; set; }

        public string? Address { get; set; }

        public bool ReceiveNewsLetters { get; set; }
    }
}
