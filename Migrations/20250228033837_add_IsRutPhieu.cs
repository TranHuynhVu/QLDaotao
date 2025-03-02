using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDaoTao.Migrations
{
    /// <inheritdoc />
    public partial class add_IsRutPhieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRutPhieu",
                table: "PhieuDangKyDayBu",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRutPhieu",
                table: "PhieuDangKyDayBu");
        }
    }
}
