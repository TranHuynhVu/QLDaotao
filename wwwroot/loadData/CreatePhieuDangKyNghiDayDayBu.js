$(document).ready(function () {
    DKNDDB_DateValidation();
    DangKyNghiDayDayBu_AddLHP();
});

function DangKyNghiDayDayBu_AddLHP() {
    $("#DangKyNghiDayDayBu_AddLHP").click(function () {

        if (!DKNDDB_Validation()) {
            return; // Nếu validation thất bại, dừng lại
        }

        var tbody = $("#DangKyNghiDayDayBuLHPTable tbody");
        var submitBtn = $("#DKNDDB_submitBtn");

        if (tbody.find("tr").length === 0) {
            submitBtn.prop("disabled", true); // Vô hiệu hóa nút
        } else {
            submitBtn.prop("disabled", false); // Kích hoạt nút nếu có hàng
        }

        // Lấy dữ liệu từ input
        var idLopHocPhan = $("#DKNDDB_LHP").val();
        var ngayXinNghi = $("#DKNDDB_NgayXinNghi").val();
        var ngayDayBu = $("#DKNDDB_NgayDauBu").val();
        var thu = $("#DKNDDB_Thu").val();
        var tuTiet = $("#DKNDDB_TuTiet").val();
        var denTiet = $("#DKNDDB_DenTiet").val();
        var phong = $("#DKNDDB_Phong").val();
        var lyDo = $("#DKNDDB_LyDo").val();

        var tbody = $("#DangKyNghiDayDayBuLHPTable tbody");

        // Kiểm tra nếu có dòng thông báo thì xóa nó đi
        if (tbody.find("tr").length === 1 && tbody.find("tr td").length === 1) {
            tbody.empty();
        }

        // Tạo index mới dựa trên số lượng hàng hiện có trong bảng
        var index = tbody.find("tr").length + 1;

        // Tạo các input hidden
        var newInputs = `
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].IdLopHocPhan" value="${idLopHocPhan}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].NgayXinNghi" value="${ngayXinNghi}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].NgayDayBu" value="${ngayDayBu}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].Thu" value="${thu}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].TuTiet" value="${tuTiet}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].DenTiet" value="${denTiet}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].Phong" value="${phong}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index - 1}].LyDo" value="${lyDo}" />
        `;

        // Tạo dòng mới cho bảng
        var newRow = `
            <tr>
                <td>${index}</td>
                <td>${idLopHocPhan}</td>
                <td>${ngayXinNghi}</td>
                <td>${ngayDayBu}</td>
                <td>${thu}</td>
                <td>${tuTiet}</td>
                <td>${denTiet}</td>
                <td>${phong}</td>
                <td>${lyDo}</td>
                <td>
                    <button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">Xóa</button>
                </td>
            </tr>
        `;

        // Thêm vào danh sách input ẩn
        $("#DangKyNghiDayDayBu_ListLHP").append(newInputs);

        // Thêm dòng vào bảng
        $("#DangKyNghiDayDayBuLHPTable tbody").append(newRow);

        updateSoBuoiXinNghi();

        // Reset giá trị của các input về rỗng
        $("#DKNDDB_LHP").val("");
        $("#DKNDDB_NgayXinNghi").val("");
        $("#DKNDDB_NgayDauBu").val("");
        $("#DKNDDB_Thu").val("");
        $("#DKNDDB_TuTiet").val("");
        $("#DKNDDB_DenTiet").val("");
        $("#DKNDDB_Phong").val("");
        $("#DKNDDB_LyDo").val("");

        // reset giới hạn của ngày nghỉ và ngày dạy bù
        $("#DKNDDB_NgayXinNghi, #DKNDDB_NgayDauBu").removeAttr("min max");

        // reset disable cho các option trong select
        $("#DKNDDB_TuTiet option, #DKNDDB_DenTiet option").prop("disabled", false);

        // Disable LHP vừa chọn
        $("#DKNDDB_LHP option[value='" + idLopHocPhan + "']").prop("disabled", true);
    });
}

// Hàm xóa dòng khi bấm nút "Xóa"
function removeRow(button) {
    var tbody = $("#DangKyNghiDayDayBuLHPTable tbody");
    var row = $(button).closest("tr");
    var rowIndex = row.index(); // Lấy vị trí của hàng trong tbody
    var submitBtn = $("#DKNDDB_submitBtn");



    // Xóa hàng trong bảng
    row.remove();

    // Xóa các input ẩn tương ứng trong #DangKyNghiDayDayBu_ListLHP
    $("#DangKyNghiDayDayBu_ListLHP").find(`input[name^="LopHocPhanNghiDayDayBu[${rowIndex}]"]`).remove();

    // Cập nhật lại STT trong bảng
    tbody.find("tr").each(function (index) {
        $(this).find("td:first").text(index + 1);
    });

    // 🛠 Cập nhật lại chỉ mục của các input ẩn sau khi xóa
    var inputsContainer = $("#DangKyNghiDayDayBu_ListLHP");
    var inputs = inputsContainer.children("input").toArray(); // Lấy danh sách tất cả input

    inputsContainer.empty(); // Xóa toàn bộ input cũ

    inputs.forEach((input, newIndex) => {
        var newName = $(input).attr("name").replace(/\[\d+\]/, `[${Math.floor(newIndex / 8)}]`); // 8 input ẩn mỗi dòng
        $(input).attr("name", newName);
        inputsContainer.append(input); // Thêm lại input với tên mới
    });

    // Nếu bảng không còn dòng nào, thêm lại dòng thông báo
    if (tbody.find("tr").length === 0) {
        tbody.append(`
            <tr>
                <td colspan="10" class="text-center">Hiện chưa có LHP đăng ký nghỉ dạy - dạy bù nào !</td>
            </tr>
        `);

        submitBtn.prop("disabled", true); // Vô hiệu hóa nút
    }

    updateSoBuoiXinNghi();

    // Lấy lại ID của lớp học phần từ dòng bị xóa
    var idLopHocPhan = row.find("td:nth-child(2)").text();

    // Enable option tương ứng trong select
    $("#DKNDDB_LHP option[value='" + idLopHocPhan + "']").prop("disabled", false);
}

function updateSoBuoiXinNghi() {
    var ngayXinNghiList = new Set(); // Sử dụng Set để lấy danh sách ngày không trùng lặp

    $("#DangKyNghiDayDayBuLHPTable tbody tr").each(function () {
        var ngayXinNghi = $(this).find("td:nth-child(3)").text();
        if (ngayXinNghi) {
            ngayXinNghiList.add(ngayXinNghi);
        }
    });

    $("#DKNDDB_SoBuoiXinNghi").val(ngayXinNghiList.size); // Cập nhật số buổi nghỉ
}


function DKNDDB_Validation() {
    let isValid = true;
    let errorMessage = ""; // Chuỗi chứa lỗi

    // Danh sách các input cần kiểm tra
    let fields = [
        { id: "DKNDDB_LHP", message: "Vui lòng chọn lớp học phần." },
        { id: "DKNDDB_NgayXinNghi", message: "Vui lòng chọn ngày xin nghỉ." },
        { id: "DKNDDB_NgayDauBu", message: "Vui lòng chọn ngày dạy bù." },
        { id: "DKNDDB_Thu", message: "Vui lòng chọn thứ." },
        { id: "DKNDDB_TuTiet", message: "Vui lòng chọn tiết bắt đầu." },
        { id: "DKNDDB_DenTiet", message: "Vui lòng chọn tiết kết thúc." },
        { id: "DKNDDB_Phong", message: "Vui lòng nhập phòng học." },
        { id: "DKNDDB_LyDo", message: "Vui lòng nhập lý do." }
    ];

    // Kiểm tra từng input
    for (let field of fields) {
        let value = $("#" + field.id).val().trim();
        if (value === "") {
            errorMessage = field.message; // Lưu thông báo lỗi
            $("#" + field.id).focus(); // Focus vào ô bị thiếu
            isValid = false;
            break; // Dừng vòng lặp khi gặp lỗi đầu tiên
        }
    }

    // Hiển thị lỗi trong thẻ span DKNDDB_error
    $("#DKNDDB_error").text(errorMessage);

    return isValid;
}

function DKNDDB_DateValidation() {
    $("#DKNDDB_NgayXinNghi").change(function () {
        let ngayXinNghi = $(this).val();
        let ngayDayBu = $("#DKNDDB_NgayDauBu").val();

        if (ngayXinNghi) {
            $("#DKNDDB_NgayDauBu").attr("min", getNextDate(ngayXinNghi));

            // Nếu ngày dạy bù đã chọn không hợp lệ thì reset
            if (ngayDayBu && ngayDayBu <= ngayXinNghi) {
                $("#DKNDDB_error").text("Ngày dạy bù phải lớn hơn ngày xin nghỉ!");
                $("#DKNDDB_NgayDauBu").val("");
            }
        } else {
            $("#DKNDDB_NgayDauBu").removeAttr("min");
        }
    });

    $("#DKNDDB_NgayDauBu").change(function () {
        let ngayXinNghi = $("#DKNDDB_NgayXinNghi").val();
        let ngayDayBu = $(this).val();

        if (ngayDayBu) {
            $("#DKNDDB_NgayXinNghi").attr("max", getPreviousDate(ngayDayBu));

            // Nếu ngày xin nghỉ đã chọn không hợp lệ thì reset
            if (ngayXinNghi && ngayXinNghi >= ngayDayBu) {
                $("#DKNDDB_error").text("Ngày xin nghỉ phải nhỏ hơn ngày dạy bù!");
                $("#DKNDDB_NgayXinNghi").val("");
            } else {
                $("#DKNDDB_error").text(""); // Xóa lỗi nếu hợp lệ
            }

            let thu = new Date(ngayDayBu).getDay();
            let selectThu = $("#DKNDDB_Thu");

            if (thu === 0) { // Nếu là Chủ Nhật
                $("#DKNDDB_error").text("Không thể dạy bù vào Chủ Nhật!");
                $(this).val(""); // Reset ngày dạy bù
                selectThu.val(""); // Reset dropdown thứ
            } else {
                selectThu.val(thu + 1).change(); // Gán giá trị thứ vào dropdown
            }
        } else {
            $("#DKNDDB_NgayXinNghi").removeAttr("max");
        }
    });

    $("#DKNDDB_TuTiet, #DKNDDB_DenTiet").change(function () {
        var tuTiet = parseInt($("#DKNDDB_TuTiet").val()) || 0;
        var denTiet = parseInt($("#DKNDDB_DenTiet").val()) || 0;

        $("#DKNDDB_TuTiet option").prop("disabled", false);
        $("#DKNDDB_DenTiet option").prop("disabled", false);

        if (tuTiet > 0) {
            $("#DKNDDB_DenTiet option").each(function () {
                var value = parseInt($(this).val());
                if (value <= tuTiet) {
                    $(this).prop("disabled", true);
                }
            });
        }

        if (denTiet > 0) {
            $("#DKNDDB_TuTiet option").each(function () {
                var value = parseInt($(this).val());
                if (value >= denTiet) {
                    $(this).prop("disabled", true);
                }
            });
        }
    });
}

function getNextDate(date) {
    let d = new Date(date);
    d.setDate(d.getDate() + 1); // Cộng thêm 1 ngày
    return d.toISOString().split("T")[0]; // Format YYYY-MM-DD
}

function getPreviousDate(date) {
    let d = new Date(date);
    d.setDate(d.getDate() - 1); // Trừ 1 ngày
    return d.toISOString().split("T")[0];
}
