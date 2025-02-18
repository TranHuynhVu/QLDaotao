$(document).ready(function () {
    DKNDDBAddIntoSelect();
});
function DKNDDBAddIntoSelect() {
    $.ajax({
        url: "/Teacher/LopHocPhan/ListByTeacher",
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response && response.data) {
                var select = $("#DKNDDB_LHP");
                var thu = $("#DKNDDB_Thu");
                var tuTiet = $("#DKNDDB_TuTiet");
                var denTiet = $("#DKNDDB_DenTiet");

                // Hủy bỏ niceSelect nếu nó đang được áp dụng
                if ($.fn.niceSelect) {
                    select.niceSelect('destroy');
                    thu.niceSelect('destroy');
                    tuTiet.niceSelect('destroy');
                    denTiet.niceSelect('destroy');
                }

                thu.show();
                tuTiet.show();
                denTiet.show();
                select.show(); // Đảm bảo thẻ <select> được hiển thị
                select.empty(); // Xóa các option cũ

                // Thêm option mặc định
                select.append('<option value="">Chọn</option>');

                response.data.forEach(function (item) {
                    var option = `<option value="${item.id}">${item.id} - ${item.hocPhan.ten}</option>`;
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