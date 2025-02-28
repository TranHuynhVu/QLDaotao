var dataTable;

$(document).ready(function () {
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

