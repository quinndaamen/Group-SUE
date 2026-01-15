using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SUE.Data.Migrations
{
    /// <inheritdoc />
    public partial class household : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegionCode",
                table: "Households",
                newName: "P1ID");

            migrationBuilder.AddColumn<string>(
                name: "P1ID",
                table: "Measurements",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "P1ID",
                table: "Measurements");

            migrationBuilder.RenameColumn(
                name: "P1ID",
                table: "Households",
                newName: "RegionCode");
        }
    }
}
