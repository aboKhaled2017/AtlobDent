using Microsoft.EntityFrameworkCore.Migrations;

namespace Atlob_Dent.Migrations
{
    public partial class updateCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "imgSrc",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imgSrc",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "Customers",
                nullable: false,
                defaultValue: "");
        }
    }
}
