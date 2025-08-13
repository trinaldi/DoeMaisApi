using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeMais.Migrations
{
    /// <inheritdoc />
    public partial class DonationAddressSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Addresses_AddressId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_AddressId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Donations");

            migrationBuilder.AddColumn<string>(
                name: "AddressSnapshot_City",
                table: "Donations",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressSnapshot_Complement",
                table: "Donations",
                type: "character varying(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressSnapshot_Neighborhood",
                table: "Donations",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressSnapshot_State",
                table: "Donations",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressSnapshot_Street",
                table: "Donations",
                type: "character varying(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressSnapshot_ZipCode",
                table: "Donations",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressSnapshot_City",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AddressSnapshot_Complement",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AddressSnapshot_Neighborhood",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AddressSnapshot_State",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AddressSnapshot_Street",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "AddressSnapshot_ZipCode",
                table: "Donations");

            migrationBuilder.AddColumn<long>(
                name: "AddressId",
                table: "Donations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_AddressId",
                table: "Donations",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Addresses_AddressId",
                table: "Donations",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
