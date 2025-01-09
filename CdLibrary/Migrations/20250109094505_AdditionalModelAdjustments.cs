using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CdLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalModelAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cd_Genre_GenreId",
                table: "Cd");

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Cd",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Artist",
                table: "Cd",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AddForeignKey(
                name: "FK_Cd_Genre_GenreId",
                table: "Cd",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cd_Genre_GenreId",
                table: "Cd");

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "Cd",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Artist",
                table: "Cd",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cd_Genre_GenreId",
                table: "Cd",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
