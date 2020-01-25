

var ItemList = new Array();
var ItemNewSalesList = new Array();

function SalesManagment()
{
    GetItemList(); GetCustomers(); GetMainCustomers();
    //ItemList.push({ id: 1, itemCode: "101A", itemName: "Item1", itemUnit: "Piece", itemQty: 1, itemUnitPrice: 20, itemTaxAmount: 10 });
    //ItemList.push({ id: 2, itemCode: "102A", itemName: "Item2", itemUnit: "Piece", itemQty: 1, itemUnitPrice: 30, itemTaxAmount: 15 });
    //ItemList.push({ id: 3, itemCode: "103A", itemName: "Item3", itemUnit: "Piece", itemQty: 1, itemUnitPrice: 40, itemTaxAmount: 20 });
    //ItemList.push({ id: 4, itemCode: "104A", itemName: "Item4", itemUnit: "Piece", itemQty: 1, itemUnitPrice: 50, itemTaxAmount: 25 });
    //ItemList.push({ id: 5, itemCode: "105A", itemName: "Item5", itemUnit: "Piece", itemQty: 1, itemUnitPrice: 60, itemTaxAmount: 30 });
    //ItemList.push({ id: 6, itemCode: "106A", itemName: "Item6", itemUnit: "Piece", itemQty: 1, itemUnitPrice: 70, itemTaxAmount: 35 });

    BindItemList(ItemList);
   


   

    $("#btn_sell_Save").on("click", function () {

        Clear();
        showNotification("Saved", "success");
        
    });



    $(document).on('click', '.del', function () {


        var id = $(this).attr('rel') * 1;
        ItemNewSalesList = $.grep(ItemNewSalesList, function (data, index) {
            return data.listitemid != id
        });
        BindItemtable(ItemNewSalesList);

    });

    var maxID = 1;

    $(".AddSales").on("click", function () {

        AddSalesIntoList();
    });


    $(document).on('change', '.code', function () {

        //var value = $(this).val().toUpperCase();
        //var itemlistID = ItemList.filter(x => x.itemCode === value).map(x=>x.id);
        //$('.item').val(itemlistID);
        //var itemlistunitPrice = ItemList.filter(x => x.itemCode === value).map(x=>x.itemUnitPrice);
        //$('.price').val(itemlistunitPrice);
        //var itemlistqty = ItemList.filter(x => x.itemCode === value).map(x=>x.itemQty);
        //$('.qty').val(itemlistqty);
        //var itemlistunit = ItemList.filter(x => x.itemCode === value).map(x=>x.itemUnit);
        //$('.unit').val(itemlistunit);
        //var itemlistTax = ItemList.filter(x => x.itemCode === value).map(x=>x.itemTaxAmount);
        //$('.Tax').val(itemlistTax);
        //var Amount = (itemlistunitPrice * 1) * (itemlistqty * 1);
        //Amount = Amount + ((((itemlistTax) * 1) / 100) * Amount);
        //$('.Amount').val(Amount);
        var value = $(this).val().toUpperCase();
        var selectedItem = itemlist.find(x=>x.itemCode == value);

        if (selectedItem != "undefined") {

            if ($("#txt_purchase_Qty").val() * 1 == 0) { $("#txt_sell_Qty").val(1); }
            $("#txt_sell_item").val(selectedItem.id);
            $("#txt_sell_Unit").val(selectedItem.itemunit);
            $("#txt_sell_Unitprice").val(selectedItem.itemPurchasePrice);
            $("#txt_sell_tax").val(selectedItem.itemtax);
            var Amount = ($("#txt_sell_Unitprice").val() * 1) * ($("#txt_sell_Qty").val() * 1);
            $('.Amount').val(Amount);
        }


    });

    $(document).on('change', '.item', function () {

        //var value = $(this).val();
        //var itemlistName = ItemList.filter(x => x.id == value).map(x=>x.itemCode);
        //$('.code').val(itemlistName);
        //var itemlistunitPrice = ItemList.filter(x => x.id == value).map(x=>x.itemUnitPrice);
        //$('.price').val(itemlistunitPrice);
        //var itemlistqty = ItemList.filter(x => x.id == value).map(x=>x.itemQty);
        //$('.qty').val(itemlistqty);
        //var itemlistunit = ItemList.filter(x => x.id == value).map(x=>x.itemUnit);
        //$('.unit').val(itemlistunit);
        //var itemlistTax = ItemList.filter(x => x.id == value).map(x=>x.itemTaxAmount);
        //$('.Tax').val(itemlistTax);
        //var Amount = (itemlistunitPrice * 1) * (itemlistqty * 1);
        //Amount = Amount + ((((itemlistTax) * 1) / 100) * Amount);
        //$('.Amount').val(Amount);
        var value = $(this).val() * 1;
        if (value > 0) {
            var selectedItem = itemlist.find(x=>x.id == value);
            if ($("#txt_sell_Qty").val() * 1 == 0) { $("#txt_sell_Qty").val(1); }
            $("#txt_sell_Code").val(selectedItem.itemCode);
            $("#txt_sell_Unit").val(selectedItem.itemunit);
            $("#txt_sell_Unitprice").val(selectedItem.itemPurchasePrice);
            $("#txt_sell_tax").val(selectedItem.itemtax);
            var Amount = ($("#txt_sell_Unitprice").val() * 1) * ($("#txt_sell_Qty").val() * 1);
            $('#txt_sell_Amount').val(Amount);
        }

    });

    $(document).on('change', '.Tax', function () {

        //var unitprice = $('.price').val() * 1;
        //var itemqty = $('.qty').val() * 1;
        //var Amount = (unitprice * 1) * (itemqty * 1);;
        //var tax = $('.Tax').val() * 1;
        //var amount = Amount + ((((tax) * 1) / 100) * tax);
        //$('#txt_sell_Amount').val(amount);


    });

    $(document).on('change', '.price', function () {

        //var qty = $('.qty').val() * 1;
        //var unitPrice = $('.price').val() * 1;
        //var amount = qty * unitPrice;
        //$('.Amount').val(amount);


    });

    $(document).on('change', '.paid', function () {

        var netAmount = $('.NetAmount').val() * 1;
        var paidamount = $('.paid').val() * 1;
        var amount = netAmount - paidamount;
        $('.balance').val(amount);


    });

    $(document).on('change', '.qty', function () {

        var qty = $('.qty').val() * 1;
        var unitPrice = $('.price').val() * 1;
        var amount = qty * unitPrice;
        $('#txt_sell_Amount').val(amount);


    });
}

function AddSalesintoList()
{
    //var value = $('.code').val().toUpperCase();
    //var ItemID = ItemList.filter(x => x.itemCode === value).map(x=>x.id);
    //var ItemName = ItemList.filter(x => x.itemCode === value).map(x=>x.itemName);
    //var itemUnit = ItemList.filter(x => x.itemCode === value).map(x=>x.itemUnit);
    if ($("#txt_sell_item").val() * 1 > 0) {
        var ItemCode = $("#txt_sell_Code").val();
        var ItemID = $("#txt_sell_item").val();
        var ItemName = itemlist.find(x=>x.id == ItemID * 1).itemName;
        var itemUnit = $("#txt_sell_Unit").val();
        var itemQty = $("#txt_sell_Qty").val() * 1;
        var itemUnitPrice = $('#txt_sell_Unitprice').val() * 1;
        var Amount = $('#txt_sell_Amount').val() * 1;
        var Tax = $('#txt_sell_tax').val() * 1;

        ItemNewSalesList.push({ listitemid: maxID * 1, id: ItemID * 1, itemCode: ItemCode, itemName: ItemName, itemUnit: itemUnit, itemQty: itemQty, itemUnitPrice: itemUnitPrice, itemAmount: Amount, Tax: Tax });
        var max = Math.max.apply(Math, ItemNewSalesList.map(function (o) { return o.listitemid; }));
        maxID = max + 1;
        BindItemtable(ItemNewSalesList);
        ClearSellitem();
    }
}

function GetItemList() {
    var myurl = "/Item/GetItemList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_sell_item').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_sell_item').append('<option value="' + result[i].id + '">' + result[i].itemName + '</option>');
            }
            itemlist = result;
        }
    });
}


function GetCustomers() {
    var myurl = "/Customer/GetCustomerList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.dd_sell_Customer').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.dd_sell_Customer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }

        }
    });
}

function GetMainCustomers() {
    var myurl = "/Customer/GetCustomerList";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_sell_MainCustomer').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_sell_MainCustomer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
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
    $('.tblCurrentSales').empty();
    for (var i = 0; i < ItemNewSalesList.length; i++) {
        $(".tblCurrentSales").append('<tr><td>' + ItemNewSalesList[i].itemCode + '</td><td>'
            + ItemNewSalesList[i].itemName + '</td><td>' + ItemNewSalesList[i].itemUnit
            + '</td><td>' + ItemNewSalesList[i].itemQty + '</td><td>'
            + ItemNewSalesList[i].itemUnitPrice + '</td><td>' + ItemNewSalesList[i].Tax + '</td><td>' + ItemNewSalesList[i].itemAmount
            + '</td><td class="text-center"><input type="button" class="btn btn-success" value="Edit" />'
            +'<input type="button" class="btn btn-danger del" rel=' + ItemNewSalesList[i].listitemid + ' style="margin-left:5%" value="Delete" /></td></tr>');
        Netamount = Netamount + ItemNewSalesList[i].itemAmount;

    }
    //$('#tblSalescurrentInformation').DataTable();
    $('#txt_sell_NetAmount').val(Netamount);
    $('#txt_sell_Balance').val(Netamount);
    $('#txt_sell_PaidAmount').val(0);
}

function ClearSellitem() {
    $('#txt_sell_Code').val("");
    $('#txt_sell_item').val(0);
    $('#txt_sell_Unit').val("");
    $('#txt_sell_Qty').val("");
    $('#txt_sell_Unitprice').val("");
    $('#txt_sell_Amount').val("");
    $('#txt_sell_tax').val("");
}
function ClearPurchase() {
    ClearPuchaseitem();
    ItemPurchaseList = null;
    $("#txt_sell_NetAmount").val("");
    $("#txt_sell_PaidAmount").val("");
    $("#txt_sell_Balance").val("");
    $("#dtp_sell_ActivityDate").val("");
    $("#dd_sell_Vendor").val(0);
    $('.tblCurrentSales').empty();

}