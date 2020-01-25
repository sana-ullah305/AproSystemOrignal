function PaymentHistoryTrigger() {
    $("#dtp_sales_history_MainStartDate").val(moment().subtract('days', 90).format('YYYY-MM-DD'));
    $("#dtp_sales_history_MainEndDate").val(moment().format('YYYY-MM-DD'));
    LoadPaymentHistoryTableData();
}

//function LoadPaymentHistoryTableData() {
function LoadPaymentHistoryTableData() {
    oTable = $("#tblPaymentHistory").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/PaymentHistory/GetPaymentHistoryList",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            {
                "data": 'invoiceNo', "className": 'text-compressed invoiceNo bold ', "bSortable": false, "orderSequence": ["desc", "asc"],
                "mRender": function (data, type, obj) {
                    if (obj["invoiceNo"] == false) {
                        return '<td class=" text-compressed bold">' + obj.invoiceNo + '</td>'
                    }
                    else {
                        return '<td class="text-compressed bold">' + obj.invoiceNo + '</td>'
                    }
                }
            },
            { "data": 'customerName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{
            //    "data": 'transactionDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
            //        if (obj["transactionDate"] == null) { return '<td class=" text-compressed bold"></td>' }
            //        else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["transactionDate"])) + '</td>' }
            //    }
            //},
             {
                 "data": 'activityTimestamp', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                     if (obj["activityTimestamp"] == null) { return '<td class=" text-compressed bold"></td>' }
                     else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["activityTimestamp"])) + '</td>' }
                 }
             },
            {
                "data": 'amount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            { "data": 'recievedBy', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'tranId', "className": 'text-compressed tranId bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";

                    html2 += ' <input type="button"  value="Delete" rel=' + obj.tranId + ' id="btn_payment_history_Delete" class="btn btn-danger" />';
                    return html2;
                }
            },

        ],
        "fnDrawCallback": function (oSettings) {
            $(".tblmenue[data-toggle='popover']").popover({
                html: true,
                content: function () {
                    var content = $(this).attr("data-popover-content");
                    return $(content).children(".popover-body").html();

                },
                placement: 'left',
                //  trigger : 'click',
                //delay: { show: 500, hide: 1000 },
                title: function () {
                    var title = $(this).attr("data-popover-content");
                    return $(title).children(".popover-heading").html();
                }
            });


        },

        "aoColumnDefs": [],
        //order: [[0, "desc"]],
        order: [],

        "fnServerData": function (sSource, aoData, fnCallback) {
            //aoData.push({ "name": "SearchType", "value": $('#ddl_SearchType option:selected').val() });
            //aoData.push({ "name": "SearchValue", "value": String($('.txt_SearchValue').val()) });
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_sales_history_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_sales_history_MainEndDate').val()).format("YYYY-MM-DD") });
            //  aoData.push({ "name": "VendorId", "value": $('#ddl_purchase_MainVendor').val() });

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

$('#btn_payment_history_Search').click(function () {
    LoadPaymentHistoryTableData();
});


$(document).on('click', '#btn_payment_history_Delete', function () {
    //alert($(this).attr('rel'));
    var tranId = $(this).attr('rel');
    if (confirm("Are you sure to Delete")) {
        DeletePaymentHistory(tranId)
    }
});
function DeletePaymentHistory(tranId) {
    var myurl = "/PaymentHistory/DeletePaymentHistory";
    var mydata = new Object();
    mydata.tranId = tranId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "Success") {
            showNotification("Deleted", "success");
            LoadPaymentHistoryTableData();
        }
    });
}