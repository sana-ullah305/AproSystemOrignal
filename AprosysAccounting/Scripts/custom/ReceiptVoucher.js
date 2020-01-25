var editMode = false;
var customerlist = new Array();
var Subcustomerlist = new Array();
function ReceiptVoucherTrigger() {
    //$("#dtp_rvoucher_MainStartDate").val(moment().subtract('days', 1).format('YYYY-MM-DD'));
    $("#dtp_rvoucher_MainStartDate").val(moment().format('YYYY-MM-DD'));
    $("#dtp_rvoucher_MainEndDate").val(moment().format('YYYY-MM-DD'));
    LoadReceiptVoucherTable();
    $('#ddl_rvoucher_Type').val(0);//default no selection
    GetCustomers(2); //GetCustomers(1);


    $("#btn_rvoucher_AddNewReceipt").on("click", function () {
        ClearReceipt();
        GetReceiptInvoiceNumber();

    });

    $("#btn_rvoucher_Save").on("click", function () {
        $("#btn_rvoucher_Save").attr("disabled", true);
        SaveReceiptVoucher();
    });


    $(document).on('click', '.close', function () {
        ClearReceipt();
    });


    $(document).on('change', '#txt_rvoucher_Received', function () {

        var res = (($("#txt_rvoucher_Balance").val() * 1) - ($("#txt_rvoucher_Received").val() * 1)) * 1;
        $("#txt_rvoucher_GrossAmount").val(res);


    });


    $(document).on('click', '#chk_rvoucher_customer', function () {
        LoadReceiptVoucherTable();

        if ($('#chk_rvoucher_customer').prop('checked') == true) {
            SetSubCustomers();
        }
        SetCustomers();

    });
    $(document).on('click', '#btn_rvoucher_Delete', function () {

        var id = $(this).attr('rel');
        if (confirm("Are you sure to Delete invoiceId= " + id)) {
            DeleteReceiptVoucher(id);
        }
    });
    $(document).on('click', '#btn_rvoucher_Edit', function () {

        var id = $(this).attr('rel'); GetReceiptVoucherByInvoiceId(id);
    });
    $(document).on('click', '#btn_rvoucher_Print', function () {

        var id = $(this).attr('rel'); PrintRecieptVoucher(id);
    });

    $(document).on('click', '.btn_rvoucher_Download', function () {

        var id = $(this).attr('rel'); DownloadReceiptVoucher(id);
    });
    $(document).on('change', '.ddl_rvoucher_Customer', function () {
        var id = $("#ddl_rvoucher_Customer").val() * 1;
        if (id > 0) {
            if (($(".ddl_rvoucher_Type").val() * 1) == 2) {
                var res = $.grep(Subcustomerlist, function (re) { return re.Id == id });
                $("#txt_rvoucher_DueDate").val(res[0].DueDate);
                $("#lbl_rvoucher_MonthlyFee").val(res[0].SubscriptionAmount);
            }
            $("#txt_rvoucher_Received,#txt_rvoucher_GrossAmount").val(0);
            GetCustomerBalance(id);

        }
    });

    $(document).on('click', '#btn_rvoucher_Search', function () {
        if ($("#dtp_rvoucher_MainStartDate").val() != "" && $("#dtp_rvoucher_MainEndDate").val() != "") {
            LoadReceiptVoucherTable();
        }
        else { toastr.warning("Please Select Dates"); }
      
    });

    $(document).on('change', '.ddl_rvoucher_Type', function () {

        var id = $(".ddl_rvoucher_Type").val() * 1;
        if (id > 0) {
            if (id == 1) { $("#div_rvoucher_RBalance").show(); $("#div_voucher_balance").show(); $("#div_rvoucher_customer").show(); $("#lbl_rvoucher_Customer").text("Customer"); $("#div_duedate").hide(); SetCustomers(); }
            if (id == 2) { $("#div_rvoucher_RBalance").show(); $("#div_voucher_balance").show(); $("#div_rvoucher_customer").show(); $("#lbl_rvoucher_Customer").text("Subscription Customer"); $("#div_duedate").show(); SetSubCustomers(); }// $("#div_startdate").show(); }
            if (id == 3) { $("#div_rvoucher_RBalance").hide(); $("#div_voucher_balance").hide(); $("#div_rvoucher_customer").hide(); $("#div_duedate").hide(); return; }
            //GetCustomers(id);
        }
    });
}


function ClearReceipt() {
    $('#txt_rvoucher_InvoiceNo').val("");
    $('#dtp_rvoucher_ActivityDate').val("");
    $('#ddl_rvoucher_Type').val(0);
    $("#ddl_rvoucher_Customer").val(0);
    $('#txt_rvoucher_Received').val("");
    $('#txt_rvoucher_Balance').val("");
    $('#txt_rvoucher_GrossAmount').val("");
    $('#txt_rvoucher_Comments').val("");
    $('#txt_rvoucher_DueDate').val("");
    $("#btn_rvoucher_Save").attr("disabled", false);
    $("#lbl_rvoucher_MonthlyFee").val("");
    editMode = false;
    //$('#ddl_rvoucher_Type').empty();
    //$("#ddl_rvoucher_Customer").empty();
    //$('#dtp_rvoucher_StartDate').val("");
}


function SaveReceiptVoucher() {
    //if ($('#txt_rvoucher_InvoiceNo').val() == null || $("#txt_rvoucher_InvoiceNo").val().trim().length == 0) { $("#btn_rvoucher_Save").attr("disabled", false); toastr.warning("Please Refresh"); return; }
    if ($('#dtp_rvoucher_ActivityDate').val() == null || $("#dtp_rvoucher_ActivityDate").val().trim().length == 0) { $("#btn_rvoucher_Save").attr("disabled", false); toastr.warning("Please Select Date"); return; }
    if ((($("#ddl_rvoucher_Type").val() * 1) == 1 || $("#ddl_rvoucher_Type").val() * 1 == 2) && $('#ddl_rvoucher_Customer').val() == null) { $("#btn_rvoucher_Save").attr("disabled", false); toastr.warning("Please Select Customer"); return; }
    if ($('#txt_rvoucher_Received').val() == null || $("#txt_rvoucher_Received").val().trim().length == 0) { $("#btn_rvoucher_Save").attr("disabled", false); toastr.warning("Please Enter Receive Amount"); return; }
    if ($('#ddl_rvoucher_Type').val() == null || $("#ddl_rvoucher_Type").val().trim().length == 0) { $("#btn_rvoucher_Save").attr("disabled", false); toastr.warning("Please Select Type"); return; }
    if ($('#ddl_rvoucher_Type').val() * 1 == 2 && $('#txt_rvoucher_DueDate').val() * 1 == 0) { $("#btn_rvoucher_Save").attr("disabled", false); toastr.warning("Please Select Due Date"); return; }
    var obj = {
        invoiceNo: $('#txt_rvoucher_InvoiceNo').val(),
        rActivityDate: $("#dtp_rvoucher_ActivityDate").val(),
        rCustomerId: ($("#ddl_rvoucher_Type").val() * 1) == 3 ? 1 : $("#ddl_rvoucher_Customer").val() * 1,//1=walkin Customer

        rRecived: $('#txt_rvoucher_Received').val() * 1,
        rbalance: $('#txt_rvoucher_Balance').val() * 1,
        rGrossAmount: $('#txt_rvoucher_GrossAmount').val() * 1,
        rComments: $('#txt_rvoucher_Comments').val(),
        TypeId: $('#ddl_rvoucher_Type').val(),
        DueDate: $('#txt_rvoucher_DueDate').val() * 1
    };
    var myurl = (editMode == true) ? "/ReceiptVoucher/EditReceiptVoucher" : "/ReceiptVoucher/SaveReceiptVoucher";
    //  var myurl = "/ReceiptVoucher/SaveReceiptVoucher";
    var mydata = new Object();
    mydata.rvoucher = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {

        showNotification("Saved", "success");
        ClearReceipt();
        $('#AddNewVoucherModal').modal('hide');
        LoadReceiptVoucherTable();

        //Ask If User wants to Print

        swal({
            title: "Do you want to Print Reciept?",
            text: "",
            type: "info",
            showCancelButton: true,

            confirmButtonText: "YES",
            cancelButtonText: "No",
            closeOnConfirm: true,
        },
            function (isConfirm) {
                if (isConfirm) {
                    PrintRecieptVoucher(result);
                }
            }
        );

        //if (result == "User Already Exists") {
        //    toastr.warning(result);

        //}
    });
}

function LoadReceiptVoucherTable() {
    oTable = $("#tblReceiptInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/ReceiptVoucher/LoadReceiptVoucherTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'InvoiceNo', "className": 'text-compressed invoiceNo bold ', "bSortable": false, },
            //{
            //    "data": 'ActivityDate', "className": 'text-compressed purchaseDate bold', "bSortable": false,
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
            { "data": 'CustomerName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'Amount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'InvoiceNo', "className": 'text-compressed InvoiceNo bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.InvoiceNo + ' id="btn_rvoucher_Edit" class="form-control" />'
                        + ' <input type="button"  value="Delete" rel="' + obj.InvoiceNo + '" id="btn_rvoucher_Delete" class="form-control" />'
                        + '<input type="button"  value="Print" rel="' + obj.InvoiceNo + '" id="btn_rvoucher_Print" class="form-control" />'
                        + '<input type="button"  value="Download" rel="' + obj.InvoiceNo + '"  class="form-control btn_rvoucher_Download" />';
                    //html2 += '<input type="button"  value="Delete" rel="' + obj.InvoiceNo + '" id="btn_rvoucher_Delete" class="form-control" />'
                    //+ '<input type="button"  value="Print" rel="' + obj.InvoiceNo + '" id="btn_rvoucher_Print" class="form-control" />'
                    //+ '<input type="button"  value="Download" rel="' + obj.InvoiceNo + '"  class="form-control btn_rvoucher_Download" />';
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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_rvoucher_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_rvoucher_MainEndDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "Searchcheckbox", "value": $('#chk_rvoucher_customer').prop('checked') });
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




function DeleteReceiptVoucher(_voucherId) {
    var myurl = "/ReceiptVoucher/DeleteReceiptVoucher";
    var mydata = new Object();
    mydata.invoiceNo = _voucherId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadReceiptVoucherTable();

        }

    });
}


function GetCustomerBalance(custId) {
    var myurl = "/Customer/GetCustomerBalance";
    var mydata = new Object();
    mydata.custId = custId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $("#txt_rvoucher_Balance").val(result * 1);
            //if ($('#txt_rvoucher_Received').val() != null && $('#txt_rvoucher_Received').val() != "") {
            //    var a = ($("#txt_rvoucher_Balance").val(result)) - ($('#txt_rvoucher_Received').val());
            //    $('#txt_rvoucher_GrossAmount').val(($("#txt_rvoucher_Balance").val(result)) - ($('#txt_rvoucher_Received').val()));
            //}
        }

    });
}

function GetReceiptInvoiceNumber() {
    //var myurl = "/ReceiptVoucher/GetNextVoucher";
    //var mydata = new Object();
    //XHRPOSTRequest(myurl, mydata, function (result) {
    //    if (result.length !== 0) {
    $('#AddNewVoucherModal').modal('show'); $(".modal-title").text("Add New Monthly Receipt");
    $("#txt_rvoucher_InvoiceNo").val('');
  //  $("#dtp_rvoucher_ActivityDate").val(moment().format('YYYY-MM-DD'));
    $('#dtp_rvoucher_ActivityDate').val(FormatDateTimeToDisplay(new Date()));
    if (($('#chk_rvoucher_customer').prop('checked')) == true) {
        $("#ddl_rvoucher_Type").val(2); SetSubCustomers(); $("#lbl_rvoucher_Customer").text("Subscription Customer"); $("#div_duedate").show();
    }
    else {
        $("#ddl_rvoucher_Type").val(1); SetCustomers(); $("#div_voucher_balance").show(); $("#div_rvoucher_customer").show(); $("#lbl_rvoucher_Customer").text("Customer"); $("#div_duedate").hide();
    }
    //    }
    //});
}


function GetCustomers(typeID) {
    var myurl = "/Customer/GetSubCustomerList";
    var mydata = new Object();
    mydata.typeID = typeID;
    XHRPOSTRequest(myurl, mydata, function (result) {

        console.log(result);

        if (result.length !== 0) {
            $('.ddl_rvoucher_Customer').empty();
            $('#ddl_rvoucher_Customer').val("select");
            $('.ddl_rvoucher_Customer').append('<option value=""></option>');
            for (var i = 0; i < result.length; i++) {
                $('.ddl_rvoucher_Customer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            //$('#ddl_rvoucher_Customer').val(0);
            if (typeID == 1) { customerlist = result }
            if (typeID == 2) { Subcustomerlist = result }
        }

    });
}



function SetCustomers() {
    if (customerlist.length !== 0) {
        $(".ddl_rvoucher_Customer").empty();
        $('#ddl_rvoucher_Customer').val("select");
        for (var i = 0; i < customerlist.length; i++) {
            $('.ddl_rvoucher_Customer').append('<option value="' + customerlist[i].Id + '">' + customerlist[i].Name + '</option>');
        }
        $('.ddl_rvoucher_Customer').val(0);
    }
}

function SetSubCustomers() {
    if (Subcustomerlist.length !== 0) {
        $(".ddl_rvoucher_Customer").empty();
        $('#ddl_rvoucher_Customer').val("select");
        for (var i = 0; i < Subcustomerlist.length; i++) {
            $('.ddl_rvoucher_Customer').append('<option value="' + Subcustomerlist[i].Id + '">' + Subcustomerlist[i].Name + '</option>');
        }
        $('.ddl_rvoucher_Customer').val(0);
    }
}


function PrintRecieptVoucher(_voucherInvoiceId) {
    var myurl = "/Report/DownloadReceiptVoucher?cv=123&Preview=true&voucher=" + _voucherInvoiceId + "&typeID=" + (($('#chk_rvoucher_customer').prop('checked') == false) ? 1 : 2);
    $("#PrintPreviewIFrame").off("load");
    $("#PrintPreviewIFrame").attr("src", myurl);
    $("#PrintPreviewModal").modal('show');
    $("#PrintPreviewIFrame").on("load", function () {
        this.contentWindow.print();
    });


}

function DownloadReceiptVoucher(_voucherInvoiceId) {
    var myurl = "/Report/DownloadReceiptVoucher?";
    var mydata = new Object();
    mydata.voucher = _voucherInvoiceId;
    mydata.typeID = ($('#chk_rvoucher_customer').prop('checked') == false) ? 1 : 2;
    DownloadFiles(myurl, mydata);
}

function GetReceiptVoucherByInvoiceId(_voucherInvoiceId) {
    var myurl = "/ReceiptVoucher/GetReceiptVoucherByInvoiceId";
    var mydata = new Object();
    mydata._voucherInvoiceId = _voucherInvoiceId;
    mydata.custType = ($('#chk_rvoucher_customer').prop('checked') == false) ? 1 : 2;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            console.log(result);
            ClearReceipt();
            $('#AddNewVoucherModal').modal('show'); $(".modal-title").text("Edit Monthly Receipt");
            $('#txt_rvoucher_InvoiceNo').val(result.invoiceNo);
            // $('#dtp_rvoucher_ActivityDate').val(moment(result.rActivityDate).format('YYYY-MM-DD'));
            if (result.rActivityDate != null) { $('#dtp_rvoucher_ActivityDate').val(FormatDateTimeToDisplay(GetJSDate(result.rActivityDate))); }
           // $('#dtp_rvoucher_ActivityDate').datepicker({ format: 'yyyy-mm-dd', clearBtn: true });
            $('#ddl_rvoucher_Type').val(result.TypeId);
            
            $('#txt_rvoucher_Received').val(result.rRecived);
            var balNet = (result.rbalance + result.rRecived) * 1;
            $('#txt_rvoucher_Balance').val(balNet);
            $('#txt_rvoucher_GrossAmount').val(balNet - result.rRecived);
            $('#txt_rvoucher_Comments').val(result.rComments);
            // $('#txt_rvoucher_DueDate').val(result.);
            if (result.TypeId == 1) {
                // SetCustomers();
                $("#div_rvoucher_RBalance").show(); $("#div_voucher_balance").show(); $("#div_rvoucher_customer").show(); $("#lbl_rvoucher_Customer").text("Customer"); $("#div_duedate").hide(); SetCustomers();

            }
            if (result.TypeId == 2) {
               
                // SetSubCustomers(); $("#div_rvoucher_RBalance").show(); $("#div_voucher_balance").show(); $("#div_rvoucher_customer").show(); $("#lbl_rvoucher_Customer").text("Subscription Customer"); $("#div_duedate").show();
                $("#div_rvoucher_RBalance").show(); $("#div_voucher_balance").show(); $("#div_rvoucher_customer").show(); $("#lbl_rvoucher_Customer").text("Subscription Customer"); $("#div_duedate").show(); SetSubCustomers();
                $("#txt_rvoucher_DueDate").val(result.DueDate);
                $("#lbl_rvoucher_MonthlyFee").val(result.rSubscriptionAmount);

            }// $("#div_startdate").show(); }


            $("#ddl_rvoucher_Customer").val(result.rCustomerId);
            //GetCustomerBalance(result.rCustomerId);
            editMode = true;
        }

    });
}