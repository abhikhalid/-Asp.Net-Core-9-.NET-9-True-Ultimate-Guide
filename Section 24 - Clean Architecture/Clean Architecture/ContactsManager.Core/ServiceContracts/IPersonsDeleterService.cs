using ServiceContracts.DTO;
using System;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsDeleterService
    {
        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="personID">PersonID to delete</param>
        /// <returns>Returns true, if the deletion is succesful, otherwise false</returns>
        Task<bool> DeletePerson(Guid? personID);
    }
}
