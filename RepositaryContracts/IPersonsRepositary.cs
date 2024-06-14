using Entities;
using System;
using System.Linq.Expressions;

namespace RepositaryContracts
{
    /// <summary>
    /// Represents data access logic for managing person entity
    /// </summary>
    public interface IPersonsRepositary
    {
        /// <summary>
        /// Add a person object to the data source
        /// </summary>
        /// <param name="person">Person object to add</param>
        /// <returns>returns the person object after adding it to the table</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// Returns all the persons in the data source
        /// </summary>
        /// <returns>list of person object from the table</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// returns a person object based on person id
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns>A person object or null</returns>
        Task<Person?> GetPersonsByPersonID(Guid personID);

        /// <summary>
        /// returns all person objects based on given expression
        /// </summary>
        /// <param name="predicate">LINQ expression to check</param>
        /// <returns>All matching persons with given condition</returns>
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>
        /// Delete a person object based on the person id
        /// </summary>
        /// <param name="personID">person ID to search</param>
        /// <returns>returns true, if deletion is successful; otherwise false</returns>
        Task<bool> DeletePersonByPersonID(Guid personID);

        /// <summary>
        /// Updates a person object based on the given person id
        /// </summary>
        /// <param name="person">person object to update</param>
        /// <returns>returns the updated person object</returns>
        Task<Person> UpdatePerson(Person person);
    }
}
