var ItemList = new Array();
var editMode = false;
var Vendorlist = new Array();
var Adminstrativelist = new Array();
function PaymentVoucherTrigger() {
    $("#dtp_pvoucher_MainStartDate").val(moment().subtract('days', 7).format('YYYY-MM-DD'));
    $("#dtp_pvoucher_MainEndDate").val(moment().format('YYYY-MM-DD'));
    LoadPaymentVoucherTable();
    GetExpenseTypeList();
    LoadBankNames();
    LoadBankList();

    $("#btn_pvoucher_AddNewPayment").on("click", function () {
        ClearPayment();
        GetPaymentVoucherInvoiceNumber();
        //GetExpenseTypeList();
    });


    $(document).on('change', '.ddl_pvoucher_ExpenseType', function () {
        $(".txt_pvoucher_GrossAmount").val("");
        var id = $(".ddl_pvoucher_ExpenseType").val() * 1;
        if (id > 0) {
            if (id == 10) {
                $("#lbl_pvoucher_Expense").text("Customer"); $("#lbl_pvoucher_balance").show();
                $("#txt_pvoucher_Balance").show();
                $("#txt_pvoucher_Balance").parent().show();
                $("#div_payment_AddExpensebutton").hide();

                $("#div_payment_AddExpensebutton").hide();
            }
            if (id == 12) {
                $("#lbl_pvoucher_Expense").text("Vendor"); $("#lbl_pvoucher_balance").show();
                $("#txt_pvoucher_Balance").show();
                $("#txt_pvoucher_Balance").parent().show();
                $("#div_payment_AddExpensebutton").hide();
                $('#show_hide_0').hide();
                $('#show_hide_1').show();
            }
            if (id == 19) {
                $("#lbl_pvoucher_Expense").text("Expense"); $("#lbl_pvoucher_balance").hide();
                $("#txt_pvoucher_Balance").hide();
                $("#txt_pvoucher_Balance").parent().hide();
                $("#div_payment_AddExpensebutton").show();
                $('#show_hide_0').show();
                $('#show_hide_1').hide();
            }
            GetExpense(id, 0);
        }
    });

    $(document).on('change', '.ddl_pvoucher_Expense', function () {
        $("#txt_pvoucher_GrossAmount").val("");

        var id = $(".ddl_pvoucher_Expense").val() * 1;
        if (id > 0 && ($(".ddl_pvoucher_ExpenseType").val() * 1) == 12) { $("#txt_pvoucher_Paid").val(""); GetVendorBalance(id); }

    });

    $("#btn_pvoucher_Save").on("click", function () {
        SavePaymentVoucher();
    });
    $("#btn_pvoucher_SaveExpense").on("click", function () {
        if ($("#txt_pvoucher_AdministrativeExpense").val().length == 0) { toastr.warning("Please Add Expense Name "); return; }
        if ($("#txt_pvoucher_AdministrativeExpense").val().length > 0) { SaveAdministrativeExpense($("#txt_pvoucher_AdministrativeExpense").val()); }
    });
    $("#btn_pvoucher_deleteExpense").on("click", function () {

        if ($("#ddl_pvoucher_Expense").val() * 1 > 0) { DeleteAdministrativeExpense($("#ddl_pvoucher_Expense").val() * 1); }
    });



    $(document).on('click', '#btn_pvoucher_AddExpenseType', function () {
        $('#AddNewAdministrativeModal').modal('show');
        $("#div_payment_AddNewExpense").text('Add New Expense');
    });
    //$("#btn_pvoucher_AddExpenseType").on("click", function () {
    //    $('#AddNewAdministrativeModal').modal('show');
    //    $("#div_payment_AddNewExpense").text('Add New Expense');
    //});


    $(document).on('click', '.close', function () {
        ClearPayment();
    });

    $(document).on('click', '#btn_pvoucher_Delete', function () {
        var x = ConfirmDelete();
        if (x != false) {


            var id = $(this).attr('rel');

            DeletePaymentVoucher(id);
        }
    });
    $(document).on('click', '#btn_pvoucher_Edit', function () {
        var id = $(this).attr('rel');

        GetPaymentVoucherByInvoiceId(id);


    });

    $(document).on('click', '#btn_pvoucher_Search', function () {
        if ($("#dtp_pvoucher_MainStartDate").val() != "" && $("#dtp_pvoucher_MainEndDate").val() != "") {
            LoadPaymentVoucherTable();
        }
        else { toastr.warning("Please Select Dates"); }

    });
    $(document).on('click', '#btn_pvoucher_Download', function () {
        if ($("#dtp_pvoucher_MainStartDate").val() != "" && $("#dtp_pvoucher_MainEndDate").val() != "") {
            var obj = {
                "dtStart": moment($('#dtp_pvoucher_MainStartDate').val()).format("YYYY-MM-DD"),
                "dtEnd": moment($('#dtp_pvoucher_MainEndDate').val()).format("YYYY-MM-DD")
            }
            DownloadFiles("/Report/DownloadPaymentVoucherList?", obj);
        }
        else { toastr.warning("Please Select Dates"); }

    });
    $(document).on('change', '#txt_pvoucher_Paid', function () {
        if ($("#ddl_pvoucher_ExpenseType").val() * 1 == 19) { $('.txt_pvoucher_GrossAmount').val(($('#txt_pvoucher_Paid').val() * 1)); }
        else {
            $('.txt_pvoucher_GrossAmount').val(($('#txt_pvoucher_Balance').val() * 1) - ($('#txt_pvoucher_Paid').val() * 1));
        }
    });
    $("#btn_pvoucher_closeExpense").on("click", function () {
        $("#txt_pvoucher_AdministrativeExpense").val("");
    });
}


function ClearPayment() {
    $("#txt_PaymentVoucher_DocNum").attr("readonly", true);
    $("#ddl_PaymentVoucher_Bank").attr("disabled", "disabled");
    $("#ddl_PaymentVoucher_Bank").val(null);
    $("#ddl_PaymentVoucher_Bank").trigger('chosen:updated');
    $("#ddl_PaymentVoucher_BankTransfer").val(null);
    $("#ddl_PaymentVoucher_BankTransfer").trigger('chosen:updated');

    $('#dtp_pvoucher_ActivityDate').val("");
    $('#ddl_pvoucher_ExpenseType').val("");
    $('#ddl_pvoucher_Expense').val("");
    $('#txt_pvoucher_Paid').val("");
    $('#txt_pvoucher_Balance').val("");
    $('.txt_pvoucher_GrossAmount').val("");
    $('#txt_PaymentVoucher_DocNum').val("");
    // $('.ddl_pvoucher_ExpenseType').empty();
    $("#ddl_PaymentVoucher_PaymentMode").val(1);
    $("#lbl_pvoucher_Expense").text("Expense");
    $("#txt_pvoucher_Comments").val("");
    $("#btn_pvoucher_Save").attr("disabled", false);
    $('#show_hide_0').show();
    $('#show_hide_1').hide();
    $('.bankTransfer_hideShow').hide();

    $('.hide_showBank').show();
    $('.hide_showBank  select').hide();


    editMode = false;
}
function ConfirmDelete() {
    var x = confirm("Are you sure you want to delete?");
    if (x)
        return true;
    else
        return false;
}

function SavePaymentVoucher() {
    //if ($('#txt_pvoucher_InvoiceNo').val() == null || $("#txt_pvoucher_InvoiceNo").val().trim().length == 0) { $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Refresh"); return; }
    if ($('#dtp_pvoucher_ActivityDate').val() == null || $("#dtp_pvoucher_ActivityDate").val().trim().length == 0) { $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Select Date"); return; }
    if ($('#ddl_pvoucher_ExpenseType').val() == null || $("#ddl_pvoucher_ExpenseType").val().trim().length == 0) { $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Select ExpenseType"); return; }

    if (($('#ddl_pvoucher_Expense').val() == null || $("#ddl_pvoucher_Expense").val().trim().length == 0) && $("#ddl_pvoucher_ExpenseType").val() == 19)
    { $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Select Expense"); return; }
    if (($('#ddl_pvoucher_Expense').val() == null || $("#ddl_pvoucher_Expense").val().trim().length == 0) && $("#ddl_pvoucher_ExpenseType").val() == 12)
    { $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Select Vendor"); return; }

    if ($('#ddl_PaymentVoucher_PaymentMode').val() == "2") {
        if ($('#ddl_PaymentVoucher_Bank').val() == null) {
            $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Select The  Bank Account ");
            return;
        }
        if ($('#txt_PaymentVoucher_DocNum').val().trim().length == 0) {
            $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Enter the Cheque No "); return;
        }
    }
    if ($('#ddl_PaymentVoucher_PaymentMode').val() == "3" && $('#ddl_PaymentVoucher_BankTransfer').val() == null) {
        $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Enter The Bank "); return;
    }
    if ($('#txt_pvoucher_Paid').val() == null || $("#txt_pvoucher_Paid").val().trim().length == 0) {
        $("#btn_pvoucher_Save").attr("disabled", false); toastr.warning("Please Enter Paid Amount"); return;
    }
    //if ($('#txt_rvoucher_Balance').val() == null || $("#txt_rvoucher_Balance").val().trim().length == 0) { toastr.warning("Please Enter S"); return; }
    // if ($('#txt_pvoucher_Comments').val() == null || $("#txt_pvoucher_Comments").val().trim().length == 0) { toastr.warning("Please Enter DueDate"); return; }

    ShowAjaxLoader();

    var obj = {
        invoiceNo: $('#txt_pvoucher_InvoiceNo').val(),
        activityDate: $("#dtp_pvoucher_ActivityDate").val(),
        expenseTypeCategory: $("#ddl_pvoucher_ExpenseType").val(),
        expensetype: $('#ddl_pvoucher_Expense').val(),
        paid: $('#txt_pvoucher_Paid').val() * 1,
        balance: $('#txt_pvoucher_Balance').val(),
        grossAmount: $('#txt_pvoucher_GrossAmount').val(),
        comments: $('#txt_pvoucher_Comments').val(),
        bankId: $('#ddl_PaymentVoucher_Bank').val(),
        checkNo: $('#txt_PaymentVoucher_DocNum').val(),
        bankTransferAccountId: $('#ddl_PaymentVoucher_BankTransfer').val(),
        paymentMode: $('#ddl_PaymentVoucher_PaymentMode').val() * 1
    };
    var myurl = editMode == true ? "/PaymentVoucher/EditPaymentVoucher" : "/PaymentVoucher/SavePaymentVoucher";
    //var myurl = "/PaymentVoucher/SavePaymentVoucher";
    var mydata = new Object();
    mydata.pvoucher = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            ClearPayment();
            $('#AddNewPaymentVoucherModal').modal('hide');
            LoadPaymentVoucherTable();
        }
        HideAjaxLoader();
        if ($("#dcf_CurrDate").length > 0) { GetDailyCashFlow($("#dcf_CurrDate").val()); }
    });
}

function LoadPaymentVoucherTable() {
    oTable = $("#tblPaymentInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/PaymentVoucher/LoadPaymentVoucherTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "aoColumns": [
            { "data": 'InvoiceNo', "className": 'text-compressed bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{
            //    "data": 'ActivityDate', "className": 'text-compressed bold', "bSortable": false, "orderSequence": ["desc", "asc"],
            //    "mRender": function (data, type, obj) {
            //        if (obj["ActivityDate"] == null) {
            //            return '<td class=" text-compressed bold"></td>'
            //        }
            //        else {
            //            return '<td class=" text-compressed bold">' + moment(obj["ActivityDate"]).format('YYYY-MM-DD') + '</td>'
            //        }
            //    }
            //},
             {
                 "data": 'ActivityDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                     if (obj["ActivityDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                     else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["ActivityDate"])) + '</td>' }
                 }
             },

            { "data": 'HeadType', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'Name', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'Amount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },

            {
                "data": 'InvoiceNo', "className": 'text-compressed InvoiceNo bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    //  html2 += ' <input type="button"  value="EDIT" rel=' + obj.InvoiceNo + ' id="btn_pvoucher_Edit" class="form-control" />'
                    //                        + ' <input type="button"  value="Delete" rel=' + obj.InvoiceNo + ' id="btn_pvoucher_Delete" class="form-control" />';
                    html2 += '<input type="button"  value="Delete" rel=' + obj.InvoiceNo + ' id="btn_pvoucher_Delete" class="form-control" />';
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
        //order: [[0, "desc"]],
        order: [],

        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_pvoucher_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_pvoucher_MainEndDate').val()).format("YYYY-MM-DD") });
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




function GetPaymentVoucherInvoiceNumber() {
    //var myurl = "/PaymentVoucher/GetNextVoucher";
    //var mydata = new Object();

    //XHRPOSTRequest(myurl, mydata, function (result) {
    //    if (result.length !== 0) {

    $('#AddNewPaymentVoucherModal').modal('show');
    $(".modal-title").text("Add New Payment Voucher");
    $("#txt_pvoucher_InvoiceNo").val('');
    //$("#dtp_pvoucher_ActivityDate").val(moment().format('YYYY-MM-DD'));

    $('#dtp_pvoucher_ActivityDate').val(FormatDateTimeToDisplay(new Date()));
    $(".ddl_pvoucher_ExpenseType").val(19); $("#lbl_pvoucher_balance").hide();
    $("#txt_pvoucher_Balance").hide();
    $("#txt_pvoucher_Balance").parent().hide();
    GetExpense(19, 0);
    //    }
    //});
}

function GetExpenseTypeList() {
    var myurl = "/PaymentVoucher/GetExpenseTypeList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        $('.ddl_pvoucher_ExpenseType').empty();
        if (result.length >= 0) {


            for (var i = 0; i < result.length; i++) {
                $('.ddl_pvoucher_ExpenseType').append('<option value="' + result[i].id + '">' + result[i].name + '</option>');
            }


            //var id = $(".ddl_pvoucher_ExpenseType").val() * 1;//loading Expense Type
            //if (id > 0) {
            //    GetExpense(id);

            //}

            itemlist = result;
        }

        //$("#ddl_PaymentVoucher_Bank").attr("disabled", "disabled");
    });
}

function GetExpense(coaID, ExpenseId) {
    var myurl = "/PaymentVoucher/GetExpense";
    var mydata = new Object();
    mydata.coaID = coaID;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $("#txt_pvoucher_Paid").val("");
            $("#txt_pvoucher_Balance").val("");
            $('.ddl_pvoucher_Expense').empty();
            $('.ddl_pvoucher_Expense').html('');
            $('.ddl_pvoucher_Expense').val("select");
            // $('.ddl_pvoucher_Expense').append('<option value="' +0+ '"disabled>Select</option>');
            //$('.ddl_pvoucher_Expense').append('<option value="' +0+ '"> </option>');
            for (var i = 0; i < result.length; i++) {
                $('.ddl_pvoucher_Expense').append('<option value="' + result[i].id + '">' + result[i].name + '</option>');
            }
            $('.ddl_pvoucher_Expense').val(0);
            if (ExpenseId > 0) { $('.ddl_pvoucher_Expense').val(ExpenseId); }
        }
    });
}


//function GetExpense(coaID) {
//    var myurl = "/PaymentVoucher/GetExpense";
//    var mydata = new Object();
//    mydata.coaID = coaID;
//    XHRPOSTRequest(myurl, mydata, function (result) {
//        if (result.length !== 0) {
//            if (coaID == 12) {
//                Vendorlist = result;
//            }
//            if (coaID == 19) {
//                Adminstrativelist = result;
//            }
//        }
//    });
//}



function GetCustomerBalance(custId) {
    var myurl = "/Customer/GetCustomerBalance";
    var mydata = new Object();
    mydata.custId = custId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null && result != "") {
            $("#txt_pvoucher_Balance").val(result);
        }
    });
}
function GetVendorBalance(vendID) {
    var myurl = "/Vendor/GetVendorBalance";
    var mydata = new Object();
    mydata.vendID = vendID;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null && result != "") {
            $("#txt_pvoucher_Balance").val(result);
        }
    });
}


function DeletePaymentVoucher(_voucherId) {
    var myurl = "/PaymentVoucher/DeletePaymentVoucher";
    var mydata = new Object();
    mydata.invoiceNo = _voucherId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Deleted", "success");
            LoadPaymentVoucherTable();
        }
    });
}

function SaveAdministrativeExpense(name) {
    var myurl = "/Expense/SaveAdministrativeExpense";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.name = name;
    XHRPOSTRequest(myurl, mydata, function (result) {
        //if (result != null && result > 0) {
        if (result == "success") {
            showNotification("Saved", "success");
            $("#txt_pvoucher_AdministrativeExpense").val("");
            $('#AddNewAdministrativeModal').modal('hide');
            GetExpense(19, result);
        }
        if (result == "Expense Already Exists") {

            toastr.warning(result);

        }
        //if (result == 0) {
        //    toastr.warning("Expense Already Exists");

        //}
    });
}


function GetPaymentVoucherByInvoiceId(_voucherInvoiceId) {
    var myurl = "/PaymentVoucher/GetPaymentVoucherByInvoiceId";
    var mydata = new Object();
    mydata._voucherInvoiceId = _voucherInvoiceId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $('#AddNewPaymentVoucherModal').modal('show'); $(".modal-title").text("Edit Payment Voucher");
            $('#txt_pvoucher_InvoiceNo').val(result.invoiceNo);
            // $('#dtp_pvoucher_ActivityDate').val(moment(result.activityDate).format('YYYY-MM-DD'));

            if (result.activityDate != null) { $('#dtp_pvoucher_ActivityDate').val(FormatDateTimeToDisplay(GetJSDate(result.activityDate))); }
            $('#ddl_pvoucher_ExpenseType').val(result.expenseTypeCategory);

            $('#txt_pvoucher_Paid').val(result.paid);
            $('#txt_pvoucher_Comments').val(result.comments);
            if (result.expenseTypeCategory == 19) {
                GetExpense(19, 0); $("#lbl_pvoucher_Expense").text("Expense"); $("#lbl_pvoucher_balance").hide();
                $("#txt_pvoucher_Balance").hide();
                $("#txt_pvoucher_Balance").parent().hide();
            }
            if (result.expenseTypeCategory == 12) {
                GetExpense(12, 0); $('#ddl_pvoucher_Expense').val(result.expensetype);


                $("#lbl_pvoucher_Expense").text("Vendor"); $("#lbl_pvoucher_balance").show();
                $("#txt_pvoucher_Balance").show();
                $("#txt_pvoucher_Balance").parent().show();
            }
            if (result.expenseTypeCategory == 10) { GetExpense(10, 0); }
            editMode = true;
        }

    });
}


function LoadBankNames() {
    $.post("/Banks/GetBanksList", {}, function (data) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].bankName == "Cash") {
                continue;
            }
            var tmp = $("<option></option>");
            tmp.attr("value", data[i].bankID);
            tmp.text(data[i].bankName);
            $("#ddl_PaymentVoucher_Bank").append(tmp);
        }
        $("#ddl_PaymentVoucher_Bank").chosen({ width: '100%' });
    }, "json");
}

$("#ddl_PaymentVoucher_PaymentMode").change(function () {
    if ($(this).val() * 1 == 1) {

        $("#ddl_PaymentVoucher_Bank").val(null);
        $("#ddl_PaymentVoucher_Bank").attr("disabled", "disabled");
        $("#ddl_PaymentVoucher_Bank").trigger('chosen:updated');
        $("#txt_PaymentVoucher_DocNum").val("");
        $("#txt_PaymentVoucher_DocNum").attr("readonly", true);
        $('.bankTransfer_hideShow').hide();
        $('.hide_showBank').show();
        $('.hide_showBank  select').hide();
    }
    if ($(this).val() * 1 == 2) {
        $("#ddl_PaymentVoucher_Bank").show();
        $("#ddl_PaymentVoucher_Bank").removeAttr("disabled");
        $("#ddl_PaymentVoucher_Bank").trigger('chosen:updated');
        $("#txt_PaymentVoucher_DocNum").removeAttr("readonly");
        $('.bankTransfer_hideShow').hide();
        $('.hide_showBank').show();
        $('.hide_showBank  select').hide();
    }
    if ($(this).val() * 1 == 3) {
        //alert('hay this is alert for banck transfer.');
        $("#ddl_PaymentVoucher_Bank").val(null);
        $("#ddl_PaymentVoucher_Bank").attr("disabled", "disabled");
        $("#ddl_PaymentVoucher_Bank").trigger('chosen:updated');
        $("#txt_PaymentVoucher_DocNum").val("");
        $("#txt_PaymentVoucher_DocNum").attr("readonly", true);
        $('.hide_showBank').hide();
        $('.bankTransfer_hideShow').show();
        $("#ddl_PaymentVoucher_BankTransfer").val(null);
        $("#ddl_PaymentVoucher_BankTransfer").trigger('chosen:updated');
    }

});


function LoadBankList() {
    XHRPOSTRequest("/Banks/GetBanksList", {}, function (data) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].bankName == "Cash") {
                continue;
            }
            var tmp = $("<option></option>");
            tmp.attr("value", data[i].bankID);
            tmp.text(data[i].bankName);
            $("#ddl_PaymentVoucher_BankTransfer").append(tmp);
        }
        $("#ddl_PaymentVoucher_BankTransfer").chosen({ width: '100%' });
    });

}