using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class NewVer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Postid",
                table: "Tags",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Postid",
                table: "Tags",
                column: "Postid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Posts_Postid",
                table: "Tags",
                column: "Postid",
                principalTable: "Posts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Posts_Postid",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_Postid",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Postid",
                table: "Tags");
        }
    }
}
