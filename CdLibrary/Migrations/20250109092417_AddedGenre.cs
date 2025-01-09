using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CdLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Cd",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cd_GenreId",
                table: "Cd",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cd_Genre_GenreId",
                table: "Cd",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cd_Genre_GenreId",
                table: "Cd");

            migrationBuilder.DropIndex(
                name: "IX_Cd_GenreId",
                table: "Cd");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Cd");
        }
    }
}
