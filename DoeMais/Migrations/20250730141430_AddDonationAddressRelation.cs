using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoeMais.Migrations
{
    /// <inheritdoc />
    public partial class AddDonationAddressRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
