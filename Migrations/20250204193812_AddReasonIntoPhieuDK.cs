using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDaoTao.Migrations
{
    /// <inheritdoc />
    public partial class AddReasonIntoPhieuDK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDo",
                table: "PhieuDangKyDayBu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDo",
                table: "PhieuDangKyDayBu");
        }
    }
}
