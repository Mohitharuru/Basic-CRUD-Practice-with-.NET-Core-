using Entities;
using Microsoft.EntityFrameworkCore;
using RepositaryContracts;
using System;
using System.Linq.Expressions;

namespace Repositries
{
    public class PersonsRepository : IPersonsRepositary
    {
        private readonly Entities.ApplicationDbContext _db;
        public PersonsRepository(Entities.ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personID)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp=>temp.PersonID == personID));
            int count = await _db.SaveChangesAsync();
            return count>0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
            return await _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Country").Where(predicate).ToListAsync();
        }

        public async Task<Person?> GetPersonsByPersonID(Guid personID)
        {
            return await _db.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonID == personID);
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == person.PersonID);
            if(matchingPerson == null)
            {
                return person;
            }

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.Gender = person.Gender;
            matchingPerson.Address = person.Address;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            int count = await _db.SaveChangesAsync();
            return matchingPerson;
        }
    }
}
