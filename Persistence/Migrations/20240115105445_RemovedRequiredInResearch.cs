using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequiredInResearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Researches_Companies_OrganizerId",
                table: "Researches");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Researches",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Researches_Companies_OrganizerId",
                table: "Researches",
                column: "OrganizerId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Researches_Companies_OrganizerId",
                table: "Researches");

            migrationBuilder.UpdateData(
                table: "Researches",
                keyColumn: "OrganizerId",
                keyValue: null,
                column: "OrganizerId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Researches",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Researches_Companies_OrganizerId",
                table: "Researches",
                column: "OrganizerId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
