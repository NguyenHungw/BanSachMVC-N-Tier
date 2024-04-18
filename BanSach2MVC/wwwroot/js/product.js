var datatable;
$(document).ready(function () {
    loadDataable();
});

function loadDataable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall",
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id", "render": function (data) {
                    return `
                       <div>
                       <a href="/Admin/Product/Upsert?id=${data}"
                             class="btn btn-primary" > Edit</a>

                            <a class="btn btn-primary" > Delete</a>
                        </div>
                    `
            }
            , "width": "15%" }


        ]
        
    });
}

function Deletex(url) {
    console.log(url);
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                  //  debugger;
                    if (data.success) {
                        toastr.success(data.message)
                    } else {
                        toastr.error(data.message)
                    }
                }
            })
            
        }
    })
}