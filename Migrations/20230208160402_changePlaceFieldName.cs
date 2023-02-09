using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace placeBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class changePlaceFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "place",
                table: "place",
                newName: "seat_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "seat_number",
                table: "place",
                newName: "place");
        }
    }
}
