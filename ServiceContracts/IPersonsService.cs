using System;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new persons into a existing list of persons 
        /// </summary>
        /// <param name="personAddRequest">person to add</param>
        /// <returns>returns the same person details,  along with newly generated PersonID</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all the persons
        /// </summary>
        /// <returns>returns a list of objects of PersonResponse type</returns>
        Task<List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// the person object pased on the given person id
        /// </summary>
        /// <param name="personID">Person ID to search</param>
        /// <returns>Matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Returns all person objects that matches with the given search field and search string   
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string? searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons 
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property key , based on which the persons should be sorted</param>
        /// <param name="sortOrderOptions">Asc or Desc</param>
        /// <returns>Returns sorted list of PersonResponse as List</returns>
        Task<List<PersonResponse>> GetSortedPerson(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOptions);

        /// <summary>
        /// Updates the specified person details based on the given person ID
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update, including person id</param>
        /// <returns>returns the person object after updation </returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);

        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="personID">PersonID to delete</param>
        /// <returns>returns true or false for successful deletion</returns>
        Task<bool> DeletePerson(Guid? personID);

        //if u written as MemoryStream we can easily convert as file in controller action method
        /// <summary>
        /// Returns the persons as CSV 
        /// </summary>
        /// <returns>Returns the memory stream with CSV data of persons</returns>
        Task<MemoryStream> GetPersonsCSV();

        /// <summary>
        /// Returns the persons as Excel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data of persons</returns>
        Task<MemoryStream> GetPersonsExcel();
    }
}
