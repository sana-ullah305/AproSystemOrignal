

function ItemLoadInventory() {

    LoadInventoryTable();
}


function LoadInventoryTable() {
    oTable = $("#tblInventoryManagment").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/InventoryManagement/LoadInventoryTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "aoColumns": [
            //{ "data": 'id', "className": 'text-compressed bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'itemCode', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'itemName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'itemquantity', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], },
          

        ],
        "fnDrawCallback": function (oSettings) {
            $(".tblmenue[data-toggle='popover']").popover({
                html: true,
                content: function () {
                    var content = $(this).attr("data-popover-content");
                    return $(content).children(".popover-body").html();

                },
                placement: 'left',
                title: function () {
                    var title = $(this).attr("data-popover-content");
                    return $(title).children(".popover-heading").html();
                }
            });


        },

        "aoColumnDefs": [],
        order: [[0, "desc"]],

        "fnServerData": function (sSource, aoData, fnCallback) {
            //aoData.push({ "name": "Start_Date", "value": moment($('#dtp_item_MainStartDate').val()).format("MM-DD-YYYY") });
            //aoData.push({ "name": "End_Date", "value": moment($('#dtp_item_MainEndDate').val()).format("MM-DD-YYYY") });
            //aoData.push({ "name": "SearchType", "value": $('#ddl_item_SearchBy option:selected').val() });
            //aoData.push({ "name": "SearchValue", "value": $('#txt_item_Search').val() });
            ShowAjaxLoader();

            ShowAjaxLoader();
            $.post(sSource, aoData, function (json) {
                HideAjaxLoader();
                fnCallback(json);
            }, "json").error(function () {
                HideAjaxLoader();
            });

        },
        "initComplete": function (settings, json) {

            //$('.displayname').val("");
            HideAjaxLoader();
        }

    });
}

