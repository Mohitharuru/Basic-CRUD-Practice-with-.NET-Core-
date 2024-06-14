using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class TIN_StoredProcedure2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                ALTER PROCEDURE [dbo].[GetAllPersons]
                AS BEGIN
                    SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters, TIN FROM [dbo].[Persons]
                END
            ";
            //now we need to execute the above stored procedure created
            migrationBuilder.Sql(sp_GetAllPersons);
            //when ever this SQL statement executed it creates a stored procedure in the database 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                DROP PROCEDURE [dbo].[GetAllPersons]";

            migrationBuilder.Sql(sp_GetAllPersons);
        }
    }
}
