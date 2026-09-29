using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FanModelDB.Migrations
{
    /// <inheritdoc />
    public partial class SettingsToControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "maxDelta",
                table: "Settings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "maxDeltaExternal",
                table: "Settings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "minDelta",
                table: "Settings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "minExternal",
                table: "Settings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "minRoom",
                table: "Settings",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "topDelteRoom",
                table: "Settings",
                type: "double precision",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "maxDelta", "maxDeltaExternal", "minDelta", "minExternal", "minRoom", "topDelteRoom" },
                values: new object[] { 3.0, 5.0, 0.0, 5.0, -1.0, 3.0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "maxDelta",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "maxDeltaExternal",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "minDelta",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "minExternal",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "minRoom",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "topDelteRoom",
                table: "Settings");
        }
    }
}
