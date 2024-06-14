using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person domain model class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        //where ever we are using string type we need to set string length or else this property when converted into column like this nvarchar(max) then it size can be some billions we dont what that 
        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }
        
        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        //this value will be unique identifier in sql
        public Guid? CountryID { get; set; }

        [StringLength(200)]
        public string? Address { get; set;}

        //bit datatype as per SQL internally
        public bool ReceiveNewsLetters { get; set; }

        public string? TIN { get; set; }

        //for relations 
        [ForeignKey("CountryID")]
        public virtual Country? Country { get; set; }
    }
}
