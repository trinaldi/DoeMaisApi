using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeMais.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSurnameFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
