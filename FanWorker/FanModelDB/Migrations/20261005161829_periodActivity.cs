using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanModelDB.Migrations
{
    /// <inheritdoc />
    public partial class periodActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Profiles_ProfileId",
                table: "Periods");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Periods",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Profiles_ProfileId",
                table: "Periods",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Profiles_ProfileId",
                table: "Periods");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Periods",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Profiles_ProfileId",
                table: "Periods",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }
    }
}
