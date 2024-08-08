using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionManagement.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateFieldInLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Locations",
                newName: "IANAZone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IANAZone",
                table: "Locations",
                newName: "City");
        }
    }
}
