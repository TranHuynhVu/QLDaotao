$(document).ready(function () {
    DKNDDBAddKhoaIntoSelect();
});
function DKNDDBAddKhoaIntoSelect() {
    $.ajax({
        url: "/Admin/Khoa/List",
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response && response.data) {
                var select = $("#DKNDDB_Khoa");
        
                select.empty(); // Xóa các option cũ

                // Thêm option mặc định
                select.append('<option value="">Tất cả</option>');

                response.data.forEach(function (item) {
                    var option = `<option value="${item.ten}">${item.ten}</option>`;
                    select.append(option);
                });

            } else {
                console.error("Dữ liệu trả về không hợp lệ.");
            }
        },
        error: function (xhr, status, error) {
            console.error("Lỗi khi lấy danh sách lớp học phần:", error);
        }
    });
}