using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunGroopWebApp.Migrations
{
    /// <inheritdoc />
    public partial class _2AjoutdelanonobligationdelaFKAppUserIddanslemodelRace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Races",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "Races",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Races_AppUser_AppUserId",
                table: "Races",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
