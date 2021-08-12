using Microsoft.EntityFrameworkCore.Migrations;

namespace kat.Migrations
{
    public partial class noteUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "enteredBy",
                table: "Note",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enteredBy",
                table: "Note");
        }
    }
}
