
function SubscriptionPaymentTrigger() {

    LoadSubscriptionPaymentTable();

    GetCustomers(2);
    $("#btn_spvoucher_AddNewReceipt").on("click", function () {
        ClearSubscriptionPayment();
        GetSubscriptionPaymentInvoiceNumber();

    });

    $("#btn_spvoucher_Save").on("click", function () {
        SaveSubscriptionpaymentVoucher();
    });


    $(document).on('click', '.close', function () {
        ClearSubscriptionPayment();
    });


    $(document).on('change', '#txt_rvoucher_Received', function () {

        var res = (($("#txt_spvoucher_Balance").val() * 1) - ($("#txt_spvoucher_Received").val() * 1)) * 1;
        $("#txt_spvoucher_GrossAmount").val(res);


    });



    $(document).on('click', '#btn_spvoucher_Delete', function () {

        var id = $(this).attr('rel'); DeleteSubscriptionPaymentVoucher(id);

    });
    $(document).on('click', '#btn_spvoucher_Edit', function () {

        var id = $(this).attr('rel'); GetSubscriptionPaymentByInvoiceId(id);
    });
    $(document).on('change', '.ddl_rvoucher_Customer', function () {
        var id = $("#ddl_spvoucher_Customer").val() * 1;
        if (id > 0) {
            GetCustomerBalance(id);
        }

    });

    //$(document).on('click', '#btn_rvoucher_Search', function () {
    //    LoadReceiptVoucherTable();
    //});


}


function ClearSubscriptionPayment() {
    $('#txt_spvoucher_InvoiceNo').val("");
    $('#dtp_spvoucher_ActivityDate').val("");
    $("#ddl_spvoucher_Customer").val(0);
    $('#txt_spvoucher_Received').val("");
    $('#txt_spvoucher_Balance').val("");
    $('#txt_spvoucher_GrossAmount').val("");
    $('#txt_spvoucher_Comments').val("");
    $('#txt_spvoucher_DueDate').val(28);

}

function GetCustomers(typeID) {
    var myurl = "/Customer/GetCustomerList";
    var mydata = new Object();
    mydata.typeID = typeID;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_spvoucher_Customer').empty();
            $('#ddl_spvoucher_Customer').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_spvoucher_Customer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            $('#ddl_spvoucher_Customer').val(0);
        }
    });
}

function GetCustomerBalance(custId) {
    var myurl = "/Customer/GetCustomerBalance";
    var mydata = new Object();
    mydata.custId = custId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null && result != "") {
            $("#txt_spvoucher_Balance").val(result);
            if ($('#txt_spvoucher_Received').val() != null && $('#txt_spvoucher_Received').val() != "") {
                var a = ($("#txt_spvoucher_Balance").val(result)) - ($('#txt_spvoucher_Received').val());
                $('#txt_spvoucher_GrossAmount').val(($("#txt_spvoucher_Balance").val(result)) - ($('#txt_spvoucher_Received').val()));
            }
        }

    });
}


function GetSubscriptionPaymentInvoiceNumber() {
    //var myurl = "/ReceiptVoucher/GetNextVoucher";
    //var mydata = new Object();

    //XHRPOSTRequest(myurl, mydata, function (result) {
    //    if (result.length !== 0) {

    $('#AddNewSubscriptionPaymentModal').modal('show');
    $("#txt_spvoucher_InvoiceNo").val('');
    $("#dtp_spvoucher_ActivityDate").val(moment().format('YYYY-MM-DD'));

    //    }
    //});
}


function SaveSubscriptionpaymentVoucher() {
    //if ($('#txt_spvoucher_InvoiceNo').val() == null || $("#txt_spvoucher_InvoiceNo").val().trim().length == 0) { toastr.warning("Please Refresh"); return; }
    if ($('#dtp_spvoucher_ActivityDate').val() == null || $("#dtp_spvoucher_ActivityDate").val().trim().length == 0) { toastr.warning("Please Select Date"); return; }
    //if ((($("#ddl_rvoucher_Type").val() * 1) == 1 || $("#ddl_rvoucher_Type").val() * 1 == 2) && $('#ddl_rvoucher_Customer').val() == null) { toastr.warning("Please Select Customer"); return; }
    if ($('#txt_spvoucher_Received').val() == null || $("#txt_spvoucher_Received").val().trim().length == 0) { toastr.warning("Please Enter Receive Amount"); return; }
    //if ($('#ddl_spvoucher_Type').val() == null || $("#ddl_rvoucher_Type").val().trim().length == 0) { toastr.warning("Please Select Type"); return; }
    if ($('#txt_spvoucher_DueDate').val() * 1 == 0) { toastr.warning("Please Select Due Date"); return; }
    var obj = {
        invoiceNo: $('#txt_spvoucher_InvoiceNo').val(),
        rActivityDate: $("#dtp_spvoucher_ActivityDate").val(),
        rCustomerId: $("#ddl_spvoucher_Customer").val() * 1,//1=walkin Customer

        rRecived: $('#txt_spvoucher_Received').val() * 1,
        rbalance: $('#txt_spvoucher_Balance').val() * 1,
        rGrossAmount: $('#txt_spvoucher_GrossAmount').val() * 1,
        rComments: $('#txt_spvoucher_Comments').val(),
        TypeId: 2,//Subscription Customer
        DueDate: $('#txt_spvoucher_DueDate').val() * 1
    };
    var myurl = "/SubscriptionPayment/SaveReceiptVoucher";
    var mydata = new Object();
    mydata.rvoucher = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            ClearSubscriptionPayment();
            $('#AddNewSubscriptionPaymentModal').modal('hide');
            LoadSubscriptionPaymentTable();
        }
    });
}

function LoadSubscriptionPaymentTable() {
    oTable = $("#tblSubsciptionPayment").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/SubscriptionPayment/LoadReceiptVoucherTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'InvoiceNo', "className": 'text-compressed invoiceNo bold ', "bSortable": false, },
            { "data": 'CustomerName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'Totaldues', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'PaidAmount', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'ActivityDate', "className": 'text-compressed purchaseDate bold', "bSortable": false,
                "mRender": function (data, type, obj) {
                    if (obj["ActivityDate"] == null) {
                        return '<td class=" text-compressed bold"></td>'
                    }
                    else {
                        return '<td class=" text-compressed bold">' + moment(obj["ActivityDate"]).format('YYYY-MM-DD') + '</td>'
                    }
                }
            },
            {
                "data": 'InvoiceNo', "className": 'text-compressed InvoiceNo bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    //html2 += ' <input type="button"  value="EDIT" rel=' + obj.InvoiceNo + ' id="btn_spvoucher_Edit" class="form-control" />'
                    //                        + ' <input type="button"  value="Delete" rel="' + obj.InvoiceNo + '" id="btn_spvoucher_Delete" class="form-control" />';
                    html2 += '<input type="button"  value="Delete" rel="' + obj.InvoiceNo + '" id="btn_spvoucher_Delete" class="form-control" />';
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
                title: function () {
                    var title = $(this).attr("data-popover-content");
                    return $(title).children(".popover-heading").html();
                }
            });


        },

        "aoColumnDefs": [],
        order: [[0, "desc"]],

        "fnServerData": function (sSource, aoData, fnCallback) {
            //   aoData.push({ "name": "Start_Date", "value": moment($('#dtp_rvoucher_MainStartDate').val()).format("MM-DD-YYYY") });
            //  aoData.push({ "name": "End_Date", "value": moment($('#dtp_rvoucher_MainEndDate').val()).format("MM-DD-YYYY") });
            // aoData.push({ "name": "Searchcheckbox", "value": $('#chk_rvoucher_customer').prop('checked') });

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

function DeleteSubscriptionPaymentVoucher(_voucherId) {
    var myurl = "/SubscriptionPayment/DeleteReceiptVoucher";
    var mydata = new Object();
    mydata.invoiceNo = _voucherId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadSubscriptionPaymentTable();

        }

    });
}
