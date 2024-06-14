using System.ComponentModel.DataAnnotations; //for key attribute

namespace Entities
{
    /// <summary>
    /// Domain Model for storing the Country details 
    /// </summary>
    
    //images this as a table with 2 columns
    public class Country
    {
        //it is mandatory to add primary key for a table
        [Key]
        public Guid CountryID { get; set; }

        [StringLength(40)]
        public string? CountryName { get; set; }

        public virtual ICollection<Person>? Persons { get; set; } //by default this value is null 
    }
}
