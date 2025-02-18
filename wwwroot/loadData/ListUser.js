var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#loadDataUser').DataTable({
        /*        dom: 'Bfrtip',*/
        //buttons: [
        //    'copy',
        //    'csv', 'excel', 'pdf', 'print'
        //],
        "ajax": {
            "url": "/admin/users/list",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": null, "render": function (data, type, row, meta) { return meta.row + 1; }, "width": "5%", "className": "text-center" },
            { "data": "userName", "width": "10%" },
            { "data": "name", "width": "10%" },
            { "data": "email", "width": "10%" },
            //{
            //    "data": "status",
            //    "render": function (data) {
            //        return data == 1 ? '<span class="text-black" title="' + data + '">Hoạt động</span>' :
            //        '<span class="text-black" title="' + data + '">Không hoạt động</span>'
            //    }, "width": "10%"
            //},
            {
                "data": "id",
                "render": function (data) {
                    return `
                         <a title="Thêm vai trò cho người dùng" href="/admin/manager-user-roles/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                            <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M17.8877 10.8967C19.2827 10.7007 20.3567 9.50473 20.3597 8.05573C20.3597 6.62773 19.3187 5.44373 17.9537 5.21973" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                <path d="M19.7285 14.2505C21.0795 14.4525 22.0225 14.9255 22.0225 15.9005C22.0225 16.5715 21.5785 17.0075 20.8605 17.2815" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                <path fill-rule="evenodd" clip-rule="evenodd" d="M11.8867 14.6638C8.67273 14.6638 5.92773 15.1508 5.92773 17.0958C5.92773 19.0398 8.65573 19.5408 11.8867 19.5408C15.1007 19.5408 17.8447 19.0588 17.8447 17.1128C17.8447 15.1668 15.1177 14.6638 11.8867 14.6638Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                <path fill-rule="evenodd" clip-rule="evenodd" d="M11.8869 11.888C13.9959 11.888 15.7059 10.179 15.7059 8.069C15.7059 5.96 13.9959 4.25 11.8869 4.25C9.7779 4.25 8.0679 5.96 8.0679 8.069C8.0599 10.171 9.7569 11.881 11.8589 11.888H11.8869Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                <path d="M5.88509 10.8967C4.48909 10.7007 3.41609 9.50473 3.41309 8.05573C3.41309 6.62773 4.45409 5.44373 5.81909 5.21973" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                <path d="M4.044 14.2505C2.693 14.4525 1.75 14.9255 1.75 15.9005C1.75 16.5715 2.194 17.0075 2.912 17.2815" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                            </svg>
                        </a>
                         <a title="Xem thông tin" href="/admin/user/detail/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                            <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M22.4541 11.3918C22.7819 11.7385 22.7819 12.2615 22.4541 12.6082C21.0124 14.1335 16.8768 18 12 18C7.12317 18 2.98759 14.1335 1.54586 12.6082C1.21811 12.2615 1.21811 11.7385 1.54586 11.3918C2.98759 9.86647 7.12317 6 12 6C16.8768 6 21.0124 9.86647 22.4541 11.3918Z" stroke="currentColor"></path>
                                <circle cx="12" cy="12" r="3.5" stroke="currentColor"></circle>
                                <circle cx="13.5" cy="10.5" r="1.5" fill="currentColor"></circle>
                            </svg>
                        </a>
                        <a title="Đặt lại mật khẩu" href="/admin/user/reset-password/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                             <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M16.334 2.75H7.665C4.644 2.75 2.75 4.889 2.75 7.916V16.084C2.75 19.111 4.635 21.25 7.665 21.25H16.333C19.364 21.25 21.25 19.111 21.25 16.084V7.916C21.25 4.889 19.364 2.75 16.334 2.75Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                    <path fill-rule="evenodd" clip-rule="evenodd" d="M10.6889 11.9999C10.6889 13.0229 9.85986 13.8519 8.83686 13.8519C7.81386 13.8519 6.98486 13.0229 6.98486 11.9999C6.98486 10.9769 7.81386 10.1479 8.83686 10.1479H8.83986C9.86086 10.1489 10.6889 10.9779 10.6889 11.9999Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                    <path d="M10.6919 12H17.0099V13.852" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                    <path d="M14.1816 13.852V12" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                </svg>
                        </a>
                         <a title="Chỉnh sửa thông tin" href="/admin/user/edit/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                            <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M11.4925 2.78906H7.75349C4.67849 2.78906 2.75049 4.96606 2.75049 8.04806V16.3621C2.75049 19.4441 4.66949 21.6211 7.75349 21.6211H16.5775C19.6625 21.6211 21.5815 19.4441 21.5815 16.3621V12.3341" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M8.82812 10.921L16.3011 3.44799C17.2321 2.51799 18.7411 2.51799 19.6721 3.44799L20.8891 4.66499C21.8201 5.59599 21.8201 7.10599 20.8891 8.03599L13.3801 15.545C12.9731 15.952 12.4211 16.181 11.8451 16.181H8.09912L8.19312 12.401C8.20712 11.845 8.43412 11.315 8.82812 10.921Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M15.1655 4.60254L19.7315 9.16854" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                    </svg>
                        </a>
                        <a title="Xóa người dùng" class='btn btn-danger text-white btn-pd ml-0 mr-0' style='cursor:pointer'
                            onclick=Delete('/admin/user/delete/${data}')>
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
        info: true
    });
}

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