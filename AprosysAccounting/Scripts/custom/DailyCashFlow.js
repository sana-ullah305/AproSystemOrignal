function InitDailyCashFlow() {

    $('.cashFlowContainer').hide();
    $('#btnProceed').click(function () {
        var datex = $('#dcf_CurrDate').val();
        if (datex == '') { alert('Date connot be empty'); return; }
        GetDailyCashFlow(datex);
    });
    $('#btnAddCashOut').click(function () {
        if ($("#InpHAllowEditDate").val().toLowerCase()!="true" && moment(new Date()).format("YYYY-MM-DD") != $("#dcf_CurrDate").val()) {
            swal("Expense Can Only be added on Current Date");
            return;
        }
        $("#AddNewPaymentVoucherModal").modal('show');
        if ($("#dcf_CurrDate").length > 0) {
            var dt = new Date();
           
            $('#dtp_pvoucher_ActivityDate').val($("#dcf_CurrDate").val() +' '+ dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds());
            $('#dtp_pvoucher_ActivityDate').prop('disabled', true);
            $("#ddl_pvoucher_ExpenseType").val(0);
            //$("#ddl_pvoucher_Expense").trigger('change');
        }
    });
    $('#btn_dailyCashFlow_download').click(function () {
        var datex = $('#dcf_CurrDate').val();
        if (datex == '') { alert('Date connot be empty'); return; }
        DownloadDailyCashFlow(datex);
    });
    $("#btn_pvoucher_closeExpense").on("click", function () {
        $("#txt_pvoucher_AdministrativeExpense").val("");
    });
}

function DownloadDailyCashFlow(datex) {
    var myurl = "/Report/DownloadDailyCashFlow?";
    var mydata = new Object();
    mydata.date = datex;
    DownloadFiles(myurl, mydata);
}

function GetDailyCashFlow(datex) {
    ShowAjaxLoader();
    var myurl = "/DailyCashFlow/GetDailyCashFlow";
    var mydata = new Object();
    mydata.date = datex;
    XHRPOSTRequest(myurl, mydata, function (result) {
        HideAjaxLoader();
        console.log(result);
        MapOpenings(result);
        MapDeposit(result);
        MapCashOut(result);
        MapItemShopMonthly(result);
        MapTotals(result);
        $('.cashFlowContainer').show();
    });
}

function MapOpenings(res) {
    var openRow = $.grep(res, function (rr) { return rr.TYPEID == 0; })[0];
    $("#lblDay").text(openRow.WeekDay);
    $("#lblOpening").text(formatNumber(openRow.AMOUNT.toFixed(2)));
}

function MapDeposit(res) {
    var depRow = $.grep(res, function (rr) { return rr.GROUPID == 2; })[0];
    $(".ssmDeposit").text(formatNumber(depRow.AMOUNT.toFixed(2)));
}

function MapTotals(res) {
    var nettotal = 0, monthlyTotal = 0, shopTotal = 0, saleTotal = 0,creditCashTotal=0, cashoutTotal = 0;
    for (var i = 0; i < res.length; i++) {
        nettotal = nettotal + res[i].AMOUNT;
        if (res[i].TYPEID == 10) {
            creditCashTotal = creditCashTotal + res[i].AMOUNT;
        }
        if (res[i].TYPEID == 2) {
            monthlyTotal = monthlyTotal + res[i].AMOUNT;
        }
        else if (res[i].TYPEID == 3) {
            shopTotal = shopTotal + res[i].AMOUNT;
        }
        else if (res[i].TYPEID == 1) {
            saleTotal = saleTotal + res[i].AMOUNT;
        }
        else if (res[i].GROUPID == 3) { cashoutTotal = cashoutTotal + res[i].AMOUNT; }
    }
    var deposit = $.grep(res, function (rr) { return rr.GROUPID == 2; })[0].AMOUNT;
    var opening = $.grep(res, function (rr) { return rr.TYPEID == 0; })[0].AMOUNT;
    //var totalCashInhand = ((opening + monthlyTotal + shopTotal + saleTotal) + deposit) - cashoutTotal;
    $(".totalCashInHand").text(formatNumber(nettotal.toFixed(2)));
    $(".totalMonthly").text(formatNumber(monthlyTotal.toFixed(2)));
    $(".totalShop").text(formatNumber(shopTotal.toFixed(2)));
    $(".totalSale").text(formatNumber(saleTotal.toFixed(2)));
    $(".totalPartialCredit").text(formatNumber(creditCashTotal.toFixed(2)));
}

function MapItemShopMonthly(res) {
    var openRow = $.grep(res, function (rr) {res.sort(rr.typeID); return rr.GROUPID == 1; });// $.grep(res, function (rr) { return rr.GROUPID == 1; });
    openRow = openRow.sort(function (a, b) { return a.TYPEID - b.TYPEID; });
    var dummyRow = $('<tr class=""><td class="col-xs-4 ssmItem"></td><td class="col-xs-4 ssmType"></td><td class="col-xs-1 text-right ssmQuantity"></td><td class="col-xs-1 text-right ssmUnitPrice"></td><td class="col-xs-1 text-right ssmAmount"></td><td class="col-xs-1 text-right ssmTax"></td></tr>');
    var totAmount = 0, totTax = 0, totUp = 0, totQuantity = 0;
    $('.bodySSM').html('');
    for (var i = 0; i < openRow.length; i++) {
        var curr = openRow[i];
        var cloneDiv = dummyRow.clone();
        $('.ssmItem', cloneDiv).text(curr.NAME);
        $('.ssmType', cloneDiv).text(curr.TYPENAME);
        $('.ssmQuantity', cloneDiv).text(curr.Quantity);
        $('.ssmUnitPrice', cloneDiv).text(formatNumber( curr.UnitPrice.toFixed(2)));
        $('.ssmAmount', cloneDiv).text(formatNumber(curr.AMOUNT.toFixed(2)));
        $('.ssmTax', cloneDiv).text(formatNumber(curr.TAX.toFixed(2)));
        $('.bodySSM').append(cloneDiv);
        totAmount = totAmount + curr.AMOUNT;
        totTax = totTax + curr.TAX;
        totUp = totUp + curr.UnitPrice;
        totQuantity = totQuantity + curr.Quantity;
    }
    $('.ssmTotQty').text(totQuantity);
    $('.ssmTotPrice').text(formatNumber(totUp.toFixed(2)));
    $('.ssmTotTotal').text(formatNumber(totAmount.toFixed(2)));
    $('.ssmTotTaxes').text(formatNumber(totTax.toFixed(2)));
}

function MapCashOut(res) {
    var openRow = $.grep(res, function (rr) { return rr.GROUPID == 3; });
    var dummyRow = $('<tr class=""><td class="col-xs-4 coItem"></td><td class="col-xs-4"></td><td class="col-xs-1 text-right"></td><td class="col-xs-1 text-right"></td><td class="col-xs-1 text-right coType"></td><td class="col-xs-1 text-right"></td></tr>');
    $('.bodyCashOut').html('');
    var coAmount = 0;
    for (var i = 0; i < openRow.length; i++) {
        var curr = openRow[i];
        var cloneDiv = dummyRow.clone();
        $('.coItem', cloneDiv).text(curr.NAME);
        $('.coType', cloneDiv).text(formatNumber(curr.AMOUNT));
        $('.bodyCashOut').append(cloneDiv);
        coAmount = coAmount + curr.AMOUNT;
    }
    $('.cashoutTotal').text(formatNumber(coAmount));
}

function formatNumber(value) {
    if (value < 0) {
        value = value * -1;
        return "PKR  (" + value.toFixed(2) + ")";
    }
    else {
        return "PKR " + (value * 1).toFixed(2);
    }
};

//$("#btn_pvoucher_SaveExpense").on("click", function () {
//    if ($("#txt_pvoucher_AdministrativeExpense").val().length > 0) { SaveAdministrativeExpense($("#txt_pvoucher_AdministrativeExpense").val()); }
//});

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
        //if (result == 0) {
        //    toastr.warning("Expense Already Exists");

        //}
        if (result == "Expense Already Exists") {

            toastr.warning(result);

        }
    });
}

