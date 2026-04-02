using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using QuantityMeasurementWebApi.Data;

#nullable disable

namespace RepositoryLayer.Migrations
{
    [DbContext(typeof(QuantityDbContext))]
    [Migration("20260401093000_AddTargetUnitToMeasurements")]
    public class AddTargetUnitToMeasurements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetUnit",
                table: "Measurements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetUnit",
                table: "Measurements");
        }
    }
}
