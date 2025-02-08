using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class GetPersonsStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                Create PROCEDURE [dbo].[GetAllPersons]
                AS BEGIN
                   SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReciveNewsLetters,TaxIdentificationNumber  
                   FROM [dbo].[Persons]
                END
            ";

            migrationBuilder.Sql(sp_GetAllPersons);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                DROP PROCEDURE [dbo].[GetAllPersons]";

            migrationBuilder.Sql(sp_GetAllPersons);

        }
    }
}
