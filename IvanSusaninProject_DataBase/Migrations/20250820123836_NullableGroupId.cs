using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvanSusaninProject_DataBase.Migrations
{
    /// <inheritdoc />
    public partial class NullableGroupId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "Places",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "Places",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Groups_GroupId",
                table: "Places",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
