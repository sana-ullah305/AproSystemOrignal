

var ItemList = new Array();
var ItemPurchaseList = new Array();
var Vendorlist = new Array();
var maxID = 1;
var editMode = false;

function PurchaseInformation() {
    $("#dtp_purchase_MainStartDate").val(moment().subtract('days', 7).format('YYYY-MM-DD'));

    $("#dtp_purchase_MainEndDate").val(moment().format('YYYY-MM-DD'));
    GetItemList();// GetVendors(); 
    GetMainVendors();

    LoadPurchaseInfo();
    $("#btn_purchase_Search").on("click", function () {
        if ($("#dtp_purchase_MainStartDate").val() != "" && $("#dtp_purchase_MainEndDate").val() != "") {
            LoadPurchaseInfo();
        }
        else { toastr.warning("Please Select Dates"); }


    });

    $("#btn_purchase_AddNewPurchase").on("click", function () {
        setPurchaseForEdit();
        ClearPurchase();
        GetPurchaseInvoiceNumber();
        GetVendors();
        //SetVendor();
    });

    $("#btn_purchase_Save").on("click", function () {
        // $("#btn_purchase_Save").attr("disabled", true);
        SavePurchaseInvoice();

    });



    $(document).on('click', '.btn_purchaseMain_Delete', function () {

        var invoiceId = $(this).attr('rel');
        if (confirm("Are you sure to Delete invoiceId= " + invoiceId)) {
            DeletePurchaseInvoice(invoiceId);
        }


    });
    $(document).on('click', '.btn_purchaseMain_Edit', function () {
        var invoiceId = $(this).attr('rel');
        GetPurchaseByInvoiceNumber(invoiceId,"Edit");
        //setPurchaseForEdit();
    });

    $(document).on('click', '.btn_purchaseMain_View', function () {
        var invoiceId = $(this).attr('rel');
        GetPurchaseByInvoiceNumber(invoiceId, "View");
        //just to save the list so we can enable fields later
        //var list = [];
        //$('#.modal-body').find(':input:visible:not([readonly][disabled]),button').each(function () {
        //    list.push('#' + this.id);
        //});

        //$(list.join(',')).attr('readonly', true);
    });


    $(document).on('click', '.deleteItem', function () {


        var id = $(this).attr('rel') * 1;
        ItemPurchaseList = $.grep(ItemPurchaseList, function (data, index) {
            return data.listitemid != id
        });
        BindItemtable(ItemPurchaseList);

    });

    $(document).on('click', '.editItem', function () {


        var id = $(this).attr('rel') * 1;

        var selectedItem = ItemPurchaseList.find(x => x.id == id);

        if (selectedItem != "undefined") {

            //if ($("#txt_purchase_Qty").val() * 1 == 0) { $("#txt_purchase_Qty").val(1); }
            $("#txt_purchase_Code").val(selectedItem.itemCode);
            $("#txt_purchase_item").val(selectedItem.id);
            $("#txt_purchase_Unit").val(selectedItem.itemUnit);
            $("#txt_purchase_Qty").val(selectedItem.itemQty);
            $("#txt_purchase_Unitprice").val(selectedItem.itemUnitPrice);
            $("#txt_purchase_Amount").val(selectedItem.itemAmount);

        };
        //  BindItemtable(ItemNewSalesList);

    });


    $(".AddPurchase").on("click", function () {
        AddPurchaseintoList();

    });



    $(document).on('change', '.code', function () {

        var value = $(this).val().toUpperCase();
        var selectedItem = itemlist.find(x => x.itemCode.toUpperCase() == value);
        if (selectedItem != null) {
            SetItemControls(selectedItem);
        }
        else { ClearPurchaseitem(); }

    });

    $(document).on('change', '.item', function () {
        var value = $(this).val() * 1;
        if (value > 0) {
            var selectedItem = itemlist.find(x => x.id == value);
            SetItemControls(selectedItem);
        }
        else { ClearPurchaseitem(); }
    });

    $(document).on('change', '#txt_purchase_Unitprice', function () {
        var amount = ($('#txt_purchase_Qty').val() * 1) * ($('#txt_purchase_Unitprice').val() * 1);
        $('.Amount').val(amount);
    });

    $(document).on('change', '#txt_purchase_Qty', function () {
        var amount = ($('#txt_purchase_Qty').val() * 1) * ($('#txt_purchase_Unitprice').val() * 1);
        $('.Amount').val(amount);
    });

    $(document).on('change', '.paid', function () {

        var netAmount = $('#txt_purchase_NetAmount').val() * 1;
        var paidamount = $('#txt_purchase_PaidAmount').val() * 1;
        var amount = netAmount - paidamount;
        $('#txt_purchase_Balance').val(amount);


    });

    $(document).on('click', '.close', function () {
        ClearPurchase();
    });


}

function SetItemControls(selectedItem) {
    if ($("#txt_purchase_Qty").val() * 1 == 0) { $("#txt_purchase_Qty").val(1); }
    $("#txt_purchase_item").val(selectedItem.id);
    $("#txt_purchase_Code").val(selectedItem.itemCode);
    $("#txt_purchase_Unit").val(selectedItem.unit);
    $("#txt_purchase_Unitprice").val(selectedItem.purchasePrice);
    var Amount = ($("#txt_purchase_Unitprice").val() * 1) * ($("#txt_purchase_Qty").val() * 1);
    $('.Amount').val(Amount);
}

function AddPurchaseintoList() {

    if ($("#txt_purchase_item").val() * 1 == 0) { toastr.warning("Please Select Item"); return; }
    if ($("#txt_purchase_Qty").val() * 1 == 0) { toastr.warning("Please Enter Quantity"); return; }
    if ($("#txt_purchase_Unitprice").val() * 1 == 0) { toastr.warning("Please Enter Unit Price"); return; }
    if ($.grep(ItemPurchaseList, function (rx) { return rx.id == $("#txt_purchase_item").val() }).length > 0) { toastr.warning("Item Already in the list"); return; }
    if ($("#txt_purchase_item").val() * 1 > 0) {
        var ItemCode = $("#txt_purchase_Code").val();
        var ItemID = $("#txt_purchase_item").val();
        var ItemName = itemlist.find(x => x.id == ItemID * 1).name;
        var itemUnit = $("#txt_purchase_Unit").val();
        var itemQty = $("#txt_purchase_Qty").val() * 1;
        var itemUnitPrice = $('#txt_purchase_Unitprice').val() * 1;
        var Amount = $('#txt_purchase_Amount').val() * 1;

        ItemPurchaseList.push({ listitemid: maxID * 1, id: ItemID, itemCode: ItemCode, itemName: ItemName, itemUnit: itemUnit, itemQty: itemQty, itemUnitPrice: itemUnitPrice, itemAmount: Amount });
        var max = Math.max.apply(Math, ItemPurchaseList.map(function (o) { return o.listitemid; }));
        maxID = max + 1;
        BindItemtable(ItemPurchaseList);
        ClearPurchaseitem();
    }
}

function GetPurchaseInvoiceNumber() {
    //var myurl = "/Purchase/GetNextVoucher";
    //var mydata = new Object();

    //XHRPOSTRequest(myurl, mydata, function (result) {
    //    if (result.length !== 0) {

    $('#AddNewPurchaseModal').modal('show');
    $("modal-title").text("Add New Purchase");
    $("#txt_purchase_InvoiceNo").val('');
    //$("#dtp_purchase_ActivityDate").val(moment().format('YYYY-MM-DD'));
    //$('#dtp_purchase_ActivityDate').val(FormatDateTimeToDisplay(GetJSDate(result.searchFeePayDate)));
    $('#dtp_purchase_ActivityDate').val(FormatDateTimeToDisplay(new Date()));
    //    }
    //});
}

function GetItemList() {
    var myurl = "/Item/GetItemList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_purchase_item').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_purchase_item').append('<option value="' + result[i].id + '">' + result[i].name + '</option>');
            }
            itemlist = result;
        }
    });
}


function SetVendor() {
    if (Vendorlist.length !== 0) {
        $(".ddl_purchase_Vendor").empty();
        for (var i = 0; i < Vendorlist.length; i++) {
            $('.ddl_purchase_Vendor').append('<option value="' + Vendorlist[i].Id + '">' + Vendorlist[i].Name + '</option>');
        }
        $('.ddl_purchase_Vendor').val(0);
    }
}
function GetVendors() {
    var myurl = "/Vendor/GetVendorList";
    var mydata = new Object();
    Vendorlist = [];
    $('.ddl_purchase_Vendor').empty();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_purchase_Vendor').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_purchase_Vendor').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            $('.ddl_purchase_Vendor').val(0);
            Vendorlist = result;
        }
    });
}

function GetMainVendors() {
    var myurl = "/Vendor/GetVendorList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_purchase_MainVendor').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_purchase_MainVendor').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }

        }
    });
}


//function BindItemList(ItemList) {
//    for (var i = 0; i < ItemList.length; i++) {
//        $(".item").append('<option value=' + ItemList[i].id + '>' + ItemList[i].itemName + '</option>');
//    }
//}

function BindItemtable() {
    var Netamount = 0;
    $('.tblCurrentPurchase').empty();
    for (var i = 0; i < ItemPurchaseList.length; i++) {
        $(".tblCurrentPurchase").append('<tr><td>' + ItemPurchaseList[i].itemCode + '</td><td>'
            + ItemPurchaseList[i].itemName + '</td><td>' + ItemPurchaseList[i].itemUnit
            + '</td><td class="NumbersAlign">' + ItemPurchaseList[i].itemQty
            + '</td><td class="NumbersAlign">' + CurrencyFormat(ItemPurchaseList[i].itemUnitPrice)
            + '</td><td class=NumbersAlign>' + CurrencyFormat(ItemPurchaseList[i].itemAmount)
            + '</td><td class="text-center">'//<input type="button" class="btn btn-success editItem" rel=' + ItemPurchaseList[i].listitemid + ' value="Edit" />'
            + '<input type="button" class="btn btn-danger deleteItem" rel=' + ItemPurchaseList[i].listitemid
            + ' style="margin-left:5%" value="Delete" /></td></tr>');
        Netamount = Netamount + ItemPurchaseList[i].itemAmount;

    }
    $('#txt_purchase_NetAmount').val(Netamount);
    $('#txt_purchase_Balance').val((Netamount * 1) - ($('#txt_purchase_PaidAmount').val() * 1));

    //$('#txt_purchase_NetAmount').val(Netamount);
    //$('#txt_purchase_Balance').val(Netamount);
    //$('#txt_purchase_PaidAmount').val(0);
}
function ClearPurchaseitem() {
    $('#txt_purchase_Code').val("");
    $('#txt_purchase_item').val(0);
    $('#txt_purchase_Unit').val("");
    $('#txt_purchase_Qty').val("");
    $('#txt_purchase_Unitprice').val("");
    $('#txt_purchase_Amount').val("");
}
function ClearPurchase() {
    ClearPurchaseitem();
    ItemPurchaseList = [];
    $("#txt_purchase_NetAmount").val("");
    $("#txt_purchase_PaidAmount").val("");
    $("#txt_purchase_Balance").val("");
    $("#dtp_purchase_ActivityDate").val("");
    $("#dd_purchase_Vendor").val(0);
    $('.tblCurrentPurchase').empty();
    $("#txt_purchase_Comments").val("");
    editMode = false;
    $("#btn_purchase_Save").attr("disabled", false);

}

//function Clear() {
//    $('.tblCurrentSales').empty();
//    $('.NetAmount').val(" ");
//    $('.balance').val(" ");
//    $('.paid').val(" ");
//    $('.qty').val(" ");
//    $('.price').val(" ");
//    $('.Amount').val(" ");
//    $('.code').val(" ");
//    $('.item').val(0);
//    $('.unit').val(" ");
//    ItemNewSalesList = [];
//}



function SavePurchaseInvoice() {


    //listItem.push({ itemID: 1, qty: 2, unitPrice: 250, tax: 0, amount: 500 });
    //listItem.push({ itemID: 2, qty: 2, unitPrice: 500, tax: 0, amount: 1000 });

    //if ($('#txt_purchase_InvoiceNo').val() == null || $("#txt_purchase_InvoiceNo").val().trim().length == 0) { $("#btn_purchase_Save").attr("disabled", false); toastr.warning("Reload Again, Invoice no can not be empty"); return; }
    if ($('#dtp_purchase_ActivityDate').val() == null || $("#dtp_purchase_ActivityDate").val().trim().length == 0) { $("#btn_purchase_Save").attr("disabled", false); toastr.warning("Please Select Date"); return; }
    if ($('#dd_purchase_Vendor').val() == null || $("#dd_purchase_Vendor").val().trim().length == 0) { $("#btn_purchase_Save").attr("disabled", false); toastr.warning("Please Select Vendor"); return; }
    if (ItemPurchaseList == null || ItemPurchaseList.length == 0) { $("#btn_purchase_Save").attr("disabled", false); toastr.warning("Please Purchase Items"); return; }
    var listItem = [];

    for (var i = 0; i < ItemPurchaseList.length; i++) {
        listItem.push({ itemID: ItemPurchaseList[i].id, qty: ItemPurchaseList[i].itemQty, unitPrice: ItemPurchaseList[i].itemUnitPrice, tax: 0, amount: ItemPurchaseList[i].itemAmount });
    }

    var obj = {
        invoiceNo: $('#txt_purchase_InvoiceNo').val(),
        purchaseDate: $("#dtp_purchase_ActivityDate").val(),
        vendorID: $("#dd_purchase_Vendor").val(),
        items: listItem,
        comments: $('#txt_purchase_Comments').val(),
        netAmount: $('#txt_purchase_NetAmount').val(),
        paid: $('#txt_purchase_PaidAmount').val() * 1
    };
    var myurl = editMode == true ? "/Purchase/EditPurchaseInvoice" : "/Purchase/SavePurchaseInvoice";
    var mydata = new Object();
    mydata.purchaseInvoice = JSON.stringify(obj);
    ShowAjaxLoader();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            HideAjaxLoader();
            $("#btn_purchase_Save").attr("disabled", false);
            showNotification("Saved", "success");
            ClearPurchase();
            $('#AddNewPurchaseModal').modal('hide');
            LoadPurchaseInfo();
            // ClearCallLogs();

        }
        else { toastr.warning(result); return; }
        HideAjaxLoader();
    });
}



function LoadPurchaseInfo() {
    oTable = $("#tblPurchaseInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Purchase/GetPurchasesList",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "aoColumns": [
            { "data": 'invoiceNo', "className": 'text-compressed invoiceNo bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{
            //    "data": 'purchaseDate', "className": 'text-compressed purchaseDate bold', "bSortable": false, "orderSequence": ["desc", "asc"],
            //    "mRender": function (data, type, obj) {
            //        if (obj["purchaseDate"] == null) {
            //            return '<td class=" text-compressed bold"></td>'
            //        }
            //        else {
            //            return '<td class=" text-compressed bold">' + moment(obj["purchaseDate"]).format('YYYY-MM-DD') + '</td>'
            //        }
            //    }
            //},
             {
                 "data": 'purchaseDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                     if (obj["purchaseDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                     else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["purchaseDate"])) + '</td>' }
                 }
             },
            { "data": 'vendorName', "className": 'text-compressed vendorName bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'netAmount', "className": 'text-compressed NumbersAlign bold', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'paid', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'balance', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'invoiceNo', "className": 'text-compressed invoiceNo bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    if (obj["isDeletable"] == true) {
                        html2 += ' <input type="button"  value="EDIT" rel=' + obj.invoiceNo + ' id="btn_purchaseMain_Edit" class="form-control btn_purchaseMain_Edit" />'
                            + ' <input type="button"  value="Delete" rel=' + obj.invoiceNo + ' id="btn_purchaseMain_Delete" class="form-control btn_purchaseMain_Delete" />'
                        + ' <input type="button"  value="View" rel=' + obj.invoiceNo + ' id="btn_purchaseMain_View" class="form-control btn_purchaseMain_View" />';
                    }
                    else {
                        html2 += ' <input type="button"  value="View" rel=' + obj.invoiceNo + ' id="btn_purchaseMain_View" class="form-control btn_purchaseMain_View" />'
                    }

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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_purchase_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_purchase_MainEndDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "VendorId", "value": $('#ddl_purchase_MainVendor').val() });

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

function DeletePurchaseInvoice(purchaseInvoiceId) {
    var myurl = "/Purchase/DeletePurchaseInvoice";
    var mydata = new Object();
    mydata.invoiceId = purchaseInvoiceId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Deleted", "success");
            LoadPurchaseInfo();
        }
    });
}

function GetPurchaseByInvoiceNumber(invoiceNo, action) {
    var myurl = "/Purchase/GetPurchaseByInvoiceId";
    var mydata = new Object();
    mydata._purchaseInvoiceId = invoiceNo;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {

            $('#AddNewPurchaseModal').modal('show');
            $(".modal-title").text("Edit Purchase");
            ClearPurchase(); ItemPurchaseList = [];

            SetVendor(); var paid = 0;
            // $("#dtp_purchase_ActivityDate").val(moment(result.activitydate).format('YYYY-MM-DD'));
            for (i = 0; i < result.length; i++) {
                if (result[i].TYPED == "TOTALS") {
                    $("#txt_purchase_InvoiceNo").val(result[i].InvoiceNo);
                    // $("#txt_sell_NetAmount").val(result[i].AMOUNT);
                    // $("#txt_sell_PaidAmount").val(result[i].PAID);
                    paid = result[i].PAID * 1;
                    // $("#txt_sell_Balance").val(result[i].BALANCE);
                    $(".ddl_purchase_Vendor").val(result[i].vendID);
                    //$("#dtp_purchase_ActivityDate").val(moment(result[i].ACTTIMESTAMP).format('YYYY-MM-DD'));

                    if (result[i].ACTTIMESTAMP != null) { $('#dtp_purchase_ActivityDate').val(FormatDateTimeToDisplay(GetJSDate(result[i].ACTTIMESTAMP))); }

                    $("#txt_purchase_Comments").val(result[i].COMMENTS);
                }
                else {
                    // var _serviceID = result[i].CoaId;
                    var _itemCode = result[i].ItemCode;
                    var _itemId = result[i].ItemId;
                    var _itemName = result[i].Name;
                    var _itemUnit = result[i].Unit;
                    var _itemQty = result[i].Quantity;
                    var _itemUnitPrice = result[i].UnitPrice;
                    //var amount =  (_serviceID == 0)?((result[i].UnitPrice * result[i].Quantity) + result[i].TAX): (result[i].UnitPrice +result[i].TAX);
                    //var _amount = amount;//result[i].AMOUNT; ($('#txt_sell_Unitprice').val() * 1) * ($('#txt_sell_Qty').val() * 1) + $('#txt_sell_tax').val() * 1
                    var _amount = result[i].AMOUNT;
                    //  var _tax = result[i].TAX;

                    //var _isServiceItem = (_serviceID > 0) ? true : false;
                    ItemPurchaseList.push({ listitemid: maxID * 1, id: _itemId, itemCode: _itemCode, itemName: _itemName, itemUnit: _itemUnit, itemQty: _itemQty, itemUnitPrice: _itemUnitPrice, itemAmount: _amount });
                    var max = Math.max.apply(Math, ItemPurchaseList.map(function (o) { return o.listitemid; }));
                    maxID = max + 1;
                    BindItemtable(ItemPurchaseList);
                }
            }
            $("#txt_purchase_PaidAmount").val(paid);
            $("#txt_purchase_Balance").val($("#txt_purchase_NetAmount").val() * 1 - paid);
            editMode = true;
        }
        action === "View" ? setPurchaseForView() : setPurchaseForEdit()

    });
}



function setPurchaseForView() {
    $(".modal-title").text("View Purchase");
    $('.modal-body  *').prop('disabled', true);
    $('.deleteItem').prop('disabled', true);
    $('#btn_purchase_Save').prop('disabled', true);
}
function setPurchaseForEdit() {
    $('.modal-body  *').prop('disabled', false);
    $('.deleteItem').prop('disabled', false);
    $('#btn_purchase_Save').prop('disabled', false);
}