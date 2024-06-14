using System;
using ServiceContracts.Enums;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="Person name cannot be blank")]
        public string? PersonName { get; set; }
        
        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Email should be in proper format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [Display(Name = "Gender")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [Display(Name = "Country")]
        public Guid? CountryID { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// converts the current object of PersonAddRequest into a new object of person type
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Address = Address,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
