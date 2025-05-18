using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureFileSharingSystem.Persistence.Migrations
{
    public partial class AddAPIKeyColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "AspNetUsers");
        }
    }
}
