var dataTable;

$(document).ready(function () {
    DKNDDB_DateValidation()
    loadDataTable();
    $('#filterBtn').on('click', function () {
        var fromDate = $('#fromDate').val();
        var toDate = $('#toDate').val();
        var status = $('#status').val();
        var khoa = $('#DKNDDB_Khoa').val();
        // Gọi lại DataTable với các tham số lọc
        dataTable.ajax.reload();
    });
    DangKyNghiDayDayBu_AddLHP();
    AddVBCTDiKem();
});
function loadDataTable() {
    dataTable = $('#PhieuDangKyNghiDayDayBuTable').DataTable({
        "ajax": {
            "url": "/Admin/NghiDayDayBu/List",
            "type": "GET",
            "datatype": "json",
            "data": function (d) {
                // Thêm các tham số lọc vào yêu cầu
                d.fromDate = $('#fromDate').val();
                d.toDate = $('#toDate').val();
                d.status = $('#status').val();
                d.khoa = $('#DKNDDB_Khoa').val();
            }
        },
        "columns": [
            { "data": null, "render": function (data, type, row, meta) { return meta.row + 1; }, "width": "5%", "className": "text-center" },
            { "data": "maGV", "width": "10%" },
            { "data": "tenGV", "width": "15%" },
            { "data": "khoa", "width": "12%" },
            { "data": "soBuoiXinNghi", "width": "10%", "className": "text-center" },
            {
                "data": "trangThai",
                "render": function (data, type, row) {
                    // Xác định các trạng thái cần disabled
                    const isDisabled = data == -1 || data == 3 || data == 4;
                    const id = row.id;
                    return `
                        <select class="form-select form-select-sm" change-status="/Admin/NghiDayDayBu/Edit/${id}" ${isDisabled ? 'disabled' : ''}
                        ${data == 0 ? 'style="color:black !important; background-color:#F3F3E0;"' :
                            data == 1 ? 'style="color:black !important; background-color:#0dcaf0;"' :
                                data == 2 ? 'style="color:white !important; background-color:#0d6efd;"' :
                                    data == 3 ? 'style="color:white !important; background-color:#198754;"' :
                                        data == 4 ? 'style="color:white !important; background-color:#ffc107;"' :
                                            'style="color:white !important; background-color:#dc3545;"'}>
    
                        ${isDisabled
                            ? `<option value="${data}" selected>${data == -1 ? 'Đã từ chối' : data == 3 ? 'Đã nhận' : 'Hết hạn'}</option>`
                            : `
                            <option value="0" ${data == 0 ? 'selected' : ''}>Chờ xử lý</option> 
                            <option value="1" ${data == 1 ? 'selected' : ''}>Đang xử lý</option>
                            <option value="2" ${data == 2 ? 'selected' : ''}>Đã xử lý</option> 
                            <option value="3" ${data == 3 ? 'selected' : ''}>Đã nhận</option>
                            <option value="4" ${data == 4 ? 'selected' : ''}>Hết hạn</option>
                            `
                        }
                        </select>`;
                },
                "width": "10%",
                "className": "text-center"
            },
            {
                "data": "id",
                "render": function (data, type, row) {
                    const isDisable = row.trangThai == -1 || row.trangThai == 3 || row.trangThai == 4;
                    return ` 
                        <a title="Xem thông tin" href="/Admin/NghiDayDayBu/Details/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                            <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M22.4541 11.3918C22.7819 11.7385 22.7819 12.2615 22.4541 12.6082C21.0124 14.1335 16.8768 18 12 18C7.12317 18 2.98759 14.1335 1.54586 12.6082C1.21811 12.2615 1.21811 11.7385 1.54586 11.3918C2.98759 9.86647 7.12317 6 12 6C16.8768 6 21.0124 9.86647 22.4541 11.3918Z" stroke="currentColor"></path>
                                <circle cx="12" cy="12" r="3.5" stroke="currentColor"></circle>
                                <circle cx="13.5" cy="10.5" r="1.5" fill="currentColor"></circle>
                            </svg>
                        </a>
                         <a title="Xuất PDF" href="/Admin/NghiDayDayBu/ExportPDF/${data}" class='btn btn-success text-white btn-pd ml-0 mr-0' style='cursor:pointer'>
                            <svg width="18" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                            <path fill="white" d="M0 64C0 28.7 28.7 0 64 0L224 0l0 128c0 17.7 14.3 32 32 32l128 0 0 144-208 0c-35.3 0-64 28.7-64 64l0 144-48 0c-35.3 0-64-28.7-64-64L0 64zm384 64l-128 0L256 0 384 128zM176 352l32 0c30.9 0 56 25.1 56 56s-25.1 56-56 56l-16 0 0 32c0 8.8-7.2 16-16 16s-16-7.2-16-16l0-48 0-80c0-8.8 7.2-16 16-16zm32 80c13.3 0 24-10.7 24-24s-10.7-24-24-24l-16 0 0 48 16 0zm96-80l32 0c26.5 0 48 21.5 48 48l0 64c0 26.5-21.5 48-48 48l-32 0c-8.8 0-16-7.2-16-16l0-128c0-8.8 7.2-16 16-16zm32 128c8.8 0 16-7.2 16-16l0-64c0-8.8-7.2-16-16-16l-16 0 0 96 16 0zm80-112c0-8.8 7.2-16 16-16l48 0c8.8 0 16 7.2 16 16s-7.2 16-16 16l-32 0 0 32 32 0c8.8 0 16 7.2 16 16s-7.2 16-16 16l-32 0 0 48c0 8.8-7.2 16-16 16s-16-7.2-16-16l0-64 0-64z"/>
                            </svg>
                        </a>
                         ${!isDisable ? `
                        <a title="Từ chối" onclick="Deny('/Admin/NghiDayDayBu/Deny/${data}')" class='btn btn-danger btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                            <svg width="18" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 384 512">
                            <path fill="white" d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z"/>
                            </svg>
                        </a>
                        ` : ''}
                    `;
                }, "width": "18%", "className": "text-center"
            }
        ],
        "language": {
            "emptyTable": "Không có dữ liệu",
            "lengthMenu": "_MENU_",
            "zeroRecords": "Không tìm thấy",
            "info": "Đang hiển thị trang _PAGE_ của _PAGES_",
            "infoEmpty": "Không có dữ liệu",
            "infoFiltered": "(được lọc từ _MAX_ tổng dữ liệu)",
            "search": "",
            searchPlaceholder: 'Tìm kiếm',
            "loadingRecords": "Loading...",
        },
        "width": "100%",
        lengthChange: true,
        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "Tất cả"]],
        pagingType: "full_numbers",
        stateSave: true,
        searching: true,
        ordering: true,
        info: true
    });
    dataTable.ajax.reload();
}

function Deny(url) {
    Swal.fire({
        title: 'Nhập lý do từ chối',
        icon: 'question',
        html: `<textarea id="reason" class="swal2-textarea" 
               placeholder="Nhập lý do..." 
               style="width:100%; height:100px;"></textarea>`,
        showCancelButton: true,
        confirmButtonText: 'Gửi',
        cancelButtonText: 'Hủy',
        confirmButtonColor: '#28A745',
        cancelButtonColor: '#d33',
        focusConfirm: false,
        preConfirm: () => {
            const reason = document.getElementById('reason').value.trim();
            if (!reason) {
                Swal.showValidationMessage('Vui lòng nhập lý do!');
                return false;
            }
            return reason;
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const currentPage = dataTable.page();
            $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify({ reason: result.value }),
                contentType: "application/json",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                    dataTable.ajax.reload(() => {
                        dataTable.page(currentPage).draw(false);
                    });
                },
                error: function () {
                    toastr.error('Có lỗi xảy ra, vui lòng thử lại!');
                }
            });
        }
    });
}

$(document).on('focus', '[change-status]', function () {
    const selectElement = $(this);
    const originalStatus = selectElement.val();

    selectElement.data('original-status', originalStatus);
});
$(document).on('change', '[change-status]', function () {

    const selectElement = $(this);
    const selectedStatus = selectElement.val();
    var url = selectElement.attr('change-status');
    const originalStatus = selectElement.data('original-status');

    if (selectedStatus === originalStatus) return;

    Swal.fire({
        title: 'Bạn có muốn đổi trạng thái?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Xác nhận',
        cancelButtonText: 'Hủy',
        confirmButtonColor: '#28A745',
        cancelButtonColor: '#d33',
        focusConfirm: false
    }).then((result) => {
        if (result.isConfirmed) {
            url += `/${selectedStatus}`;
            console.log(url);
            const currentPage = dataTable.page();
            $.ajax({
                type: "POST",
                url: url,
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                    } else {
                        toastr.error(response.message);
                    }
                    dataTable.ajax.reload(() => {
                        dataTable.page(currentPage).draw(false);
                    });
                },
                error: function () {
                    toastr.error('Có lỗi xảy ra, vui lòng thử lại!');
                }
            });
        } else {
            selectElement.val(originalStatus).trigger('change');
        }
    });
});

function DangKyNghiDayDayBu_AddLHP() {
    $("#DangKyNghiDayDayBu_AddLHP").click(function () {

        if (!DKNDDB_Validation()) {
            return; // Nếu validation thất bại, dừng lại
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
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].IdLopHocPhan" value="${idLopHocPhan}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].NgayXinNghi" value="${ngayXinNghi}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].NgayDayBu" value="${ngayDayBu}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].Thu" value="${thu}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].TuTiet" value="${tuTiet}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].DenTiet" value="${denTiet}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].Phong" value="${phong}" />
            <input type="hidden" name="LopHocPhanNghiDayDayBu[${index-1}].LyDo" value="${lyDo}" />
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

function AddVBCTDiKem() {
    var fileInput = $("#DangKyNghiDayDayBu_AddDoc");
    var previewContainer = $("#DKNDDB_BanSaoVBCTDiKem");
    var selectedFiles = [];

    fileInput.on("change", function (event) {
        var files = Array.from(event.target.files);
        selectedFiles = selectedFiles.concat(files); // Thêm ảnh mới vào danh sách
        renderPreview();
    });

    function renderPreview() {
        previewContainer.empty(); // Xóa danh sách trước khi render lại

        selectedFiles.forEach((file, index) => {
            if (!file.type.startsWith("image/")) {
                alert("Vui lòng chọn file ảnh!");
                return;
            }

            var reader = new FileReader();
            reader.onload = function (e) {
                var imgWrapper = $("<div>").css({
                    "display": "inline-block",
                    "position": "relative",
                    "margin": "5px"
                });

                var imgElement = $("<img>")
                    .attr("src", e.target.result)
                    .css({
                        "width": "100px",
                        "height": "100px",
                        "border": "1px solid #ddd",
                        "border-radius": "5px",
                        "cursor": "pointer"
                    })
                    .on("click", function () {
                        $("#previewImage").attr("src", e.target.result);
                        $("#imageModal").modal("show");
                    });

                var removeBtn = $("<button>")
                    .text("✖")
                    .css({
                        "position": "absolute",
                        "top": "0",
                        "right": "0",
                        "background": "gray",
                        "color": "white",
                        "border": "none",
                        "border-radius": "50%",
                        "width": "20px",
                        "height": "20px",
                        "cursor": "pointer",
                        "font-size": "12px"
                    })
                    .on("click", function () {
                        selectedFiles.splice(index, 1); // Xóa file khỏi danh sách
                        updateFileInput(); // Cập nhật input file
                        renderPreview(); // Cập nhật danh sách hiển thị
                    });

                imgWrapper.append(imgElement).append(removeBtn);
                previewContainer.append(imgWrapper);
            };

            reader.readAsDataURL(file);
        });

        updateFileInput(); // Cập nhật input file khi danh sách thay đổi
    }

    function updateFileInput() {
        var dataTransfer = new DataTransfer();
        selectedFiles.forEach(file => dataTransfer.items.add(file));
        fileInput[0].files = dataTransfer.files;

        // Nếu không còn file nào, reset input file
        if (selectedFiles.length === 0) {
            fileInput.val("");
        }
    }
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
                selectThu.val(thu+1).change(); // Gán giá trị thứ vào dropdown
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

$('#exportPDFsBtn').on('click', function () {
    var url = '/Admin/NghiDayDayBu/ExportPDFs';
    $.ajax({
        type: "GET",
        url: url,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            const currentPage = dataTable.page();
            var blob = new Blob([data], { type: 'application/pdf' });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = downloadUrl;
            a.download = "NghiDayDayBu_GiangVien.pdf"; // Đặt tên file cố định
            document.body.appendChild(a);
            a.click();
            URL.revokeObjectURL(downloadUrl);
            dataTable.ajax.reload(() => {
                dataTable.page(currentPage).draw(false);
            });
        },
        error: function () {
            toastr.error('Không có phiếu để xuất!');
        }
    });
});
