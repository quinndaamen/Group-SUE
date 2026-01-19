using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SUE.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMeasurementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentPowerKW",
                table: "Measurements",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPowerKW",
                table: "Measurements");
        }
    }
}
