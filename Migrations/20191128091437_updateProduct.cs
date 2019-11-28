using Microsoft.EntityFrameworkCore.Migrations;

namespace Atlob_Dent.Migrations
{
    public partial class updateProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "seen",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "ordersCount",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<long>(
                name: "consumedCount",
                table: "Products",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "consumedCount",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "seen",
                table: "Products",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "ordersCount",
                table: "Products",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
