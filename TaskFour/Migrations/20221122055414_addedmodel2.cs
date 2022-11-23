using Microsoft.EntityFrameworkCore.Migrations;

namespace TaskFour.Migrations
{
    public partial class addedmodel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserProfiles");
        }
    }
}
