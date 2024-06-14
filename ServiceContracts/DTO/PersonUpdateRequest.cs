using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        /// <summary>
        /// Acts as a DTO for updating a new person
        /// </summary>
        [Required(ErrorMessage = "Person ID cant be blank")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person name cannot be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Email should be in proper format")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// converts the current object of PersonAddRequest into a new object of person type
        /// </summary>
        /// <returns>Returns person object</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
            //it copies all the personupdaterequest details into person object 
        }
    }
}

