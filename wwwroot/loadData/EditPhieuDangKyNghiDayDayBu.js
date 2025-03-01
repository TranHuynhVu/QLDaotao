$(document).ready(function () {
    DangKyNghiDayDayBuEdit_AddLHP();
    DangKyNghiDayDayBuEdit_EditLHP();
    DKNDDB_DateValidation();
    CancelForm();
    OpenForm();
});

function DangKyNghiDayDayBuEdit_AddLHP() {
    $("#DangKyNghiDayDayBuEdit_AddLHP").click(function () {

        if (!DKNDDB_Validation()) {
            return; // Nếu validation thất bại, dừng lại
        }

        $("#DKNDDB_submitBtn").prop("disabled", false);

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
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].Id" value="${idLopHocPhan}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].NgayXinNghiDT" value="${ngayXinNghi}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].NgayDayBuDT" value="${ngayDayBu}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].NgayXinNghi" value="0001-01-01" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].NgayDayBu" value="0001-01-01" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].ThuDayBu" value="${thu}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].TuTietDayBu" value="${tuTiet}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].DenTietDayBu" value="${denTiet}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].Phong" value="${phong}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].LyDo" value="${lyDo}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].TenHocPhan" value="temp" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].ThuTKB" value="0" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].TuTietTKB" value="0" />
            <input type="hidden" name="LopHocPhanNghiDayDayBuVM[${index - 1}].DenTietTKB" value="0" />
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
                    <button type="button" class="btn btn-primary btn-sm" onclick="EditRow(this)">Sửa</button>
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

function DangKyNghiDayDayBuEdit_EditLHP() {
    $("#DangKyNghiDayDayBuEdit_EditLHP").click(function () {
        // Kiểm tra validate
        if (!DKNDDB_Validation()) {
            return; // Nếu validation thất bại, dừng lại
        }

        $("#DKNDDB_submitBtn").prop("disabled", false);

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

        // Lấy chỉ số của hàng đang được chỉnh sửa (đã lưu khi bấm nút Sửa)
        var editingIndex = $("#DangKyNghiDayDayBuLHPTable").attr("data-editing-row");
        if (editingIndex === undefined) {
            console.error("Không xác định được hàng cần chỉnh sửa.");
            return;
        }
        editingIndex = parseInt(editingIndex, 10);

        // Cập nhật giá trị cho các input hidden tương ứng (dựa theo index)
        var container = $("#DangKyNghiDayDayBu_ListLHP");
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].IdLopHocPhan"]`).val(idLopHocPhan);
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].NgayXinNghiDT"]`).val(ngayXinNghi);
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].NgayDayBuDT"]`).val(ngayDayBu);
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].ThuDayBu"]`).val(thu);
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].TuTietDayBu"]`).val(tuTiet);
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].DenTietDayBu"]`).val(denTiet);
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].Phong"]`).val(phong);
        container.find(`input[name="LopHocPhanNghiDayDayBuVM[${editingIndex}].LyDo"]`).val(lyDo);


        // Cập nhật nội dung của dòng trong bảng
        // Lưu ý: Nếu bảng không có dòng tiêu đề trong tbody, ta có thể dùng index trực tiếp.
        var row = tbody.find("tr").eq(editingIndex);
        // Cập nhật các cột (với cấu trúc đã tạo: số thứ tự ở td:eq(0) không thay đổi)
        row.find("td:eq(1)").text(idLopHocPhan);
        row.find("td:eq(2)").text(formatDate(ngayXinNghi));
        row.find("td:eq(3)").text(formatDate(ngayDayBu));
        row.find("td:eq(4)").text(thu);
        row.find("td:eq(5)").text(tuTiet);
        row.find("td:eq(6)").text(denTiet);
        row.find("td:eq(7)").text(phong);
        row.find("td:eq(8)").text(lyDo);

        // Sau khi chỉnh sửa, reset lại form:
        $("#DKNDDB_LHP").val("");
        $("#DKNDDB_NgayXinNghi").val("");
        $("#DKNDDB_NgayDauBu").val("");
        $("#DKNDDB_Thu").val("");
        $("#DKNDDB_TuTiet").val("");
        $("#DKNDDB_DenTiet").val("");
        $("#DKNDDB_Phong").val("");
        $("#DKNDDB_LyDo").val("");

        // Reset giới hạn của ngày và các option của select
        $("#DKNDDB_NgayXinNghi, #DKNDDB_NgayDauBu").removeAttr("min max");
        $("#DKNDDB_TuTiet option, #DKNDDB_DenTiet option").prop("disabled", false);

        // Ẩn form chỉnh sửa và hiện lại nút mở form (nếu cần)
        $("#DKNDDB_AddForm").prop("hidden", true);
        $("#DangKyNghiDayDayBuEdit_OpenAddLHP").prop("hidden", false);

        // Xóa thuộc tính data-editing-row để biết rằng không còn dòng nào đang được chỉnh sửa
        $("#DangKyNghiDayDayBuLHPTable").removeAttr("data-editing-row");
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
    $("#DangKyNghiDayDayBu_ListLHP").find(`input[name^="LopHocPhanNghiDayDayBuVM[${rowIndex}]"]`).remove();

    // Cập nhật lại STT trong bảng
    tbody.find("tr").each(function (index) {
        $(this).find("td:first").text(index + 1);
    });

    // 🛠 Cập nhật lại chỉ mục của các input ẩn sau khi xóa
    var inputsContainer = $("#DangKyNghiDayDayBu_ListLHP");
    var inputs = inputsContainer.children("input").toArray(); // Lấy danh sách tất cả input

    inputsContainer.empty(); // Xóa toàn bộ input cũ

    inputs.forEach((input, newIndex) => {
        var newName = $(input).attr("name").replace(/\[\d+\]/, `[${Math.floor(newIndex / 14)}]`); // 14 input ẩn mỗi dòng
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
    else {
        submitBtn.prop("disabled", false);
    }
  
    updateSoBuoiXinNghi();

    // Lấy lại ID của lớp học phần từ dòng bị xóa
    var idLopHocPhan = row.find("td:nth-child(2)").text();

    // Enable option tương ứng trong select
    $("#DKNDDB_LHP option[value='" + idLopHocPhan + "']").prop("disabled", false);

    
}

function CancelForm() {
    $("#DangKyNghiDayDayBuEdit_CloseAddVBCT").click(function () {
        $(".container-dragdrop").prop("hidden", true);
        $("#DangKyNghiDayDayBuEdit_OpenAddVBCT").prop("hidden", false);
        $("#DangKyNghiDayDayBuEdit_CloseAddVBCT").prop("hidden", true);

        var fileInput = document.querySelector(".file-selector-input-dragdrop");
        if (fileInput) {
            fileInput.value = ""; // Reset input file để xóa file đã chọn
        }
    });

    $("#DangKyNghiDayDayBuEdit_Cancel").click(function () {
       
        // Reset giá trị của các input về rỗng
        $("#DKNDDB_LHP").val("");
        $("#DKNDDB_NgayXinNghi").val("");
        $("#DKNDDB_NgayDauBu").val("");
        $("#DKNDDB_Thu").val("");
        $("#DKNDDB_TuTiet").val("");
        $("#DKNDDB_DenTiet").val("");
        $("#DKNDDB_Phong").val("");
        $("#DKNDDB_LyDo").val("");
        $("#DKNDDB_error").text("");

        // reset giới hạn của ngày nghỉ và ngày dạy bù
        $("#DKNDDB_NgayXinNghi, #DKNDDB_NgayDauBu").removeAttr("min max");

        // reset disable cho các option trong select
        $("#DKNDDB_TuTiet option, #DKNDDB_DenTiet option").prop("disabled", false);

        // Thêm thuộc tính hidden cho #DKNDDB_AddForm để ẩn form
        $("#DKNDDB_AddForm").prop("hidden", true);

        $("#DangKyNghiDayDayBuEdit_OpenAddLHP").prop("hidden", false);
    });
}

function OpenForm() {
    $("#DangKyNghiDayDayBuEdit_OpenAddVBCT").click(function () {
        $(".container-dragdrop").prop("hidden", false);
        $("#DangKyNghiDayDayBuEdit_OpenAddVBCT").prop("hidden", true);
        $("#DangKyNghiDayDayBuEdit_CloseAddVBCT").prop("hidden", false);
    });

    $("#DangKyNghiDayDayBuEdit_OpenAddLHP").click(function () {

        DKNDDB_ValidLHP();

        $("#DKNDDB_error").text("");
        $("#DKNDDB_AddForm").prop("hidden", false);
        $("#DangKyNghiDayDayBuEdit_EditLHP").prop("hidden", true);
        $("#DangKyNghiDayDayBuEdit_AddLHP").prop("hidden", false);
        $("#DangKyNghiDayDayBuEdit_OpenAddLHP").prop("hidden", true);

        // Sau khi chỉnh sửa, reset lại form:
        $("#DKNDDB_LHP").val("");
        $("#DKNDDB_NgayXinNghi").val("");
        $("#DKNDDB_NgayDauBu").val("");
        $("#DKNDDB_Thu").val("");
        $("#DKNDDB_TuTiet").val("");
        $("#DKNDDB_DenTiet").val("");
        $("#DKNDDB_Phong").val("");
        $("#DKNDDB_LyDo").val("");

        // Reset giới hạn của ngày và các option của select
        $("#DKNDDB_NgayXinNghi, #DKNDDB_NgayDauBu").removeAttr("min max");
        $("#DKNDDB_TuTiet option, #DKNDDB_DenTiet option").prop("disabled", false);

    });

}

function EditRow(button) {
    var row = $(button).closest("tr"); // Lấy hàng chứa button được bấm

    $("#DangKyNghiDayDayBuLHPTable").attr("data-editing-row", row.index());

    $("#DKNDDB_error").text("");

    $("#DKNDDB_AddForm").prop("hidden", false);
    $("#DangKyNghiDayDayBuEdit_EditLHP").prop("hidden", false);
    $("#DangKyNghiDayDayBuEdit_AddLHP").prop("hidden", true);
    $("#DangKyNghiDayDayBuEdit_OpenAddLHP").prop("hidden", false);

    // Lấy dữ liệu từ các cột trong hàng
    var idLopHocPhan = row.find("td:eq(1)").text().trim();
    var ngayXinNghi = row.find("td:eq(2)").text().trim();
    var ngayDayBu = row.find("td:eq(3)").text().trim();
    var thu = row.find("td:eq(4)").text().trim();
    var tuTiet = row.find("td:eq(5)").text().trim();
    var denTiet = row.find("td:eq(6)").text().trim();
    var phong = row.find("td:eq(7)").text().trim();
    var lyDo = row.find("td:eq(8)").text().trim();

    function convertDate(dateStr) {
        var parts = dateStr.split('-');
        // Kiểm tra xem định dạng ban đầu có đúng dd/mm/yyyy không
        if (parts.length === 3) {
            // parts[0]: dd, parts[1]: mm, parts[2]: yyyy
            return parts[2] + '-' + parts[1] + '-' + parts[0];
        }
        return dateStr; // Nếu không đúng định dạng, trả về nguyên chuỗi
    }

    ngayXinNghi = convertDate(ngayXinNghi);
    ngayDayBu = convertDate(ngayDayBu);

    // Đổ dữ liệu vào các input
    $("#DKNDDB_LHP").val(idLopHocPhan);
    $("#DKNDDB_NgayXinNghi").val(ngayXinNghi);
    $("#DKNDDB_NgayDauBu").val(ngayDayBu);
    $("#DKNDDB_Thu").val(thu);
    $("#DKNDDB_TuTiet").val(tuTiet);
    $("#DKNDDB_DenTiet").val(denTiet);
    $("#DKNDDB_Phong").val(phong);
    $("#DKNDDB_LyDo").val(lyDo);
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

function DKNDDB_ValidLHP() {
    // Duyệt qua các dòng trong bảng để lấy danh sách LHP đã thêm
    $("#DangKyNghiDayDayBuLHPTable tbody tr").each(function () {
        var idLopHocPhan = $(this).find("td:eq(1)").text().trim(); // Lấy ID LHP từ cột thứ 2

        if (idLopHocPhan) {
            // Disable option trong select nếu đã tồn tại trong bảng
            $("#DKNDDB_LHP option[value='" + idLopHocPhan + "']").prop("disabled", true);
        }
    });
}

function DeleteImg(i) {
    // Xóa container hình ảnh tương ứng
    let imageContainers = document.querySelectorAll('.image-container');
    if (imageContainers[i]) {
        imageContainers[i].remove();
    }

    // Xóa các input liên quan
    document.querySelectorAll(`#DangKyNghiDayDayBu_ListBSVBCT input[name^="BanSaoVBCTDiKem[${i}]"]`)
        .forEach(input => input.remove());

    // Cập nhật lại thứ tự cho các phần tử còn lại
    UpdateIndex();
    $('#DKNDDB_submitBtn').prop("disabled", false);
}

function UpdateIndex() {
    let imageContainers = document.querySelectorAll('.image-container');
    let inputs = document.querySelectorAll('#DangKyNghiDayDayBu_ListBSVBCT input');

    // Cập nhật lại chỉ mục trong image-container
    imageContainers.forEach((container, index) => {
        let button = container.querySelector('.delete_img');
        if (button) {
            button.setAttribute('onclick', `DeleteImg(${index})`);
        }
    });

    // Cập nhật lại chỉ mục trong các input
    let groupedInputs = {};
    inputs.forEach(input => {
        let match = input.name.match(/BanSaoVBCTDiKem\[(\d+)\]\.(.+)/);
        if (match) {
            let oldIndex = match[1];
            let field = match[2];

            if (!groupedInputs[oldIndex]) {
                groupedInputs[oldIndex] = [];
            }
            groupedInputs[oldIndex].push(input);
        }
    });

    let newIndex = 0;
    Object.values(groupedInputs).forEach(inputGroup => {
        inputGroup.forEach(input => {
            let field = input.name.match(/\.(.+)/)[1];
            input.name = `BanSaoVBCTDiKem[${newIndex}].${field}`;
        });
        newIndex++;
    });
}

function formatDate(date) {
    if (!date) return ""; // Kiểm tra nếu date không có giá trị
    var d = new Date(date);
    var day = ("0" + d.getDate()).slice(-2); // Lấy ngày (dd)
    var month = ("0" + (d.getMonth() + 1)).slice(-2); // Lấy tháng (mm) (Lưu ý: getMonth() tính từ 0)
    var year = d.getFullYear(); // Lấy năm (yyyy)
    return `${day}-${month}-${year}`;
}
