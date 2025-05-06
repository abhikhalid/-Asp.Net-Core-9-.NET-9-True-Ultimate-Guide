using System;
using System.Collections.Generic;
using ContactsManager.Core.Domain.Entities;
namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CountriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }

        //It compares the current object to another object of CountryResponse type and return true if both values are same, otherwise return false
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            if (obj.GetType() != typeof(CountryResponse)) return false;

            CountryResponse country_to_compare = (CountryResponse)obj;

            return this.CountryID == country_to_compare.CountryID && this.CountryName == country_to_compare.CountryName;
        }

        //when you override, Equals() method, it is advisible to override 'GetHashCode()' method.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    //assume that, we are adding this method to 'Country Class'
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName,
            };
        }
    }
}
