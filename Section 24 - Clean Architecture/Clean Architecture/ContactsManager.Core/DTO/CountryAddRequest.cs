using ContactsManager.Core.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new country
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        //by calling this method in the service, you can easily convert an existing 'Country Add Request' object into 'Country' class
        public Country ToCountry()
        {
            return new Country() { CountryName = CountryName };
        }
    }
}
