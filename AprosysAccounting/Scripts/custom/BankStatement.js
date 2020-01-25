function BankStatementTrigger() {
    $("#dtp_bankStatement_EndDate").datepicker('setDate',new Date());
    $("#dtp_bankStatement_StartDate").datepicker('setDate',new Date(2019,00,01));
    LoadBankList();
    $("#btn_bankStatement_View").click(function () {
        DownloadBankStatement(true);
    });

    $("#btn_bankStatement_Download").click(function () {
        DownloadBankStatement(false);
    });
}

function LoadBankList() {
    XHRPOSTRequest("/Banks/GetBanksList", {}, function (data) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].bankName == "Cash") {
                continue;
            }
            var tmp = $("<option></option>");
            tmp.attr("value", data[i].bankID);
            tmp.text(data[i].bankName);
            $("#ddlBankList").append(tmp);
        }
    });

}

function DownloadBankStatement(Preview) {
    var myurl = "/Report/DownloadBankStatement?";
    var mydata = new Object();
    mydata.dtStart = $("#dtp_bankStatement_StartDate").val();
    mydata.dtEnd = $("#dtp_bankStatement_EndDate").val();
    if (!mydata.dtStart || !mydata.dtEnd) {
        window.alert("Start end End Dates are mandatory");
    }
    mydata.bankID = $("#ddlBankList").val();
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}