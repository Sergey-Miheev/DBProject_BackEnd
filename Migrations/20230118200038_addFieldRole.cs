using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace placeBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class addFieldRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "role",
                table: "account",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "role",
                table: "account",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }
    }
}
