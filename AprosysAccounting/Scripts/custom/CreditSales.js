var customerlist = new Array();
var customerInvoicelist = new Array();
function CreditSalesTrigger() {
    updateDatePickers();
    LoadCreditSalesTable();
    GetUnpaidCustomers();
    LoadBankNames();


    $("#btn_creditSales_Search").on("click", function () {
        if ($("#dtp_creditSales_MainStartDate").val() != "" && $("#dtp_creditSales_MainEndDate").val() != "") {
            LoadCreditSalesTable();
        }
        else { toastr.warning("Please Select Dates"); }
    });

    $("#btn_creditSales_AddReceiving").on("click", function () {
        ClearCredeitSales();
        $('#AddReceivingModal').modal('show'); $(".modal-title").text('Add Receiving');
        // SetUnPaidCustomers();
        //$("#dtp_creditSales_paidDate").val(moment().format('YYYY-MM-DD'));
        SetUnpaidCustomers(0);
        $('#dtp_creditSales_paidDate').val(FormatDateTimeToDisplay(new Date()));
    });

    $(document).on('change', '#dd_creditSales_Customer', function () {
        var custId = $(".dd_creditSales_Customer").val();
        if (custId > 0) {
            GetUnPaidCreditCustomerInvoices(custId);
        }
    });

    $(document).on('change', '#dd_creditSales_InvoiceNo', function () {
        var _invoiceId = $(".dd_creditSales_InvoiceNo").val();
        //if (_invoiceId > 0) {
        var selectedItem = customerInvoicelist.find(x => x.invoiceNo == _invoiceId);
        if (selectedItem != null) {
            $("#txt_creditSales_PaidAmount").val(selectedItem.paidAmount);
            $("#txt_creditSales_InvoiceAmount").val(selectedItem.invoiceAmount);
            $("#txt_creditSales_Amount").val(selectedItem.netAmount);
            $("#txt_creditSale_SaleDate").val(FormatDateTimeToDisplay(GetJSDate(selectedItem.sellDate)));

            //  }
        }
    });

    $("#btn_creditSales_Save").on("click", function () {
        //$("#btn_customer_Save").attr("disabled", true);
        UpdateUnPaidCreditSales("", 0, "");
    });

    $(document).on('click', '#btn_creditSales_Paid', function () {
        var invoiceId = $(this).attr('rel');
        var custname = $(this).attr('cust');
        var custID = $(this).attr('custID') * 1;
        var amount = $(this).attr('amount') * 1;
        var selldate = $(this).attr('selldate');



        //ClearCredeitSales();
        //$('#AddReceivingModal').modal('show'); $(".modal-title").text('Add Receiving');
        //SetUnpaidCustomers(custID);
        //$('#dd_creditSales_InvoiceNo').append('<option value="' + invoiceId + '">' + invoiceId + '</option>');
        //$("#dd_creditSales_Customer").attr("disabled", true);
        //$("#dd_creditSales_InvoiceNo").attr("disabled", true);
        //$("#txt_creditSales_Amount").val(amount);
        //$("#txt_creditSale_SaleDate").val(selldate);
        //$('#dtp_creditSales_paidDate').val(FormatDateTimeToDisplay(new Date()));

        //if (confirm("Are you sure to Receive Payment on invoiceId= " + invoiceId)) {
        //UpdateUnPaidCreditSales(invoiceId, custID, FormatDateTimeToDisplay(new Date()));
        //}
        $("#btn_creditSales_AddReceiving").click();
        $("#dd_creditSales_Customer").val(custID);
        GetUnPaidCreditCustomerInvoices(custID, function () {
            $('#AddReceivingModal').modal('show');
            $("#dd_creditSales_InvoiceNo").val(invoiceId);
            $("#dd_creditSales_InvoiceNo").change();
        });
    });
    $(document).on('click', '#btn_creditSales_View', function () {
        var invoiceId = $(this).attr('rel');
        var custname = $(this).attr('cust');
        GetSellByInvoiceNumber(invoiceId, custname);
    });
    $(document).on('click', '#btn_creditSales_Print', function () {
        var invoiceId = $(this).attr('rel');
        var custname = $(this).attr('cust');
        PrintCreditSaleInvoice(invoiceId);
    });
    $(document).on('click', '#btn_creditsales_Download', function () {
        var invoiceId = $(this).attr('rel');
        DownloadCreditSaleInvoice(invoiceId);
    });

    $("#ddl_CreditSales_PaymentMode").change(function () {
        if ($(this).val() * 1 == 1) {
            $("#ddl_CreditSales_Bank").val(null);
            $("#ddl_CreditSales_Bank").attr("disabled", "disabled");
            $("#ddl_CreditSales_Bank").trigger('chosen:updated');
            $("#txt_creditSales_DocNum").attr("disabled", "disabled");
        } else {

            $("#ddl_CreditSales_Bank").removeAttr("disabled");
            $("#ddl_CreditSales_Bank").trigger('chosen:updated');
            $("#txt_creditSales_DocNum").removeAttr("disabled");
        }
    });
    //$('#tblCreditSalesInformation tbody').on('click', 'tr .record', function () {

    //    console.log(oTable.row(this).data());
    //});


}
function updateDatePickers() {
    $("#dtp_creditSales_MainStartDate").val(moment().subtract('days', 90).format('YYYY-MM-DD'));
    $("#dtp_creditSales_MainEndDate").val(moment().format('YYYY-MM-DD'));
}
function LoadBankNames() {
    $.post("/CreditSales/GetAllBankNames", {}, function (data) {
        $("#ddl_CreditSales_Bank").append("<option></option>");
        for (var i = 0; i < data.length; i++) {
            var item = $("<option></option>");
            item.text(data[i]);
            item.attr("value", data[i]);
            $("#ddl_CreditSales_Bank").append(item);
        }
        $("#ddl_CreditSales_Bank").chosen({ width: '100%' });
    }, "json");
}
function ClearCredeitSales() {
    $(".dd_creditSales_InvoiceNo").empty();
    $("#txt_creditSales_Amount").val("");
    $("#txt_creditSale_SaleDate").val("");
    $("#dd_creditSales_Customer").attr("disabled", false);
    $("#dd_creditSales_InvoiceNo").attr("disabled", false);
    $("#txt_creditSales_PaidAmount").val("");
    $("#txt_creditSales_InvoiceAmount").val("");
    $("#txt_creditSales_Comment").val("");
    $("#txt_creditSales_DocNum").val("");
    $("#txt_creditSales_DocNum").removeAttr("disabled");
    $("#ddl_CreditSales_PaymentMode").val(2);//By Cheque
    $("#ddl_CreditSales_Bank").val(null);
    $("#ddl_CreditSales_Bank").trigger('chosen:updated');
    $('#sales_person').val('');
}


function LoadCreditSalesTable() {
    oTable = $("#tblCreditSalesInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/creditSales/GetUnPaidCreditSalesList",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'invoiceNo', "className": 'text-compressed  bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'customerName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },

           {
               "data": 'sellDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                   if (obj["sellDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                   else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["sellDate"])) + '</td>' }
               }
           },
            {
                "data": 'netAmount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
          {
              "data": 'invoiceNo', "className": 'text-compressed id bold', "bSortable": false,
              "render": function (full, type, obj) {

                  //var obj_ = { InvoiveNo: obj.invoiceNo, customer: obj.customerName }
                  //console.log(obj_);
                  var html2 = "";
                  html2 += ' <input type="button"  value="PAID" rel=' + obj.invoiceNo + ' salePerId="' +  '" cust="' + obj.customerName + '" custID=' + obj.customerID + ' amount=' + obj.netAmount + ' selldate="' + FormatDateTimeToDisplay(GetJSDate(obj.sellDate)) + '" id="btn_creditSales_Paid" class="form-control" />'
                                          + ' <input type="button"  value="View" rel=' + obj.invoiceNo + ' cust="' + obj.customerName + '" id="btn_creditSales_View" class="form-control"/>'
                                           + ' <input type="button"  value="Print" rel=' + obj.invoiceNo + ' cust="' + obj.customerName + '" id="btn_creditSales_Print" class="form-control"/>'
                                            + ' <input type="button"  value="Download" rel=' + obj.invoiceNo + ' id="btn_creditsales_Download" class="form-control" />';
                  //html2 += ' <input type="button"  value="PAID" rel=' + obj.invoiceNo + ' id="btn_creditSales_Paid" class="form-control record" />';

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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_creditSales_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_creditSales_MainEndDate').val()).format("YYYY-MM-DD") });
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


function GetUnpaidCustomers() {
    var myurl = "/creditSales/GetUnPaidCreditCustomerList";
    var mydata = new Object();
    mydata.typeID = 1;
    customerlist = [];
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            //$('#dd_creditSales_Customer').html('');
            //$('#dd_creditSales_Customer').val("select");
            for (var i = 0; i < result.length; i++) {
                //  $('#dd_creditSales_Customer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            customerlist = result;
            // $('#dd_creditSales_Customer').val("select");

        }
    });
}
function SetUnpaidCustomers(custid) {
    if (customerlist.length !== 0) {
        $(".dd_creditSales_Customer").empty();
        for (var i = 0; i < customerlist.length; i++) {
            $('.dd_creditSales_Customer').append('<option value="' + customerlist[i].Id + '">' + customerlist[i].Name + '</option>');
        }
        $('.dd_creditSales_Customer').val(0);
        console.log(custid);
        if (custid > 0) {
            $('.dd_creditSales_Customer').val(custid);
        }
    }
}
//function SetUnPaidCustomers(custid) {
//    if (customerlist != null) {
//        for (var i = 0; i < customerlist.length; i++) {
//            $('#dd_creditSales_Customer').append('<option value="' + customerlist[i].Id + '">' + customerlist[i].Name + '</option>');
//            $('#dd_creditSales_Customer').val("select");
//        }
//        if (custid > 0) {
//            $('#dd_creditSales_Customer').val(custid);
//        }
//    }
//}


function GetUnPaidCreditCustomerInvoices(_custID, callback) {
    var myurl = "/creditSales/GetUnPaidCreditCustomerInvoices";
    var mydata = new Object();
    mydata.custID = _custID;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            ClearCredeitSales();
            $('#dd_creditSales_InvoiceNo').html('');
            $('#dd_creditSales_InvoiceNo').val("select");
            for (var i = 0; i < result.length; i++) {
                $('#dd_creditSales_InvoiceNo').append('<option value="' + result[i].invoiceNo + '">' + result[i].invoiceNo + '</option>');
            }
            customerInvoicelist = result;
            $("#txt_creditSales_Amount").val(result[0].netAmount);
            $("#txt_creditSale_SaleDate").val(FormatDateTimeToDisplay(GetJSDate(result[0].sellDate)));

            $("#txt_creditSales_InvoiceAmount").val(result[0].invoiceAmount);
            $("#txt_creditSales_PaidAmount").val(result[0].paidAmount);
            $('#sales_person').val(result[0].salePerson);
            //if (selected) { $('#dd_sell_Customer').val(selected); }
            if (callback) {
                callback();
            }
        }
    });
}


function UpdateUnPaidCreditSales(_invoiceno, _customerId, _creditpaiddate, _amount) {
    if (_invoiceno != "" && _customerId * 1 > 0 && _creditpaiddate != "") {
        var obj = {
            customerID: _customerId,
            invoiceNo: _invoiceno,
            creditPaidDate: _creditpaiddate,

        };
    }
    else {
        if ($("#dtp_creditSales_paidDate").val().trim().length == 0) { $("#btn_creditSales_Save").attr("disabled", false); toastr.warning("Please Select Paid Date"); return; }
        if ($("#dd_creditSales_Customer").val() * 1 == 0) { $("#btn_creditSales_Save").attr("disabled", false); toastr.warning("Please Select Customer"); return; }
        if ($('#dd_creditSales_InvoiceNo').val() == null) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Select Invoice"); return; }
        if ((($("#txt_creditSales_Amount").val() * 1) + ($("#txt_creditSales_PaidAmount").val() * 1)) > $("#txt_creditSales_InvoiceAmount").val() * 1) { $("#btn_customer_Save").attr("disabled", false); alert('Please enter less amount then invoice amount.'); return; }
        var obj = {
            customerID: $("#dd_creditSales_Customer").val() * 1,
            invoiceNo: $('#dd_creditSales_InvoiceNo').val(),
            creditPaidDate: $("#dtp_creditSales_paidDate").val(),
            Amount: $("#txt_creditSales_Amount").val(),
            PaymentMode: $("#ddl_CreditSales_PaymentMode").val(),
            DocumentID: $("#txt_creditSales_DocNum").val(),
            Comment: $("#txt_creditSales_Comment").val(),
            BankName: $("#ddl_CreditSales_Bank").val()
        };
        if (obj.PaymentMode == 2 && (!obj.DocumentID || !obj.BankName)) {
            window.alert("Bank Name and Cheque Number are necessary for Payment By Cheque");
            return;
        }
    }
    var invoiceNo = obj.invoiceNo;
    ShowAjaxLoader();
    var myurl = "/creditSales/UpdateUnPaidCreditSales";
    var mydata = new Object();
    mydata.paramcustomer = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == invoiceNo) {
            HideAjaxLoader();
            showNotification("Amount Received", "success");
            ClearCredeitSales();
            $('#AddReceivingModal').modal('hide');
            if ($("#dtp_creditSales_MainStartDate").val() == "" && $("#dtp_creditSales_MainEndDate").val() == "") {
                updateDatePickers();
            }
            GetUnpaidCustomers();
            LoadCreditSalesTable();
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
                   PrintCreditSaleInvoice(result);
               }
           }
       );
        }
        else { HideAjaxLoader(); toastr.warning(result); }

    });
}






function GetSellByInvoiceNumber(invoiceNo, custname) {
    var myurl = "/Sales/GetSaleByInvoiceId";
    var mydata = new Object();
    mydata._saleInvoiceId = invoiceNo;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {

            $('#AddNewSellModal').modal('show'); $(".modal-title").text("View Sales");
            ClearSell(); ItemNewSalesList = [];
            $("#div_sell_Service").hide();
            //SetCustomers();
            var paid = 0;
            $("#dd_sell_Customer").val(custname);
            $("#dtp_sell_ActivityDate").val(moment(result.activitydate).format('YYYY-MM-DD'));
            for (i = 0; i < result.length; i++) {
                if (result[i].TYPED == "TOTALS") {
                    if (result[i].IsSalesCredit * 1 > 0) { $('#chk_Sale_customer_IsSalesCredit').prop('checked', true); }

                    $("#txt_sell_InvoiceNo").val(result[i].InvoiceNo);
                    paid = result[i].PAID * 1;
                    if (result[i].ACTTIMESTAMP != null) { $('#dtp_sell_ActivityDate').val(FormatDateTimeToDisplay(GetJSDate(result[i].ACTTIMESTAMP))); }

                    $("#txt_sell_Comments").val(result[i].COMMENTS);
                }
                else {
                    var _serviceID = result[i].CoaId;
                    var _itemCode = result[i].ItemCode;
                    var _itemId = (_serviceID == 0) ? result[i].ItemId : 0;
                    var _itemName = (_serviceID > 0) ? result[i].SERVICETYPE : result[i].Name;
                    var _itemUnit = result[i].Unit;
                    var _itemQty = result[i].Quantity;
                    var _itemUnitPrice = result[i].UnitPrice;
                    var _amount = result[i].AMOUNT;
                    var _tax = result[i].TAX;

                    var _isServiceItem = (_serviceID > 0) ? true : false;
                    ItemNewSalesList.push({ listitemid: maxID * 1, id: _itemId, itemCode: _itemCode, itemName: _itemName, itemUnit: _itemUnit, itemQty: _itemQty, itemUnitPrice: _itemUnitPrice, itemAmount: _amount, Tax: _tax, coaID: _serviceID, isServiceItem: _isServiceItem });
                    var max = Math.max.apply(Math, ItemNewSalesList.map(function (o) { return o.listitemid; }));
                    maxID = max + 1;
                    BindItemtable(ItemNewSalesList);
                }
            }
            $("#txt_sell_PaidAmount").val(paid);
            $("#txt_sell_Balance").val($("#txt_sell_NetAmount").val() * 1 - paid);
            editMode = true;
        }
    });
}


function ClearSellitem() {
    // listItem = [];

    $('#txt_sell_Code').val("");
    $('#txt_sell_item').val(0);
    $('#txt_sell_Unit').val("");
    $('#txt_sell_Qty').val("");
    $('#txt_sell_Unitprice').val("");
    $('#txt_sell_Amount').val("");
    $('#txt_sell_tax').val("");
    $('#txt_sell_service_Amount').val("");
    $('#txt_sell_service_unitprice').val("");
    //$('#txt_sell_service_tax').val("");
    $('#txt_sell_service').val(0);

}
function ClearSell() {
    ClearSellitem();
    ItemNewSalesList = [];
    $("#txt_sell_NetAmount").val("");
    $("#txt_sell_PaidAmount").val("");
    $("#txt_sell_Balance").val("");
    $("#dtp_sell_ActivityDate").val("");
    //$("#dd_sell_Vendor").val(0);
    $('.tblCurrentSales').empty();
    $("#div_sell_Item").show();
    $("#div_sell_amount").show();
    $('.tblCurrentSales').empty();
    $("#txt_sell_Comments").val("");
    $("#btn_sell_Save").attr("disabled", false);
    $("#txt_sell_NetTaxAmount").val("");
    $('#chk_Sale_customer_IsSalesCredit').prop('checked', false);
    editMode = false;
}

function BindItemtable() {
    var Netamount = 0;
    var Nettaxamount = 0;
    $('.tblCurrentSales').empty();
    for (var i = 0; i < ItemNewSalesList.length; i++) {
        $(".tblCurrentSales").append('<tr><td>' + ItemNewSalesList[i].itemCode + '</td><td>'
            + ItemNewSalesList[i].itemName + '</td><td>' + ItemNewSalesList[i].itemUnit
            + '</td><td class="NumbersAlign">' + ItemNewSalesList[i].itemQty
            + '</td><td class="NumbersAlign">' + CurrencyFormat(ItemNewSalesList[i].itemUnitPrice)
            + '</td><td class="NumbersAlign">' + CurrencyFormat(ItemNewSalesList[i].Tax)
            + '</td><td class="NumbersAlign">' + CurrencyFormat(ItemNewSalesList[i].itemAmount)
            + '</td><td class="hidden">' + ItemNewSalesList[i].coaID + '</td><td class="hidden">' + ItemNewSalesList[i].isServiceItem
            //+ '</td><td class="text-center"><input type="button" class="btn btn-danger del" rel=' + ItemNewSalesList[i].listitemid
            //+ ' style="margin-left:5%" value="Delete" />'
            + '</td></tr>');
        Netamount = Netamount + ItemNewSalesList[i].itemAmount;
        Nettaxamount = Nettaxamount + ItemNewSalesList[i].Tax;
    }
    //$('#tblSalescurrentInformation').DataTable();
    $('#txt_sell_PaidAmount').val(Netamount);
    $('#txt_sell_NetAmount').val(Netamount);
    $('#txt_sell_NetTaxAmount').val(Nettaxamount);
    $('#txt_sell_Balance').val((Netamount * 1) - ($('#txt_sell_PaidAmount').val() * 1));
    //  if (editMode == false) { $('#txt_sell_PaidAmount').val(0); }
}
var ItemNewSalesList = new Array();
var maxID = 1;

function PrintCreditSaleInvoice(invoiceId) {
    //var myurl = "/Report/DownloadCreditSaleInvoice?cv=123&Preview=true&invoiceId=" + invoiceId;
    var myurl = "/Report/DownloadSaleInvoice?cv=123&Preview=true&invoiceId=" + invoiceId;
    $("#PrintPreviewIFrame").off("load");
    $("#PrintPreviewIFrame").attr("src", myurl);
    $("#PrintPreviewModal").modal('show');
    $("#PrintPreviewIFrame").on("load", function () {
        this.contentWindow.print();
    });
}
function DownloadCreditSaleInvoice(invoiceId) {
    var myurl = "/Report/DownloadSaleInvoice?";
    var mydata = new Object();
    mydata.invoiceId = invoiceId;
    DownloadFiles(myurl, mydata);
}
