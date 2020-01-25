function ItemTrigger() {
    LoadItemInfo();
    GetOilGradeData();
    $("#btn_item_AddNewItem").on("click", function () {
        Clearitem();
        $('#AddNewItemModal').modal('show'); $(".modal-title").text('Add New Item');
        $("#ddl_item_unit").val("Piece");
    });

    $("#btn_item_Save").on("click", function () {
        SaveItem();

    });

    $("#AdjustQtyModal #btnitemSave").on("click", function () {
        var unitPrice = $("#AdjustQtyModal #txtUnitPrice").val() * 1;
        var account = $("#AdjustQtyModal #ddlAccount").val() * 1;
        var currQty = $("#AdjustQtyModal #txtStock").val() * 1;
        var adjustQty = $("#AdjustQtyModal #txtAdjust").val() * 1;
        if (account == 0) { alert("Please select account!"); return; }
        if (unitPrice == 0) { alert("UnitPrice canot be empty"); return; }
        if (currQty == adjustQty) { alert("Adjust quantity cannot be identical"); return; }
        var itemID = $("#AdjustQtyModal #lblItemID").html() * 1;
        var diff = Math.abs(currQty - adjustQty);
        var netAmount = diff * unitPrice;

        var param = { coaID: account, netAmount: netAmount, items: [{ itemID: itemID, qty: diff, unitPrice: unitPrice, amount: netAmount }], comments: '' }
        if (currQty < adjustQty) { StockIn(param); }
        if (currQty > adjustQty) { StockOut(param); }

        $("#AdjustQtyModal").modal('hide');
    });


    $(document).on('click', '.close', function () {
        Clearitem();
    });

    $(document).on('click', '#btn_item_Delete', function () {

        var id = $(this).attr('rel') * 1;

        if (id > 0) {
            if (confirm("Are you sure to Delete ?")) {
                DeleteItem(id);
            }
        }
    });

    $(document).on('click', '#btnAdjStock', function () {
        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            var curr = $(this).parent().parent();
            var itemName = $('.itemNameTD', curr).text();
            var stock = $('.itemStockTD', curr).text();
            var purchase = $('.itemPurchase', curr).text();
            $("#AdjustQtyModal #txtItem").val(itemName);
            $("#AdjustQtyModal #txtStock").val(stock);
            $("#AdjustQtyModal #lblItemID").text(id);
            $("#AdjustQtyModal #ddlAccount").val(132);
            $("#AdjustQtyModal #txtUnitPrice").val(purchase)
            $("#AdjustQtyModal").modal('show');
            $("#txtAdjust").val("");
        }
    })

    $(document).on('click', '#btn_item_Edit', function () {

        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            GetItemByID(id);
        }
    });

    $(document).on('click', '#btn_item_Search', function () {
        LoadItemInfo();
    });

    $(document).on('click', '#btn_item_downLoadExcel', function () {
        downLoadItemInventoryInExcel();
    });

    GetActingAccounts();
}

function downLoadItemInventoryInExcel()
{
    ShowAjaxLoader();
    var ajaxSource = "/Item/DownloadExcel";
    var myurl = ajaxSource + "?";
    ShowAjaxLoader();
    DownloadFiles(myurl, null, function (result) {
        HideAjaxLoader();
    });
}

function StockIn(param) {
    var myurl = "/StockIn/SaveStockIn";
    var mydata = new Object();
    var objx = param;
    mydata.stockIn = JSON.stringify(objx);
    ShowAjaxLoader();
    XHRPOSTRequest(myurl, mydata, function (result) {
        HideAjaxLoader();
        LoadItemInfo();
    });
}

function StockOut(param) {
    var myurl = "/StockOut/SaveStockOut";
    var mydata = new Object();
    var objx = param;
    mydata.stockOut = JSON.stringify(objx);
    ShowAjaxLoader();
    XHRPOSTRequest(myurl, mydata, function (result) {
        HideAjaxLoader();
        LoadItemInfo();

    });
}

function GetSellInvoiceNumber() {
    var myurl = "/Sales/GetNextVoucher";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {

            $('#AddNewSellModal').modal('show');
            $("#txt_sell_InvoiceNo").val(result);
        }
    });
}

function GetActingAccounts() {
    var myurl = "/Item/GetActingAccounts";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $("#AdjustQtyModal #ddlAccount").html('');
            // $("#AdjustQtyModal #ddlAccount").append('<option value="0" disabled>--Select--</option>');
            for (var i = 0; i < result.length; i++) {
                $("#AdjustQtyModal #ddlAccount").append('<option value="' + result[i].Key + '">' + result[i].Value + '</option>');
            }
            $("#AdjustQtyModal #ddlAccount").val(0);
        }
    });
}



function Clearitem() {
    $('#lbl_item_id').val("");
    $('#txt_item_Name').val("");
    $('#txt_item_Code').val("");
    $('#ddl_item_unit').val(0);
    $('#txt_item_MinQty').val("");
    $('#txt_item_PurchaseRate').val("");
    $('#txt_item_SellRate').val("");
    $('#txt_item_Description').val("");
    $('#txt_item_Search').val("");
    // $('#txt_item_tax').val("");
    $("#btn_item_Save").attr("disabled", false);
    $('#chk_item_isTaxable').prop('checked', false);

    $('#txt_oil_ltr').val('');
    $('#qty_in_carton').val('');
    $('#ddl_oilgrade').val('0');

}



function SaveItem() {
    if ($('#txt_item_Name').val() == null || $("#txt_item_Name").val().trim().length == 0) { $("#btn_item_Save").attr("disabled", false); toastr.warning("Please Enter Item Name"); return; }
    if ($('#txt_item_Code').val() == null || $("#txt_item_Code").val().trim().length == 0) { $("#btn_item_Save").attr("disabled", false); toastr.warning("Please Enter Item Code"); return; }
    if ($('#txt_item_MinQty').val() == null || $("#txt_item_MinQty").val().trim().length == 0) { $("#btn_item_Save").attr("disabled", false); toastr.warning("Please Enter Min Quantity"); return; }
    if ($("#txt_item_PurchaseRate").val() * 1 > $("#txt_item_SellRate").val() * 1) { $("#btn_item_Save").attr("disabled", false); toastr.warning("Sale Rate should be greater or equal to Purchase Rate "); return; }

    if ($('#ddl_oilgrade').val() * 1 === 0 || $("#ddl_oilgrade").val().trim().length == 0) { $("#btn_item_Save").attr("disabled", false); toastr.warning("Please Select Oil Grade"); return; }
    if ($('#txt_oil_ltr').val() * 1 <= 0 || $("#txt_oil_ltr").val().trim().length == 0) { $("#btn_item_Save").attr("disabled", false); toastr.warning(" Please Enter Quantity in Litre"); return; }
    if ($('#qty_in_carton').val() * 1 <= 0 || $("#qty_in_carton").val().trim().length == 0) { $("#btn_item_Save").attr("disabled", false); toastr.warning("Please Enter Quantity In Carton"); return; }

    var obj = {
        id: $('#lbl_item_id').val() * 1,
        name: $('#txt_item_Name').val(),
        itemCode: $("#txt_item_Code").val(),
        unit: $("#ddl_item_unit").val(),
        minQuantity: $('#txt_item_MinQty').val(),
        purchasePrice: $('#txt_item_PurchaseRate').val() * 1,
        sellPrice: $('#txt_item_SellRate').val() * 1,
        description: $('#txt_item_Description').val(),
        taxPercent: 0,// $('#txt_item_tax').val() * 1,
        isTaxable: $('#chk_item_isTaxable').prop('checked'),

        oilGradeId: $('#ddl_oilgrade').val() * 1,
        packingInLitre: $('#txt_oil_ltr').val() * 1,
        quantityInCarton: $('#qty_in_carton').val() * 1,
    };
    var myurl = "/Item/SaveItem";
    var mydata = new Object();
    mydata.paramitem = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            Clearitem();
            $('#AddNewItemModal').modal('hide');
            LoadItemInfo();
        }
        if (result == "Code Already Exists") {
            $("#btn_item_Save").attr("disabled", false);
            toastr.warning(result);

        }
        if (result == "Name Already Exists") {
            $("#btn_item_Save").attr("disabled", false);
            toastr.warning(result);
        }
    });

}

function LoadItemInfo() {
    oTable = $("#tblItemInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Item/LoadItemTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "sScrollX": "100%",
        "sScrollXInner": "110%",
        "aoColumns": [
            { "data": 'name', "className": 'text-compressed  bold itemNameTD', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'itemCode', "className": 'text-compressed  bold itemCodeTD', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'unit', "className": 'text-compressed  bold itemUnitTD', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'minQuantity', "className": 'text-compressed NumbersAlign bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'purchasePrice', "className": 'text-compressed NumbersAlign itemPurchase bold', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            {
                "data": 'sellPrice', "className": 'text-compressed NumbersAlign  bold', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
           // { "data": 'taxPercent', "className": 'text-compressed NumbersAlign  bold', "bSortable": false, "orderSequence": ["desc", "asc"],},
             {

                 "data": 'description', "autoWidth": true, "className": 'text-compressed bold', "bSortable": false, "orderSequence": ["desc", "asc"], "render": function (full, type, row) {
                     if (row.description.length > 50) {
                         return row.description.substr(0, 50);
                     }
                     else
                         return row.description;
                 }
             },
            //{ "data": 'stock', "className": 'text-compressed bold text-center itemStockTD', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'stock', "className": 'text-compressed NumbersAlign bold itemStockTD', "bSortable": false, "orderSequence": ["desc", "asc"], "render": function (data, type, row) {
                    //if (row.stock == 0 && row.quantityInLitre == 0)
                    //    return 0.00.toFixed(2);
                    //else if (row.stock != 0 && row.quantityInLitre != 0)
                    return (row.stock * 1).toFixed(2);
                    //else {
                    //    if (row.stock == 0 && row.quantityInLitre != 0)
                    //return (row.quantityInLitre * 1).toFixed(2);
                    //else
                    //    return  (row.stock * 1).toFixed(2);
                    //}
                }
            },
            {
                "data": 'stockInLtrs', "className": 'text-compressed NumbersAlign bold', "bSortable": false, "orderSequence": ["desc", "asc"], "render": function (data, type, row) {
                    //if (row.stock == 0 && row.quantityInLitre == 0)
                    //    return 0.00.toFixed(2);
                    //else if (row.stock != 0 && row.quantityInLitre != 0)
                    return ((row.stock * 1) * (row.packingInLitre * 1)).toFixed(2);
                    //else {
                    //    if (row.stock == 0 && row.quantityInLitre != 0)
                            //return (row.quantityInLitre * 1).toFixed(2);
                        //else
                        //    return  (row.stock * 1).toFixed(2);
                    //}
                }
            },
             {
                 "data": 'stockInAmount', "className": 'text-compressed bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                     return CurrencyFormat(data);
                 }
             },
            { "data": 'oilGrade', "className": 'text-compressed bold oilGrade', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'packingInLitre', "className": 'text-compressed NumbersAlign  bold', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return data.toFixed(2);
                }
            },
            { "data": 'quantityInCarton', "className": 'text-compressed bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'id',  "className": 'text-compressed id bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.id + ' id="btn_item_Edit" class="form-control" />'
                                            + ' <input type="button"  value="Delete" rel=' + obj.id + ' id="btn_item_Delete" class="form-control" />';
                                            /*+ ' <input type="button"  value="Adjust Stock" rel=' + obj.id + ' id="btnAdjStock" class="form-control" />';*/
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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_item_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_item_MainEndDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "SearchType", "value": $('#ddl_item_SearchBy option:selected').val() });
            aoData.push({ "name": "SearchValue", "value": $('#txt_item_Search').val() });
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




function DeleteItem(itemId) {
    var myurl = "/Item/DeleteItem";
    var mydata = new Object();
    mydata.itemId = itemId;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Deleted", "success");
            LoadItemInfo();
        }
        else { toastr.warning(result); return; }
    });
}


function GetItemByID(itemId) {
    var myurl = "/Item/GetItemByID";
    var mydata = new Object();
    mydata.itemId = itemId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $('#AddNewItemModal').modal('show'); $(".modal-title").text('Edit Item');
            $('#lbl_item_id').val(result.id);
            $('#txt_item_Name').val(result.name);
            $('#txt_item_Code').val(result.itemCode);
            $('#ddl_item_unit').val(result.unit);
            $('#txt_item_MinQty').val(result.minQuantity);
            $('#txt_item_PurchaseRate').val(result.purchasePrice);
            $('#txt_item_SellRate').val(result.sellPrice);
            $('#txt_item_Description').val(result.description);
            //$('#txt_item_tax').val(result.taxPercent);
            $('#chk_item_isTaxable').prop('checked', result.isTaxable);
            $('#ddl_oilgrade').val(result.oilGradeId.toString());
            $('#txt_oil_ltr').val(result.packingInLitre);
            $('#qty_in_carton').val(result.quantityInCarton);
        }

    });
}


function GetOilGradeData() {
    var myurl = "/Item/GetOilGradeData";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.ddl_oilgrade').append(cloneDom);
        }
        //$('.dd_sell_sales_person').chosen();
        //$(".chosen-select").chosen({ width: '100%' });
    });
}

