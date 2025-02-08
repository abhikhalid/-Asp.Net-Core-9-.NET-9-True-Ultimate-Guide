using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new pesron into the list of persons
        /// </summary>
        /// <param name="personAddRequest"></param>
        /// <returns>Returns the same person details, along with newly generated PersonID</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the give person id
        /// </summary>
        /// <param name="guid">Person id to search</param>
        /// <returns>Returns maching person object</returns>
        Task<PersonResponse?> GetPersonByPersonId(Guid? personID);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string
        /// </summary>
        /// <param name="searchBy">Saerch field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
        /// <param name="sortOrder">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the scpefied person details based on the given person ID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>Returns the person response object after updation</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="personID">PersonID to delete</param>
        /// <returns>Returns true, if the deletion is succesful, otherwise false</returns>
        Task<bool> DeletePerson(Guid? personID);
    }
}
