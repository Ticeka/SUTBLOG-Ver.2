using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class AddingTagAndSubHeadAboutContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AboutContent",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubHeader",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "AboutContent",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SubHeader",
                table: "AspNetUsers");
        }
    }
}
