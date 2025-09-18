using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemaws.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAdministratorField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdministrator",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdministrator",
                table: "Users");
        }
    }
}
