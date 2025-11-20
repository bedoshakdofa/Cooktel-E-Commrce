using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cooktel_E_commrece.Migrations
{
    /// <inheritdoc />
    public partial class AddProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "users");
        }
    }
}
