using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductDimensions_Width",
                table: "ProductDetails",
                newName: "ProductDimensions_WidthMm");

            migrationBuilder.RenameColumn(
                name: "ProductDimensions_Length",
                table: "ProductDetails",
                newName: "ProductDimensions_LengthMm");

            migrationBuilder.RenameColumn(
                name: "ProductDimensions_Height",
                table: "ProductDetails",
                newName: "ProductDimensions_HeightMm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductDimensions_WidthMm",
                table: "ProductDetails",
                newName: "ProductDimensions_Width");

            migrationBuilder.RenameColumn(
                name: "ProductDimensions_LengthMm",
                table: "ProductDetails",
                newName: "ProductDimensions_Length");

            migrationBuilder.RenameColumn(
                name: "ProductDimensions_HeightMm",
                table: "ProductDetails",
                newName: "ProductDimensions_Height");
        }
    }
}
