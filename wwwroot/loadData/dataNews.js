var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#loadDataNews').DataTable({
        //dom: 'Bfrtip',
        //buttons: [
        //    'copy',
        //    'csv', 'excel', 'pdf', 'print'
        //],
        "ajax": {
            "url": "/admin/news/list",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": null, "render": function (data, type, row, meta) { return meta.row + 1; }, "width": "3%", "className": "text-center" },
            {
                "data": "image",
                "render": function (data, type, row) {
                    if (data != null) {
                        return '<img src="' + data + '" class="post-img" alt="post image" width="auto" height="100%" style="border: 1px solid #ccc;" />';
                    } else {
                        return '<img src="~/img/thongbao.jpg" class="post-img" alt="post image" width="auto" height="100%" style="border: 1px solid #ccc;" />';
                    }
                },
                "width": "5%"
            },
            {
                "data": "title",
                "width": "30%",
                "className": "title-column"
            },
            {
                "data": "status",
                "render": function (data) {
                    return data == 1 ? '<span class="new-public" title="' + data + '">Đã xuất bản</span>' :
                        '<span class="new-draf" title="' + data + '">Lưu nháp</span>'
                }, "width": "10%",
                "className": "text-center"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a title="Chỉnh sửa thông tin" href="/admin/new/edit/${data}" class='btn btn-primary btn-pd text-white ml-0 mr-0' style='cursor:pointer;'>
                            <svg width="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M11.4925 2.78906H7.75349C4.67849 2.78906 2.75049 4.96606 2.75049 8.04806V16.3621C2.75049 19.4441 4.66949 21.6211 7.75349 21.6211H16.5775C19.6625 21.6211 21.5815 19.4441 21.5815 16.3621V12.3341" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path fill-rule="evenodd" clip-rule="evenodd" d="M8.82812 10.921L16.3011 3.44799C17.2321 2.51799 18.7411 2.51799 19.6721 3.44799L20.8891 4.66499C21.8201 5.59599 21.8201 7.10599 20.8891 8.03599L13.3801 15.545C12.9731 15.952 12.4211 16.181 11.8451 16.181H8.09912L8.19312 12.401C8.20712 11.845 8.43412 11.315 8.82812 10.921Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M15.1655 4.60254L19.7315 9.16854" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                                    </svg>
                        </a>
                        <a title="Xóa người dùng" class='btn btn-danger text-white btn-pd ml-0 mr-0' style='cursor:pointer'
                            onclick=Delete('/admin/new/delete/${data}')>
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