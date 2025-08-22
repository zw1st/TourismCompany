using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvanSusaninProject_DataBase.Migrations
{
    /// <inheritdoc />
    public partial class DbContextFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
