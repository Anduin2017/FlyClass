using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Anduin.FlyClass.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class AddMoneyToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoneyPaid",
                table: "TeachEvents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoneyPaid",
                table: "TeachEvents");
        }
    }
}
