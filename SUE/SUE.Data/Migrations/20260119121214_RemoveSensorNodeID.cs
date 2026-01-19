using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SUE.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSensorNodeID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_SensorNodes_SensorNodeId",
                table: "Measurements");

            migrationBuilder.AlterColumn<Guid>(
                name: "SensorNodeId",
                table: "Measurements",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_SensorNodes_SensorNodeId",
                table: "Measurements",
                column: "SensorNodeId",
                principalTable: "SensorNodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Measurements_SensorNodes_SensorNodeId",
                table: "Measurements");

            migrationBuilder.AlterColumn<Guid>(
                name: "SensorNodeId",
                table: "Measurements",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Measurements_SensorNodes_SensorNodeId",
                table: "Measurements",
                column: "SensorNodeId",
                principalTable: "SensorNodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
