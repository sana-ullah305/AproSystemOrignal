function StockInTrigger() {

    var myurl = "/StockIn/SaveStockIn";
    var mydata = new Object();
    var objx = { coaID: 132, empID: 0, date: '2019-02-11', netAmount: 200, items: [{ itemID:1, qty :2,unitPrice:100,amount :200}], comments: '' };

    mydata.stockIn = JSON.stringify(objx);
    ShowAjaxLoader();
    XHRPOSTRequest(myurl, mydata, function (result) {
        //if (result == "success") {
        //    HideAjaxLoader();
        //    $("#btn_purchase_Save").attr("disabled", false);
        //    showNotification("Saved", "success");
        //    ClearPurchase();
        //    $('#AddNewPurchaseModal').modal('hide');
        //    LoadPurchaseInfo();
        //    // ClearCallLogs();

        //}
        HideAjaxLoader();

    });
}