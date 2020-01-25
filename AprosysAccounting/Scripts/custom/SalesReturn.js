function SalesReturnTrigger() {
    GetSalesInvoicesList();
    GetCustomersData();
    GetSalesPersonData();
    $("#txt_sell_PaidAmount").attr('disabled', true);

}
var customerlist = new Array();
var ItemList = new Array();
var ItemNewSalesList = new Array();
var maxID = 1;
var editMode = false;

$(".ClearSalescontrols").on("click", function () { ClearSellitem(); });

function GetSalesInvoicesList() {
    var myurl = "/SalesReturn/GetSaleInvoicesList";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.ddl_sale_invoices').append(cloneDom);
        }
        $('.ddl_sale_invoices').chosen();
        $(".chosen-select").chosen({ width: '100%' });
        $('#ddl_sale_invoices').val('').trigger("chosen:updated");
    });
}


$(document).on('change', '#ddl_sale_invoices', function () {
    ClearSell();
    GetOrderDetailsByInvoiceNo($(this).val());
});

function GetOrderDetailsByInvoiceNo(invoiceNo) {

    GetSellByInvoiceNumber(invoiceNo);
}


function GetSellByInvoiceNumber(invoiceNo) {
    var myurl = "/Sales/GetSaleByInvoiceId";
    var mydata = new Object();
    mydata._saleInvoiceId = invoiceNo;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {

            $('#SalesReturnModal').modal('show'); $(".modal-title").text("Sales Return");
            //ClearSell(); ItemNewSalesList = [];
            $("#div_sell_Service").hide();
            SetCustomers(); var paid = 0;
            $("#dtp_sell_ActivityDate").val(moment(result.activitydate).format('YYYY-MM-DD'));
            for (i = 0; i < result.length; i++) {
                if (result[i].TYPED == "TOTALS") {
                    var disableCreditCheck = false;
                    if (result[i].IsSalesCredit * 1 > 0) {
                        $('#chk_Sale_customer_IsSalesCredit').prop('checked', true);
                        if (result[i].PAID > 0) {
                            disableCreditCheck = true;
                        }

                    }
                    if (disableCreditCheck) {
                        $('#chk_Sale_customer_IsSalesCredit').attr("disabled", "disabled");
                    } else {
                        $('#chk_Sale_customer_IsSalesCredit').removeAttr("disabled");
                    }

                    $("#txt_sell_InvoiceNo").val(result[i].InvoiceNo);
                    paid = result[i].PAID * 1;
                    $("#dd_sell_Customer").val(result[i].cstID);
                    if (result[i].ACTTIMESTAMP != null) { $('#dtp_sell_ActivityDate').val(FormatDateTimeToDisplay(GetJSDate(result[i].ACTTIMESTAMP))); }

                    $("#txt_sell_Comments").val(result[i].COMMENTS);
                    if (result[i].CreditPaidDate) {
                        $("#txt_CreditPaymentNote").text("Sold on Credit. Payment Recieved On " + FormatDateTimeToDisplay(GetJSDate(result[i].CreditPaidDate)));
                    }
                }
                else {
                    var _serviceID = result[i].CoaId;
                    var _itemCode = result[i].ItemCode;
                    var _itemId = (_serviceID == 0) ? result[i].ItemId : 0;
                    var _itemName = (_serviceID > 0) ? result[i].SERVICETYPE : result[i].Name;
                    var _itemUnit = result[i].Unit == null ? "" : result[i].Unit;
                    var _itemQty = result[i].Quantity;
                    var _itemUnitPrice = result[i].UnitPrice;
                    var _amount = result[i].AMOUNT;
                    var _tax = result[i].TAX;

                    var _isServiceItem = (_serviceID > 0) ? true : false;
                    ItemNewSalesList.push({ listitemid: maxID * 1, id: _itemId, itemCode: _itemCode, itemName: _itemName, itemUnit: _itemUnit, itemQty: _itemQty, itemUnitPrice: _itemUnitPrice, itemAmount: _amount, Tax: _tax, coaID: _serviceID, isServiceItem: _isServiceItem });
                    var max = Math.max.apply(Math, ItemNewSalesList.map(function (o) { return o.listitemid; }));
                    maxID = max + 1;
                    BindItemtable(ItemNewSalesList, true);
                }
            }
            $("#txt_sell_PaidAmount").val(paid);
            $("#txt_sell_Balance").val($("#txt_sell_NetAmount").val() * 1 - paid);
            editMode = true;
        }
    });
}


function SetCustomers() {
    if (customerlist.length !== 0) {
        $(".ddl_sell_Customer").empty();
        for (var i = 0; i < customerlist.length; i++) {
            $('.ddl_sell_Customer').append('<option value="' + customerlist[i].Id + '">' + customerlist[i].Name + '</option>');
        }
        $('.ddl_sell_Customer').val(0);
    }
}
function GetCustomersData(selected) {
    var myurl = "/Customer/GetCustomerList";
    var mydata = new Object();
    mydata.typeID = 1;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('#dd_sell_Customer').html('');
            for (var i = 0; i < result.length; i++) {
                $('#dd_sell_Customer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            $('#dd_sell_Customer').val("select");
            customerlist = result;
            if (selected) { $('#dd_sell_Customer').val(selected); }

        }
    });
}

function BindItemtable(xx, AllowDelete) {
    var Netamount = 0;
    var Nettaxamount = 0;
    $('.tblSalesReturn').empty();
    for (var i = 0; i < ItemNewSalesList.length; i++) {
        $(".tblSalesReturn").append('<tr><td>' + ItemNewSalesList[i].itemCode + '</td><td>'
            + ItemNewSalesList[i].itemName + '</td><td>' + ItemNewSalesList[i].itemUnit
            + '</td><td class="NumbersAlign">' + ItemNewSalesList[i].itemQty
            + '</td><td class="NumbersAlign">' + CurrencyFormat(ItemNewSalesList[i].itemUnitPrice)
            + '</td><td class="NumbersAlign">' + CurrencyFormat(ItemNewSalesList[i].Tax)
            + '</td><td class="NumbersAlign">' + CurrencyFormat(ItemNewSalesList[i].itemAmount)
            + '</td><td class="hidden">' + ItemNewSalesList[i].coaID + '</td><td class="hidden">' + ItemNewSalesList[i].isServiceItem
            + '</td><td class="text-center"><input type="button" class="btn btn-primary return" rel=' + ItemNewSalesList[i].listitemid
            + ' style="margin-left:5%" value="Return" /></td></tr>');
        Netamount = Netamount + ItemNewSalesList[i].itemAmount;
        Nettaxamount = Nettaxamount + ItemNewSalesList[i].Tax;
    }
    $('#txt_sell_PaidAmount').val(Netamount);
    $('#txt_sell_NetAmount').val(Netamount);
    $('#txt_sell_NetTaxAmount').val(Nettaxamount);
    $('#txt_sell_Balance').val((Netamount * 1) - ($('#txt_sell_PaidAmount').val() * 1));
}

function GetSalesPersonData() {
    var myurl = "/Sales/GetSalesPersonList";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.dd_sell_sales_person').append(cloneDom);
        }
        //$('.dd_sell_sales_person').chosen();
        //$(".chosen-select").chosen({ width: '100%' });
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
    $("#txt_CreditPaymentNote").text("");

}

function ClearSell() {
    ClearSellitem();
    ItemNewSalesList = [];
    $('#dd_sell_Customer').html('');
    $("#txt_sell_NetAmount").val("");
    $("#txt_sell_PaidAmount").val("");
    $("#txt_sell_Balance").val("");
    // $("#dtp_sell_ActivityDate").val("");
    $('#dd_sell_sales_person').val(0)
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


$(document).on('click', '.return', function () {
    clearSaleRetunForm();
    var $row = $(this).closest("tr"), ItemCode = $row.find("td:first-child").text();
    var ItemName = $row.find("td:nth-child(2)").text();
    var TotalQuatity = $row.find("td:nth-child(4)").text();
    $("#txt_return_quantity").attr('rel', TotalQuatity);
    console.log(TotalQuatity);
    console.log(ItemName);
    item_name
    $('#txt_sell_return_InvoiceNo').val($('#txt_sell_InvoiceNo').val());
    $('#item_name').val(ItemName);
    $('#txtItemCode').val(ItemCode);
    $('#NewSalesReturnModal').modal('show');
});


$(document).on('click', '#btnSalesReturn', function () {
    if ($("#txt_return_quantity").val().trim().length == 0) { $("#btnSalesReturn").attr("disabled", false); toastr.warning("Please Enter Quantity "); return; }
    if (($("#txt_return_quantity").val() * 1) > ($("#txt_return_quantity").attr('rel') * 1)) { $("#btnSalesReturn").attr("disabled", false); toastr.warning("Please Enter less return item quantity then sales item quantity "); return; }

    var obj = new Object();
    obj = {
        InvoiceNo: $('#txt_sell_return_InvoiceNo').val(),
        ItemCode: $('#txtItemCode').val(),
        ItemName: $('#item_name').val(),
        Quantity: $("#txt_return_quantity").val(),
        Comments: $("return_reason_comments").val()
    };

    var myurl = "/SalesReturn/SaveSalesReturn";
    XHRPOSTRequest(myurl, obj, function (result) {
        $('#NewSalesReturnModal').modal('hide');
    });
});


function clearSaleRetunForm() {
    $('#txt_sell_return_InvoiceNo').val('');
    $('#txtItemCode').val('');
    $('#item_name').val('');
    $("#txt_return_quantity").val('');
}
