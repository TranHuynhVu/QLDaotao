var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {  
    dataTable = $('#loadDataContract').DataTable({
        "ajax": {
            "url": "/admin/hop-dong/danh-sach",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "code" },
            { "data": "numcontract"},
            { "data": "userid"},
            { "data": "todate"},
            { "data": "fromdate"},
            { "data": "numqd"},
            { "data": "fileqd"},
            { "data": "status"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/admin/hop-dong/edit/${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;                       
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/admin/hop-dong/delete/'+${data})>
                            Delete
                        </a>
                        </div>`;
                }, "width": "30%"
            }
           
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: 'Are you sure you want to ask the User to update their details?',
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