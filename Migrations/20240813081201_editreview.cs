using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieLab.Migrations
{
    /// <inheritdoc />
    public partial class editreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rlast",
                table: "Reviews",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "Rfirst",
                table: "Reviews",
                newName: "firstName");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "Reviews",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Reviews",
                newName: "Rlast");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "Reviews",
                newName: "Rfirst");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
