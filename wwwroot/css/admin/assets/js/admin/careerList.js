var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {  
    dataTable = $('#loadData').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "ajax": {
            "url": "/admin/chuc-danh/danh-sach",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "codeCareer", "width": "10%" },
            { "data": "name", "width": "20%" },
            { "data": "coefficient", "width": "10%" },
            {
                "data": "status",
                "render": function (data) {
                    return data === 1 ? '<span class="text-black" title="' + data + '">Hoạt động</span>' :
                    '<span class="text-black" title="' + data + '">Không hoạt động</span>'
                }, "width": "10%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <a href="/admin/chuc-danh/edit/${data}" class='btn btn-primary btn-pd text-white' style='cursor:pointer;'>
                            Chỉnh sửa
                        </a>
                        &nbsp;                       
                        <a class='btn btn-danger text-white btn-pd' style='cursor:pointer'
                            onclick=Delete('/admin/chuc-danh/delete/'+${data})>
                            Xóa
                        </a>
                        `;
                }, "width": "20%"
            }
           
        ],
        "language": {
            "emptyTable": "Không có dữ liệu"
        },
        "width": "100%"
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
        confirmButtonText: 'Yes',
        cancelButtonText: 'No',
        confirmButtonClass: 'btn btn-success btn-full-width mar-bot-5',
        cancelButtonClass: 'btn btn-danger btn-full-width mar-bot-5',
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
                    }
                }
            });
        }
    });
}