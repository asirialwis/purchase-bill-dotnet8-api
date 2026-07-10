using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PurchaseBillAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialLocationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationDetails",
                columns: table => new
                {
                    Location_Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationDetails", x => x.Location_Code);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationDetails");
        }
    }
}
