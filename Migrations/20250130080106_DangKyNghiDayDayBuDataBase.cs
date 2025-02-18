using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLDaoTao.Migrations
{
    /// <inheritdoc />
    public partial class DangKyNghiDayDayBuDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HocPhan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocPhan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Khoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhieuDangKyDayBu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoBuoiXinNghi = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuDangKyDayBu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BoMon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdKhoa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoMon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoMon_Khoa_IdKhoa",
                        column: x => x.IdKhoa,
                        principalTable: "Khoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BanSaoVBCTDiKem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdPhieuDangKyDayBu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanSaoVBCTDiKem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BanSaoVBCTDiKem_PhieuDangKyDayBu_IdPhieuDangKyDayBu",
                        column: x => x.IdPhieuDangKyDayBu,
                        principalTable: "PhieuDangKyDayBu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiangVien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdBoMon = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangVien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiangVien_BoMon_IdBoMon",
                        column: x => x.IdBoMon,
                        principalTable: "BoMon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LopHocPhan",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TuTiet = table.Column<int>(type: "int", nullable: false),
                    DenTiet = table.Column<int>(type: "int", nullable: false),
                    Thu = table.Column<int>(type: "int", nullable: false),
                    Phong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdGiangVien = table.Column<int>(type: "int", nullable: false),
                    IdHocPhan = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHocPhan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LopHocPhan_GiangVien_IdGiangVien",
                        column: x => x.IdGiangVien,
                        principalTable: "GiangVien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LopHocPhan_HocPhan_IdHocPhan",
                        column: x => x.IdHocPhan,
                        principalTable: "HocPhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LopHocPhanPhieuDangKyDayBu",
                columns: table => new
                {
                    IdPhieuDangKyDayBu = table.Column<int>(type: "int", nullable: false),
                    IdLopHocPhan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Thu = table.Column<int>(type: "int", nullable: false),
                    TuTiet = table.Column<int>(type: "int", nullable: false),
                    DenTiet = table.Column<int>(type: "int", nullable: false),
                    Phong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayXinNghi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayDayBu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LopHocPhanPhieuDangKyDayBu", x => new { x.IdLopHocPhan, x.IdPhieuDangKyDayBu });
                    table.ForeignKey(
                        name: "FK_LopHocPhanPhieuDangKyDayBu_LopHocPhan_IdLopHocPhan",
                        column: x => x.IdLopHocPhan,
                        principalTable: "LopHocPhan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LopHocPhanPhieuDangKyDayBu_PhieuDangKyDayBu_IdPhieuDangKyDayBu",
                        column: x => x.IdPhieuDangKyDayBu,
                        principalTable: "PhieuDangKyDayBu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BanSaoVBCTDiKem_IdPhieuDangKyDayBu",
                table: "BanSaoVBCTDiKem",
                column: "IdPhieuDangKyDayBu");

            migrationBuilder.CreateIndex(
                name: "IX_BoMon_IdKhoa",
                table: "BoMon",
                column: "IdKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_GiangVien_IdBoMon",
                table: "GiangVien",
                column: "IdBoMon");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhan_IdGiangVien",
                table: "LopHocPhan",
                column: "IdGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhan_IdHocPhan",
                table: "LopHocPhan",
                column: "IdHocPhan");

            migrationBuilder.CreateIndex(
                name: "IX_LopHocPhanPhieuDangKyDayBu_IdPhieuDangKyDayBu",
                table: "LopHocPhanPhieuDangKyDayBu",
                column: "IdPhieuDangKyDayBu");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BanSaoVBCTDiKem");

            migrationBuilder.DropTable(
                name: "LopHocPhanPhieuDangKyDayBu");

            migrationBuilder.DropTable(
                name: "LopHocPhan");

            migrationBuilder.DropTable(
                name: "PhieuDangKyDayBu");

            migrationBuilder.DropTable(
                name: "GiangVien");

            migrationBuilder.DropTable(
                name: "HocPhan");

            migrationBuilder.DropTable(
                name: "BoMon");

            migrationBuilder.DropTable(
                name: "Khoa");
        }
    }
}
