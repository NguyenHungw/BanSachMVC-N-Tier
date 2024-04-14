var datatable;
$(document).ready(function () {
    loadDataable();
});

function loadDataable() {
    datatable = $('#tblData').DataTable({
        "ajax": {
            "url": "/admin/product/getall"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" }
            {
                "data": "id", "render": function (data) {
                    return `
            }
            , "width": "15%" }


        ]
    });
}