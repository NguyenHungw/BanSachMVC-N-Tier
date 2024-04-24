var datatable;
$(document).ready(function () {
    loadDataable();
});

function loadDataable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/company/getall",
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "postalCode", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id", "render": function (data) {
                    return `
                       <div>
                       <a href="/Admin/company/Upsert?id=${data}"
                             class="btn btn-primary" > Edit</a>

                            <button class="btn btn-primary" onclick="Delete('/admin/company/delete?id=${data}')"" > Delete</a>
                        </div>
                    `
            }
            , "width": "15%" }


        ]
        
    });
}

function Delete(url) {
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
                        //
                    }
                }
            })
            
        }
    })
}