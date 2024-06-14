using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    //as we havent made any modification to the model class it generates an empty migration like 
    public partial class GetPersons_StoredProcedure : Migration
    {
        //here we can make any changes to db
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                CREATE PROCEDURE [dbo].[GetAllPersons]
                AS BEGIN
                    SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters FROM [dbo].[Persons]
                END
            ";
            //now we need to execute the above stored procedure created
            migrationBuilder.Sql(sp_GetAllPersons);
            //when ever this SQL statement executed it creates a stored procedure in the database 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //some time you would like to revoke or undo the migrations so while undoing the migrations 
            string sp_GetAllPersons = @"
                DROP PROCEDURE [dbo].[GetAllPersons]";
            
            migrationBuilder.Sql(sp_GetAllPersons);
            
        }
    }
}
