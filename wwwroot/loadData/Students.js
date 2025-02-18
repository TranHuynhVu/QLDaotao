var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#loadDataStudent').DataTable({
        "ajax": {
            "url": "/admin/students/list",
            "type": "GET",
            "datatype": "json",
            "data": function (d) {
                d.pageIndex = d.start / d.length + 1;
                d.pageSize = d.length;
            }
        },
        "columns": [
            { "data": null, "render": function (data, type, row, meta) { return meta.row + 1; }, "width": "5%", "className": "text-center" },
            { "data": "maSV", "width": "5%" },
            { "data": "tenViet", "width": "10%" },
            { "data": "tel", "width": "8%" },
            {
                "data": null,
                "render": function (data, type, row) {
                    var checked = row.isVisible == 1 ? 'checked' : '';
                    return '<div class="mb-3 form-switch">' +
                        '<input class="form-check-input" type="checkbox" id="statusCheckbox_' + row.id + '" ' + checked + '>' +
                        '</div>';
                },
                "width": "10%",
                "className": "text-center"
            },
            {
                "data": "id",
                "render": function (data) {
                    return ` 
          <a title="Chỉnh sửa thông tin" href="/admin/student/edit/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                 <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                             <path d="M11.4925 2.78906H7.75349C4.67849 2.78906 2.75049 4.96606 2.75049 8.04806V16.3621C2.75049 19.4441 4.66949 21.6211 7.75349 21.6211H16.5775C19.6625 21.6211 21.5815 19.4441 21.5815 16.3621V12.3341" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                             <path fill-rule="evenodd" clip-rule="evenodd" d="M8.82812 10.921L16.3011 3.44799C17.2321 2.51799 18.7411 2.51799 19.6721 3.44799L20.8891 4.66499C21.8201 5.59599 21.8201 7.10599 20.8891 8.03599L13.3801 15.545C12.9731 15.952 12.4211 16.181 11.8451 16.181H8.09912L8.19312 12.401C8.20712 11.845 8.43412 11.315 8.82812 10.921Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                             <path d="M15.1655 4.60254L19.7315 9.16854" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                         </svg>
             </a>
             <a title="Đặt lại mật khẩu" href="/admin/student/reset-password/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                  <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                         <path fill-rule="evenodd" clip-rule="evenodd" d="M16.334 2.75H7.665C4.644 2.75 2.75 4.889 2.75 7.916V16.084C2.75 19.111 4.635 21.25 7.665 21.25H16.333C19.364 21.25 21.25 19.111 21.25 16.084V7.916C21.25 4.889 19.364 2.75 16.334 2.75Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                         <path fill-rule="evenodd" clip-rule="evenodd" d="M10.6889 11.9999C10.6889 13.0229 9.85986 13.8519 8.83686 13.8519C7.81386 13.8519 6.98486 13.0229 6.98486 11.9999C6.98486 10.9769 7.81386 10.1479 8.83686 10.1479H8.83986C9.86086 10.1489 10.6889 10.9779 10.6889 11.9999Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                         <path d="M10.6919 12H17.0099V13.852" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                         <path d="M14.1816 13.852V12" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                     </svg>
             </a>
             <a title="Xóa Sinh viên" class='btn btn-danger text-white btn-pd ml-0 mr-0' style='cursor:pointer'
                 onclick=Delete('/admin/student/delete/${data}')>
                <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                             <path d="M19.3248 9.46826C19.3248 9.46826 18.7818 16.2033 18.4668 19.0403C18.3168 20.3953 17.4798 21.1893 16.1088 21.2143C13.4998 21.2613 10.8878 21.2643 8.27979 21.2093C6.96079 21.1823 6.13779 20.3783 5.99079 19.0473C5.67379 16.1853 5.13379 9.46826 5.13379 9.46826" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                             <path d="M20.708 6.23975H3.75" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                             <path d="M17.4406 6.23973C16.6556 6.23973 15.9796 5.68473 15.8256 4.91573L15.5826 3.69973C15.4326 3.13873 14.9246 2.75073 14.3456 2.75073H10.1126C9.53358 2.75073 9.02558 3.13873 8.87558 3.69973L8.63258 4.91573C8.47858 5.68473 7.80258 6.23973 7.01758 6.23973" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                         </svg>
             </a>
            
             `;
                }, "width": "20%",
                "className": "text-center"
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
        info: true,
        serverSide: true, // Bật tính năng phân trang trên server
        processing: true  // Hiển thị thông báo đang tải dữ liệu
    });
}
$(document).ready(function () {
    //$('#loadDataStudent').on('click', '.btn-status', function () {
    //    var id = $(this).data('id');
    //    var status = $(this).data('status');
    //    var url = '/admin/student/update-status/' + id + '/' + (status == 1 ? 0 : 1);
    //    updateStatus(url);
    //});
    var previousCheckedState;
    $('#loadDataStudent').on('change', '.form-check-input', function () {
        //luu trạng thái trước khi thay đổi
        previousCheckedState = $(this).prop('checked');
        var id = $(this).attr('id').split('_')[1];
        var status = $(this).prop('checked') ? 1 : 0;
        var url = '/admin/student/update-status/' + id + '/' + status;
        updateStatus(url);
    });


});
function Delete(url) {
    swal({
        title: 'Bạn có chắc chắn muốn xóa?',
        type: 'warning',
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy',
        confirmButtonClass: 'btn btn-success mx-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                        dataTable.ajax.reload();
                    }
                }
            });
        }
    });
}
function updateStatus(url) {
    swal({
        title: 'Bạn có chắc chắn muốn thay đổi trạng thái?',
        type: 'warning',
        showCancelButton: true,
        closeOnConfirm: false,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy',
        confirmButtonClass: 'btn btn-success mx-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then((willUpdate) => {
        if (willUpdate) {
            // Nếu người dùng xác nhận "Đồng ý" thì thực hiện thay đổi trạng thái
            var status = previousCheckedState ? 1 : 0;
            $.ajax({
                type: "POST",
                url: url,
                data: { status: status },
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        } else {
            // Nếu người dùng hủy bỏ, khôi phục trạng thái của input
            $('.form-check-input').prop('checked', previousCheckedState);
            dataTable.ajax.reload();
        }
    });
}