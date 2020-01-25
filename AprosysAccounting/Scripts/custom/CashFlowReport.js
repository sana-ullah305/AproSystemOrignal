function Trigger() {
    $("#btn_cashflow_Search").on("click", function () {
        GetCashFlowTable();
    });
}


function GetCashFlowTable() {
    LoadCashFlowTable();
}

function LoadCashFlowTable() {
    oTable = $("#tblCashflowInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Report/LoadCashFlowTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'invoiceNo', "className": 'text-compressed invoiceNo bold ', "bSortable": true, "orderSequence": ["desc", "asc"], },
            {
                "data": 'activityTimeStamp', "className": 'text-compressed bold', "bSortable": true, "orderSequence": ["desc", "asc"],
                "mRender": function (data, type, obj) {
                    if (obj["activityTimeStamp"] == null) { return '<td class=" text-compressed bold"></td>' }
                    else { return '<td class=" text-compressed bold">' + moment(obj["activityTimeStamp"]).format('YYYY-MM-DD') + '</td>' }
                }
            },

           { "data": 'transType', "className": 'text-compressed  bold', "bSortable": true, "orderSequence": ["desc", "asc"], },
           { "data": 'debit', "className": 'text-compressed  bold', "bSortable": true, "orderSequence": ["desc", "asc"], },
           { "data": 'credit', "className": 'text-compressed  bold', "bSortable": true, "orderSequence": ["desc", "asc"], },



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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_cashflow_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_cashflow_MainEndDate').val()).format("YYYY-MM-DD") });
            // aoData.push({ "name": "SearchType", "value": $('#ddl_customer_SearchBy option:selected').val() });


            ShowAjaxLoader();
            $.post(sSource, aoData, function (json) {
                HideAjaxLoader();
                fnCallback(json);
            }, "json").error(function () {
                HideAjaxLoader();
            });

        },
        "initComplete": function (settings, json) {
            HideAjaxLoader();
        }

    });
}
