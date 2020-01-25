
var customerlist = new Array();
var ItemList = new Array();
var ServiceList = new Array();
var ItemNewSalesList = new Array();
var maxID = 1;
var editMode = false;

function SalesInformation() {
    $("#dtp_sell_MainStartDate").val(moment().subtract('days', 0).format('YYYY-MM-DD'));
    $("#dtp_sell_MainEndDate").val(moment().format('YYYY-MM-DD'));
    GetItemList(); GetMainCustomers(); LoadSellInfo(); GetServices();  GetSalesPersonData(); GetSalesPersonWithNewCustomerData();
    var x = Date();
    //x.setDate(x.getDate() - 7);
    ApplyTaxes(1);
    $("#btn_sell_Search").on("click", function () {
        if ($("#dtp_sell_MainStartDate").val() != "" && $("#dtp_sell_MainEndDate").val() != "") {
            LoadSellInfo();
        }
        else { toastr.warning("Please Select Dates"); }
    });

    $(document).on('click', '.close', function () {
        ClearSell();
    });

    $("#btn_sell_AddNewSales").on("click", function () {
        ClearSell();
        GetSellInvoiceNumber();
        //GetCustomersData();
        $("#div_sell_Service").hide();
        //SetCustomers();
    });

    $("#btn_sell_Save").on("click", function () {
        $("#btn_sell_Save").attr("disabled", true);
        SaveSellInvoice();
    });

    $(document).on('click', '.del', function () {
        var id = $(this).attr('rel') * 1;
        ItemNewSalesList = $.grep(ItemNewSalesList, function (data, index) {
            return data.listitemid != id
        });
        BindItemtable(ItemNewSalesList, !editMode);
    });
    $(".ClearSalescontrols").on("click", function () { ClearSellitem(); });
    var maxID = 1;
    $(".AddSales").on("click", function () {
        if (($("#txt_sell_item option:selected").attr('data-groupid') * 1) == 1) {
            var ItemID = $("#txt_sell_item").val() * 1;
            if (ItemID > 0) {
                //$("button").click(function () {
                //    $("p").hide("slow", function () {
                //       // alert("The paragraph is now hidden");
                //    });
                //});
                GetItemQuantityforSale(ItemID, function (availableqty) {

                    if (availableqty >= ($("#txt_sell_Qty").val() * 1)) {
                        AddSalesintoList();
                    }
                    else { toastr.warning("Required Quantity of Item Not Available"); return; }
                });
            }
        }
        else {
            AddSalesintoList();
        }
    });
    $(document).on('keypress', '#txt_sell_Code', function (e) {
        if (e.which == 26 || e.which == 13) {
            $(this).change();
        }
    });
    $(document).on('change', '#txt_sell_Code', function () {

        var value = $(this).val().toUpperCase();
        var selectedItem = itemlist.find(x => x.itemCode.toUpperCase() == value);
        if (selectedItem != null) {
            ApplyTaxes(1);
            SetItemControls(selectedItem);
            GetRowAmount(this);
        }
        if (selectedItem == null) {
            var selectedService = ServiceList.find(x => x.serviceCode.toUpperCase() == value);
            if (selectedService != null) {
                SetServiceControls(selectedService);
                GetRowAmount(this);
            }
            else { ClearSellitem(); }
        }
        //if (selectedItem == null || selectedService == null) { ClearSellitem();}
        //else { ClearSellitem(); }
    });

    $(document).on('change', '#txt_sell_item', function () {
        var value = $(this).val() * 1;
        var type = $("#txt_sell_item option:selected").attr('data-groupid') * 1;
        ApplyTaxes(type);
        if (type == 1) {
            if (value > 0) {
                var selectedItem = itemlist.find(x => x.id == value);
                SetItemControls(selectedItem);
                GetRowAmount(this);
            }
            else { ClearSellitem(); }
        }
        else {
            ClearForService();
            var selectedService = ServiceList.find(x => x.id == value);
            SetServiceControls(selectedService);
            GetRowAmount(this);
            //  $('#txt_sell_Unitprice').val(selectedService.cost);
        }
        //  ToggleSaleTypes(type == 1 ? false : true);
    });

    $(document).on('change', '#txt_sell_service_unitprice', function () {
        GetServiceRowAmount(this);
    });
    $(document).on('change', '#txt_sell_service_tax', function () {
        GetServiceRowAmount(this);
    });


    $(document).on('change', '#txt_sell_tax', function () {
        GetRowAmount(this);
    });

    $(document).on('change', '#txt_sell_Unitprice', function () {
        GetRowAmount(this);
    });

    $(document).on('change', '#txt_sell_Qty', function () {
        GetRowAmount(this);
    });



    $(document).on('change', '.paid', function () {
        var netAmount = $('.NetAmount').val() * 1;
        var paidamount = $('.paid').val() * 1;
        var amount = netAmount - paidamount;
        $('.balance').val(amount);
    });
    $(document).on('click', '.close', function () {
        ClearSell();
    });
    $(document).on('click', '#btn_sales_Edit', function () {
        var invoiceId = $(this).attr('rel');
        if ($(this).attr('data-isCstActive') == "false") { alert('Customer Deleted, edit cannot pe performed!'); return; }
        GetSellByInvoiceNumber(invoiceId);
    });
    $(document).on('click', '#btn_sales_Delete', function () {

        var invoiceId = $(this).attr('rel');
        if (confirm("Are you sure to Delete invoiceId= " + invoiceId)) {
            DeleteSaleInvoice(invoiceId);
        }
    });
    $(document).on('click', '#btn_sales_Print', function () {

        var invoiceId = $(this).attr('rel'); PrintSaleInvoice(invoiceId);
    });
    $(document).on('click', '#btn_sales_Download', function () {

        var invoiceId = $(this).attr('rel'); DownloadSaleInvoice(invoiceId);
    });

    $('#btn_AddCustomer').click(function () {
        $('.txtCstFname').val('');
        $(".txtCstLname").val('');
        $(".txtCstPhone").val('');
        $(".txtCstEmail").val('');
        $('#txt_customer_cnic').val("");
        $('#txt_customer_ntn').val("");
        $(".dd_sell_new_customer_sales_person").val('0');
        $("#NewCustomerModal").modal('show');
        $("#title_sales_customer").text("Add New Customer");
    })

    $("#btnSaveCustomer").click(function () {
        if ($('.txtCstFname').val().length == 0 || $('.txtCstLname').val().length == 0) { toastr.warning("Please Enter Customer Name"); return; }
        if ($('#dd_sell_new_customer_sales_person').val() == "0" || $('#dd_sell_new_customer_sales_person').val().length == 0 || $('#dd_sell_new_customer_sales_person').val().length == 0) { toastr.warning("Please Select Sales Person"); return; }
        var re = /^[a-z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)?@[a-z][a-zA-Z-0-9]*\.[a-z]+(\.[a-z]+)?$/;
        var preemail = $('.txtCstEmail').val().toLowerCase().trim();
        if (preemail.length > 0 && !re.test(preemail)) {
            $("#btnSaveCustomer").attr("disabled", false); toastr.warning("Please Enter Correct Email"); return;
        }

        if ($('#txt_customer_cnic').val() == null || $("#txt_customer_cnic").val().trim().length == 0) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter CNIC."); return; }
        SaveNewCustomer();
    })

    $("#txt_sell_PaidAmount").attr('disabled', true);
}

function SaveNewCustomer() {
    var obj = {
        id: 0,
        lastName: $('.txtCstLname').val(),
        firstName: $(".txtCstFname").val(),
        phone: $(".txtCstPhone").val(),
        email: $(".txtCstEmail").val(),
        cnic: $("#txt_customer_cnic").val(),
        ntn: $("#txt_customer_ntn").val(),
        salesPerson: $('#dd_sell_new_customer_sales_person').val()
    };
    var myurl = "/Customer/SaveCustomer";
    var mydata = new Object();
    mydata.paramcustomer = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != "") {
            //GetCustomersData(result * 1);
            GetCustomerListBySalesPersonID(obj.salesPerson);
            $("#NewCustomerModal").modal('hide');
            $('#dd_sell_sales_person').val(obj.salesPerson);
            $('#dd_sell_Customer').val(result);
        }
    });
}

function ToggleSaleTypes(sw) {
    $('#txt_sell_Code').attr('disabled', sw);
    $('#txt_sell_Qty').attr('disabled', sw);
}

function ClearForService() {
    $('#txt_sell_Code').val('');
    $('#txt_sell_Qty').val('1');
    $('#txt_sell_Unit').val('');
    $('#txt_sell_Unitprice').val('');
    $('#txt_sell_tax').val('');
    $('#txt_sell_Amount').val('');
}


function GetRowAmount(curr) {
    var qty = $(curr).closest('.row').find('#txt_sell_Qty').val() * 1;
    var unitPrice = $(curr).closest('.row').find('#txt_sell_Unitprice').val() * 1;
    var tax = $(curr).closest('.row').find('#lbl_sell_tax').html() * 1;
    var taxAmount = (qty * unitPrice) * (tax / 100);
    $('#txt_sell_tax').val(taxAmount);
    // $('#txt_sell_Amount').val(taxAmount + (qty * unitPrice));
    $('#txt_sell_Amount').val((qty * unitPrice));
}

function GetServiceRowAmount(curr) {
    var qty = 1;
    var unitPrice = $(curr).parent().parent().find('#txt_sell_service_unitprice').val() * 1;
    var tax = $(curr).parent().parent().find('#lbl_sell_service_tax').html() * 1;
    var taxAmount = (qty * unitPrice) * (tax / 100);
    $('#txt_sell_service_tax').val(taxAmount);
    $('#txt_sell_service_Amount').val(taxAmount + (qty * unitPrice));
}

function SetItemControls(selectedItem) {
    if ($("#txt_sell_Qty").val() * 1 == 0) { $("#txt_sell_Qty").val(1); }
    $("#txt_sell_item").val(selectedItem.id);
    $("#txt_sell_Code").val(selectedItem.itemCode);
    $("#txt_sell_Unit").val(selectedItem.unit);
    $("#txt_sell_Unitprice").val(selectedItem.sellPrice);
    if (!selectedItem.isTaxable) { $('#lbl_sell_tax').html(0); }
    //$("#txt_sell_tax").val(selectedItem.taxPercent);
    //var Amount = ($("#txt_sell_Unitprice").val() * 1) * ($("#txt_sell_Qty").val() * 1);
    //$('#txt_sell_Amount').val(Amount);
}

function SetServiceControls(selectedItem) {
    if ($("#txt_sell_Qty").val() * 1 == 0) { $("#txt_sell_Qty").val(1); }
    $("#txt_sell_item").val(selectedItem.id);
    $("#txt_sell_Code").val(selectedItem.serviceCode);
    // $("#txt_sell_Unit").val(selectedItem.unit);
    $("#txt_sell_Unitprice").val(selectedItem.cost);
    if (!selectedItem.isTaxable) { $('#lbl_sell_tax').html(0); }
    //$("#txt_sell_tax").val(selectedItem.taxPercent);
    //var Amount = ($("#txt_sell_Unitprice").val() * 1) * ($("#txt_sell_Qty").val() * 1);
    //$('#txt_sell_Amount').val(Amount);
}

function GetSellInvoiceNumber() {
    //var myurl = "/Sales/GetNextVoucher";
    //var mydata = new Object();

    //XHRPOSTRequest(myurl, mydata, function (result) {
    //    if (result.length !== 0) {

    $('#AddNewSellModal').modal('show'); $(".modal-title").text("Add New Sales");
    $("#txt_sell_InvoiceNo").val('');
  //Remains last activitydate , client dont want to select currentdate
    //$('#dtp_sell_ActivityDate').val(FormatDateTimeToDisplay(new Date()));
    //    }
    //});
}

function ApplyTaxes(type) {
    $("#lbl_sell_tax").html($.grep(taxes, function (rr) { return rr.Key == type; })[0].Value);
}

function AddSalesintoList() {
    /*    if ($('#chk_sell_Service').prop('checked') == false) {*/
    if ($("#txt_sell_item").val() * 1 == 0) { toastr.warning("Please Select Item"); return; }
    if ($("#txt_sell_Unitprice").val() * 1 == 0) { toastr.warning("Please Enter Sale Unit Price"); return; }
    if ($("#txt_sell_Qty").val() * 1 == 0) { toastr.warning("Please Enter Quantity"); return; }
    var groupID = $("#txt_sell_item option:selected").attr('data-groupid') * 1;
    if ((groupID != 2 && $.grep(ItemNewSalesList, function (rx) { return rx.id == $("#txt_sell_item").val() }).length > 0) || $.grep(ItemNewSalesList, function (rx) { return rx.coaID == $("#txt_sell_item").val() }).length > 0) { toastr.warning("Already in the list"); return; }

    if ($("#txt_sell_item").val() * 1 > 0) {
        var ItemCode = $("#txt_sell_Code").val();
        var ItemID = $("#txt_sell_item").val() * 1;

        var ItemName = groupID == 2 ? ServiceList.find(x => x.id == ItemID).name : itemlist.find(x => x.id == ItemID).name;
        var itemUnit = $("#txt_sell_Unit").val();
        var itemQty = $("#txt_sell_Qty").val() * 1;
        var itemUnitPrice = $('#txt_sell_Unitprice').val() * 1;
        var Amount = $('#txt_sell_Amount').val() * 1;
        var Tax = $('#txt_sell_tax').val() * 1;
        if (groupID == 2) { ItemNewSalesList.push({ listitemid: maxID * 1, id: 0, itemCode: ItemCode, itemName: ItemName, itemUnit: 1, itemQty: itemQty, itemUnitPrice: itemUnitPrice, itemAmount: Amount, Tax: Tax, coaID: ItemID, isServiceItem: true }); }
        else {
            ItemNewSalesList.push({ listitemid: maxID * 1, id: ItemID, itemCode: ItemCode, itemName: ItemName, itemUnit: itemUnit, itemQty: itemQty, itemUnitPrice: itemUnitPrice, itemAmount: Amount, Tax: Tax, coaID: 0, isServiceItem: false });
        }
        var max = Math.max.apply(Math, ItemNewSalesList.map(function (o) { return o.listitemid; }));
        maxID = max + 1;
        BindItemtable(ItemNewSalesList, true);
        ClearSellitem();
    }
    //}
    //else {
    //    if ($("#txt_sell_Unitprice").val() * 1 == 0) { toastr.warning("Please Enter Service Price"); return; }
    //    {
    //        var serviceID = $("#txt_sell_item").val() * 1;
    //        var ItemName = ServiceList.find(x => x.id == serviceID).name;
    //        //  var itemUnit = '1';
    //        // var itemQty = 1;
    //        var itemUnitPrice = $('#txt_sell_Unitprice').val() * 1;
    //        var Amount = $('#txt_sell_Amount').val() * 1;
    //        var Tax = $('#txt_sell_tax').val() * 1;
    //        ItemNewSalesList.push({ listitemid: maxID * 1, id: 0, itemCode: 0, itemName: ItemName, itemUnit: 1, itemQty: 1, itemUnitPrice: itemUnitPrice, itemAmount: Amount, Tax: Tax, coaID: serviceID, isServiceItem: true });
    //        var max = Math.max.apply(Math, ItemNewSalesList.map(function (o) { return o.listitemid; }));
    //        maxID = max + 1;
    //        BindItemtable(ItemNewSalesList);
    //        ClearSellitem();
    //    }
    //}
}

function GetItemList() {
    var myurl = "/Item/GetItemList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_sell_item').append('<optgroup label="Items">');
            //$('.ddl_sell_item').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_sell_item').append('<option data-groupid=1 value="' + result[i].id + '">' + result[i].name + '</option>');
            }
            $('.ddl_sell_item').append('</optgroup>');
            itemlist = result;
        }
    });
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


function SetCustomers() {
    if (customerlist.length !== 0) {
        $(".ddl_sell_Customer").empty();
        for (var i = 0; i < customerlist.length; i++) {
            $('.ddl_sell_Customer').append('<option value="' + customerlist[i].Id + '">' + customerlist[i].Name + '</option>');
        }
        $('.ddl_sell_Customer').val(0);
    }
}

function GetMainCustomers() {
    //var myurl = "/Customer/GetCustomerList";
    //var mydata = new Object();
    //mydata.typeID = 1;
    //XHRPOSTRequest(myurl, mydata, function (result) {
    if (customerlist.length !== 0) {
        $('.ddl_sell_MainCustomer').val("select");
        for (var i = 0; i < customerlist.length; i++) {
            $('.ddl_sell_MainCustomer').append('<option value="' + customerlist[i].Id + '">' + customerlist[i].Name + '</option>');
        }

    }
}//);


function BindItemtable(xx, AllowDelete) {
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
            //+ '</td><td class="text-center"><input type="button" class="btn btn-success" value="Edit" />'
            //+ '<input type="button" class="btn btn-danger del" rel=' + ItemNewSalesList[i].listitemid + ' style="margin-left:5%" value="Delete" /></td></tr>');
            + '</td><td class="text-center"><input  ' + (!AllowDelete ? "disabled" : "") + '  type="button" class="btn btn-danger del" rel=' + ItemNewSalesList[i].listitemid
            + ' style="margin-left:5%" value="Delete" /></td></tr>');
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




function SaveSellInvoice() {


    //listItem.push({ itemID: 1, qty: 2, unitPrice: 250, tax: 0, amount: 500 });
    //listItem.push({ itemID: 2, qty: 2, unitPrice: 500, tax: 0, amount: 1000 });
    if ($('#chk_Sale_customer_IsSalesCredit').prop('checked') == true) { if ($('#dd_sell_Customer').val() * 1 == 1) { $("#btn_sell_Save").attr("disabled", false); toastr.warning("Credit Sales not allowed for Walking Customers"); return; } }
    //if ($('#txt_sell_InvoiceNo').val() == null || $("#txt_sell_InvoiceNo").val().trim().length == 0) { $("#btn_sell_Save").attr("disabled", false); toastr.warning("Reload Again, Invoice no can not be empty"); return; }
    if ($('#dtp_sell_ActivityDate').val() == null || $("#dtp_sell_ActivityDate").val().trim().length == 0) { $("#btn_sell_Save").attr("disabled", false); toastr.warning("Please Select Date"); return; }
    if ($('#dd_sell_sales_person').val() * 1 == 0 || $('#dd_sell_sales_person').val() == null || $("#dd_sell_sales_person").val().trim().length == 0) { $("#btn_sell_Save").attr("disabled", false); toastr.warning("Please Select Sales Person"); return; }
    if ($('#dd_sell_Customer').val() == null || $("#dd_sell_Customer").val().trim().length == 0) { $("#btn_sell_Save").attr("disabled", false); toastr.warning("Please Select Customer"); return; }
    if (ItemNewSalesList == null || ItemNewSalesList.length == 0) { $("#btn_sell_Save").attr("disabled", false); toastr.warning("Please Select Sell Items"); return; }
    var listItem = [];

    for (var i = 0; i < ItemNewSalesList.length; i++) {
        listItem.push({ itemID: ItemNewSalesList[i].id, qty: ItemNewSalesList[i].itemQty, unitPrice: ItemNewSalesList[i].itemUnitPrice, tax: ItemNewSalesList[i].Tax, amount: ItemNewSalesList[i].itemAmount, coaID: ItemNewSalesList[i].coaID, isServiceItem: ItemNewSalesList[i].isServiceItem });
    }

    var obj = {
        invoiceNo: $('#txt_sell_InvoiceNo').val(),
        saleDate: $("#dtp_sell_ActivityDate").val(),
        customerID: $("#dd_sell_Customer").val(),
        items: listItem,
        comments: $('#txt_sell_Comments').val(),
        netAmount: $('#txt_sell_NetAmount').val(),
        // paid: $('#txt_sell_PaidAmount').val() * 1,
        paid: $('#txt_sell_NetAmount').val() * 1,
        IsSalesCredit: $('#chk_Sale_customer_IsSalesCredit').prop('checked'),
        salesPersonId: $("#dd_sell_sales_person").val() * 1,
    };
    ShowAjaxLoader();
    var myurl = editMode == true ? "/Sales/EditSaleInvoice" : "/Sales/SaveSaleInvoice";
    var mydata = new Object();
    mydata.paramSales = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "bad request") { toastr.warning(result); return; }
        $("#btn_sell_Save").attr("disabled", false);
        showNotification("Saved", "success");
        ClearSell();
        $('#AddNewSellModal').modal('hide');
        LoadSellInfo();

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
                    PrintSaleInvoice(result);
                }
            }
        );



    });
}



function LoadSellInfo() {
    oTable = $("#tblSalesInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Sales/GetSalesList",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            {
                "data": 'invoiceNo', "className": 'text-compressed invoiceNo bold ', "bSortable": false, "orderSequence": ["desc", "asc"],
                "mRender": function (data, type, obj) {
                    if (obj["isSalesCredit"] == false) {
                        return '<td class=" text-compressed bold">' + obj.invoiceNo + '</td>'
                    }
                    else {
                        return '<td class="text-compressed bold"><font color="red">' + obj.invoiceNo + '</td>'
                    }
                }
            },
            //{
            //    "data": 'sellDate', "className": 'text-compressed purchaseDate bold', "bSortable": false, "orderSequence": ["desc", "asc"],
            //    "mRender": function (data, type, obj) {
            //        if (obj["sellDate"] == null) {
            //            return '<td class=" text-compressed bold"></td>'
            //        }
            //        else {
            //            return '<td class=" text-compressed bold">' + moment(obj["sellDate"]).format('YYYY-MM-DD') + '</td>'
            //        }
            //    }
            //},

            {
                "data": 'sellDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    if (obj["sellDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                    else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["sellDate"])) + '</td>' }
                }
            },
            { "data": 'customerName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'netAmount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'paid', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'cashPaidDate', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    if (obj["cashPaidDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                    else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["cashPaidDate"])) + '</td>' }
                }
            },
            {
                "data": 'invoiceNo', "className": 'text-compressed invoiceNo bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";

                    html2 += ' <input type="button"  value="EDIT" data-isCstActive = ' + obj.isCustomerActive + ' rel=' + obj.invoiceNo + ' id="btn_sales_Edit" class="form-control hidden" />'
                        + ' <input type="button"  value="Delete" rel=' + obj.invoiceNo + ' id="btn_sales_Delete" class="form-control" />'
                        + ' <input type="button"  value="Print" rel=' + obj.invoiceNo + ' id="btn_sales_Print" class="form-control" />'
                        + ' <input type="button"  value="Download" rel=' + obj.invoiceNo + ' id="btn_sales_Download" class="form-control" />';

                    //html2 += '<input type="button"  value="Delete" rel=' + obj.invoiceNo + ' id="btn_sales_Delete" class="form-control" />';


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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_sell_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_sell_MainEndDate').val()).format("YYYY-MM-DD") });
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



function DeleteSaleInvoice(invoiceId) {
    var myurl = "/Sales/DeleteSaleInvoice";
    var mydata = new Object();
    mydata.invoiceId = invoiceId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "Success") {
            showNotification("Deleted", "success");
            LoadSellInfo();
        }
    });
}

function PrintSaleInvoice(invoiceId) {
    var myurl = "/Report/DownloadSaleInvoice?cv=123&Preview=true&invoiceId=" + invoiceId;
    $("#PrintPreviewIFrame").off("load");
    $("#PrintPreviewIFrame").attr("src", myurl);
    $("#PrintPreviewModal").modal('show');
    $("#PrintPreviewIFrame").on("load", function () {
        this.contentWindow.print();
    });


}
function DownloadSaleInvoice(invoiceId) {
    var myurl = "/Report/DownloadSaleInvoice?";
    var mydata = new Object();
    mydata.invoiceId = invoiceId;
    DownloadFiles(myurl, mydata);

}


function GetItemQuantityforSale(itemId, callback) {
    var myurl = "/Item/GetItemQuantityforSale";
    var mydata = new Object();
    mydata.itemId = itemId;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            if (callback) {
                callback(result * 1);
            }
        }
    });
}

function GetServices() {
    var myurl = "/Item/GetServiceNameList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_sell_item').append('<optgroup label="Services">');
            for (var i = 0; i < result.length; i++) {
                $('.ddl_sell_item ').append('<option data-groupid=2 value="' + result[i].id + '">' + result[i].name + '</option>');
            }
            $('.ddl_sell_item').append('</optgroup>');
            //$('.ddl_sell_item ').val(0);
            ServiceList = result;
        }
    });
}

function GetSellByInvoiceNumber(invoiceNo) {
    var myurl = "/Sales/GetSaleByInvoiceId";
    var mydata = new Object();
    mydata._saleInvoiceId = invoiceNo;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {

            $('#AddNewSellModal').modal('show'); $(".modal-title").text("Edit Sales");
            ClearSell(); ItemNewSalesList = [];
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
                    // $("#txt_sell_NetAmount").val(result[i].AMOUNT);
                    // $("#txt_sell_PaidAmount").val(result[i].PAID);
                    paid = result[i].PAID * 1;
                    // $("#txt_sell_Balance").val(result[i].BALANCE);
                    $("#dd_sell_Customer").val(result[i].cstID);
                    //  $("#dtp_sell_ActivityDate").val(moment(result[i].ACTTIMESTAMP).format('YYYY-MM-DD'));
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
                    //var amount =  (_serviceID == 0)?((result[i].UnitPrice * result[i].Quantity) + result[i].TAX): (result[i].UnitPrice +result[i].TAX);
                    //var _amount = amount;//result[i].AMOUNT; ($('#txt_sell_Unitprice').val() * 1) * ($('#txt_sell_Qty').val() * 1) + $('#txt_sell_tax').val() * 1
                    var _amount = result[i].AMOUNT;
                    var _tax = result[i].TAX;

                    var _isServiceItem = (_serviceID > 0) ? true : false;
                    ItemNewSalesList.push({ listitemid: maxID * 1, id: _itemId, itemCode: _itemCode, itemName: _itemName, itemUnit: _itemUnit, itemQty: _itemQty, itemUnitPrice: _itemUnitPrice, itemAmount: _amount, Tax: _tax, coaID: _serviceID, isServiceItem: _isServiceItem });
                    var max = Math.max.apply(Math, ItemNewSalesList.map(function (o) { return o.listitemid; }));
                    maxID = max + 1;
                    BindItemtable(ItemNewSalesList, false);
                }
            }
            $("#txt_sell_PaidAmount").val(paid);
            $("#txt_sell_Balance").val($("#txt_sell_NetAmount").val() * 1 - paid);
            editMode = true;
        }
    });
}



function SaveService(name) {
    var myurl = "/COA/SaveRevenueSales";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.name = name;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            //$("#txt_pvoucher_AdministrativeExpense").val("");
            //$('#AddNewAdministrativeModal').modal('hide');
            // GetServices();
        }
        if (result == "Exists") {
            toastr.warning("Expense Already Exists");

        }
    });
}

$('#dd_sell_sales_person').change(function () {
    if ($(this).val() !== "0") {
        $('#dd_sell_Customer').html('');
        GetCustomerListBySalesPersonID($(this).val());
        $('#dd_sell_Customer').val("select");
    }
    else {
        $('#dd_sell_Customer').html('');
        //GetCustomersData();
        $('#dd_sell_Customer').val("select");
    }
});


function GetCustomerListBySalesPersonID(salesPersonID) {
    var myurl = "/Customer/GetCustomerListBySalesPersonID";
    var mydata = new Object();
    mydata.salesPersonID = salesPersonID;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('#dd_sell_Customer').html('');
            for (var i = 0; i < result.length; i++) {
                $('#dd_sell_Customer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            //$('#dd_sell_Customer').val("select");
            //customerlist = result;
        }
    });
}


function GetSalesPersonWithNewCustomerData() {
    var myurl = "/Sales/GetSalesPersonList";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.dd_sell_new_customer_sales_person').append(cloneDom);
        }
        //$('.dd_sell_sales_person').chosen();
        //$(".chosen-select").chosen({ width: '100%' });
    });
}


$('#txt_customer_cnic').on('keydown', function (e) {
    var key = e.keyCode | e.which;
    if (key >= 48 && key <= 57 || key === 45 || key === 189 || key === 8 || key === 46) {
        if (key !== 8 && key !== 46) {
            var cnic = $(this).val().length; // get character length
            switch (cnic) {
                case 5:
                    var cnicVal = $(this).val();
                    var cnicNewVal = cnicVal + '-';
                    $(this).val(cnicNewVal);
                    break;
                case 13:
                    var cnicVal = $(this).val();
                    var cnicNewVal = cnicVal + '-';
                    $(this).val(cnicNewVal);
                    break;
                default:
                    break;
            }
        } return true;
    } else return false;
});
