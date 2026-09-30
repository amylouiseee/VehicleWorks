using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehicleWorks.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryBoundaries2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "MinWeightKg",
                value: 500m);

            migrationBuilder.UpdateData(
                table: "VehicleCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "MinWeightKg",
                value: 2500m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VehicleCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "MinWeightKg",
                value: 500.01m);

            migrationBuilder.UpdateData(
                table: "VehicleCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "MinWeightKg",
                value: 2500.01m);
        }
    }
}
