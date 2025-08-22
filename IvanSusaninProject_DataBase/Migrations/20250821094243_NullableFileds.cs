using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvanSusaninProject_DataBase.Migrations
{
    /// <inheritdoc />
    public partial class NullableFileds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excursions_Guides_GuideId",
                table: "Excursions");

            migrationBuilder.AddForeignKey(
                name: "FK_Excursions_Guides_GuideId",
                table: "Excursions",
                column: "GuideId",
                principalTable: "Guides",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Excursions_Guides_GuideId",
                table: "Excursions");

            migrationBuilder.AddForeignKey(
                name: "FK_Excursions_Guides_GuideId",
                table: "Excursions",
                column: "GuideId",
                principalTable: "Guides",
                principalColumn: "Id");
        }
    }
}
