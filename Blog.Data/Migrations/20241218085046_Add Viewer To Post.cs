using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class AddViewerToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Viewer",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Viewer",
                table: "Posts");
        }
    }
}
