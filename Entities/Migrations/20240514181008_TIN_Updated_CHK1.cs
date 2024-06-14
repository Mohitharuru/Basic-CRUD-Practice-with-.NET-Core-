using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class TIN_Updated_CHK1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TIN",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "Tax identification Number",
                table: "Persons",
                newName: "TaxidentificationNumber");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TIN",
                table: "Persons",
                sql: "len([TaxidentificationNumber]) = 8");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TIN",
                table: "Persons");

            migrationBuilder.RenameColumn(
                name: "TaxidentificationNumber",
                table: "Persons",
                newName: "Tax identification Number");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TIN",
                table: "Persons",
                sql: "len([Tax identification Number]) = 8");
        }
    }
}
