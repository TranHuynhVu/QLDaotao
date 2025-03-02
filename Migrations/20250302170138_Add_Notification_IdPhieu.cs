using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDaoTao.Migrations
{
    /// <inheritdoc />
    public partial class Add_Notification_IdPhieu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPhieu",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdPhieu",
                table: "Notifications");
        }
    }
}
