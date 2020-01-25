
function ReportsTrigger() {
    //Purchase List Report Portion - Start -

    //if ($('#purchaseListReportDiv').length > 0) { alert('loaded'); }

    //Purchase List Report Portion - End -

    $("#btn_PurchaseList_Search").on("click", function () {
        GetPurchaseList();
    });

    $("#btn_PurchaseList_View").on("click", function () {
        GetPurchaseList(true);
    });

    $("#btnPaymentVoucherDownload").on("click", function () {
        GetPaymentVoucher();
    });

    $("#btn_report_IncomeStatement").on("click", function () {
        location.href = "/Report/IncomeStatement";

    });

    $("#btn_IncomeStatement_Search").on("click", function () {
        GetIncomeStatement();
    });
    $("#btn_IncomeStatement_View").on("click", function () {
        GetIncomeStatement(true);
    });

    $("#btn_report_WeeklyCashFlow").on("click", function () {
        location.href = "/Report/WeeklyCashFlow";
    });
    $("#btn_weeklyCashFlow_Search").on("click", function () {
        GetWeeklyCashFlow();
    });
    $("#btn_weeklyCashFlow_View").on("click", function () {
        GetWeeklyCashFlow(true);
    });
    $("#btn_MonthlySubscription_Search").on("click", function () {
        DownloadMonthlySubscription();
    });
    $("#btn_report_itemSalesProfitList").on("click", function () {
        location.href = "/Report/ItemsSalesProfitList";
    });
    $("#btn_itemSalesProfitList_Search").on("click", function () {
        DownloadItemSalesProfitList();
    });
    $("#btn_itemSalesProfitList_View").on("click", function () {
        DownloadItemSalesProfitList(true);
    });

    $("#btn_AccountsPayable_Search").on("click", function () {

        DownloadAccountsPayable();
    });
    $("#btn_AccountsPayable_View").on("click", function () {

        DownloadAccountsPayable(true);
    });

    $("#btn_AccountsReceivable_Search").on("click", function () {
        DownloadAccountsReceivable();
    });
    $("#btn_AccountsReceivable_View").on("click", function () {
        DownloadAccountsReceivable(true);
    });

    $("#btn_SalesPerson_View").on("click", function () {
        GetSalesPersonReport(true);
    });


    $("#btn_SalesPerson_Search").on("click", function () {
        GetSalesPersonReport();
    });

    //sanaullah for customers report
    $("#btn_CustomerReport_View").on("click", function () {
        GetCustomerReport(true);
    });

    $("#btn_CustomerSearch_Search").on("click", function () {
        GetCustomerReport();
    });

    $(document).on('change', '.ddl_AccountsReceivable_Vendor', function () {

        var id = $(".ddl_AccountsReceivable_Vendor").val() * 1;
        if (id > 0) {
            if (id == 1) { GetCustomers(1); }
            if (id == 2) { GetCustomers(2); }

        }
    });
    $("#btn_trialbalance_Search").on("click", function () {
        //  GetTrialBalance();
        LoadTrialBalance();
    });
}
$("#btn_BalanceSheet_View").on("click", function () {
    DownloadBalanceSheet(true);
});

$("#btn_BalanceSheet_download").on("click", function () {
    DownloadBalanceSheet();
});
function DownloadBalanceSheet(Preview) {
    var myurl = "/Report/DownloadBalanceSheet?";
    var mydata = new Object();
    mydata.dtStart = $("#dtp_BalanceSheet_MainStartDate").val();
    mydata.dtEnd = $("#dtp_BalanceSheet_MainEndDate").val();
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}
function GetPaymentVoucher() {
    var myurl = "/Report/DownloadPaymentVoucherList?";
    var mydata = new Object();
    mydata.dtStart = $("#dtPaymentVoucherMainStartDate").val();
    mydata.dtEnd = $("#dtPaymentVoucherMainEndDate").val();
    if (!mydata.dtStart) {
        toastr.warning("Please select Start date");
        return;
    }
    if (!mydata.dtEnd) {
        toastr.warning("Please select End date");
        return;
    }
    DownloadFiles(myurl, mydata);
}


function GetTrialBalance() {
    var myurl = "/Report/DownloadtrialBalance?";
    var mydata = new Object();
    mydata.dtStart = $("#dtp_trialbalance_MainStartDate").val();
    mydata.dtEnd = $("#dtp_trialbalance_MainEndDate").val();
    if (!mydata.dtStart || !mydata.dtEnd) {
        toastr.warning("Please select start and end date");
        return;
    }
    DownloadFiles(myurl, mydata);
}


function GetIncomeStatement(Preview) {
    var myurl = "/Report/DownloadIncomeStatement?";
    var mydata = new Object();
    mydata.dtStart = $("#dtp_incomestatement_MainStartDate").val();
    mydata.dtEnd = $("#dtp_incomestatement_MainEndDate").val();
    if (!mydata.dtStart) {
        toastr.warning("Please select Start date");
        return;
    }
    if (!mydata.dtEnd) {
        toastr.warning("Please select End date");
        return;
    }
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}


function GetSalesPersonReport(Preview) {
    var myurl = "/Report/DownloadSalesPersonReport?";
    var mydata = new Object();
    mydata.dtStart = $("#dtp_salesPerson_MainStartDate").val();
    mydata.dtEnd = $("#dtp_salesPerson_MainEndDate").val();
    var salesPersons = $("#ddl_salesperson").val();
    if (salesPersons) {

        var val = "";
        for (var i = 0; i < salesPersons.length; i++) {
            if (val) {
                val = val + ",";
            }
            val += salesPersons[i];
        }

    }
    mydata.SalesPersonCSV = val;
    if (!mydata.dtStart) {
        toastr.warning("Please select Start date");
        return;
    }
    if (!mydata.dtEnd) {
        toastr.warning("Please select End date");
        return;
    }
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}


function GetCustomerReport(Preview) {
    var myurl = "/Report/DownloadCustomerReport?";
    var mydata = new Object();
    mydata.dtStart = $("#dtp_Customer_MainStartDate").val();
    mydata.dtEnd = $("#dtp_Customer_MainEndDate").val();
    var customers = $("#ddl_customer").val();
    if (customers) {

        var val = "";
        for (var i = 0; i < customers.length; i++) {
            if (val) {
                val = val + ",";
            }
            val += customers[i];
        }

    }
    mydata.customerIds = val;
    if (!mydata.dtStart) {
        toastr.warning("Please select Start date");
        return;
    }
    if (!mydata.dtEnd) {
        toastr.warning("Please select End date");
        return;
    }
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
        HideAjaxLoader();
    }
}



function GetPurchaseList(Preview) {
    if ($('#dtpi_MainStartDate').val() == "") { toastr.warning("Please Select Start Date "); return; }
    if ($('#dtpi_MainEndDate').val() == "") { toastr.warning("Please Select End Date "); return; }
    var myurl = "/Report/DownloadPurchaseList?";
    var mydata = new Object();
    mydata.vendorID = $("#ddlVendor").val() * 1;
    mydata.itemID = $("#ddlItem").val() * 1;
    mydata.dtStart = $("#dtpi_MainStartDate").val();
    mydata.dtEnd = $("#dtpi_MainEndDate").val();
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}


function GetWeeklyCashFlow(Preview) {
    var myurl = "/Report/DownloadWeeklyCashFlow?";
    var mydata = new Object();
    if ($("#dtp_weeklycashflow_MainStartDate").val() == "") { toastr.warning("Please Select Date"); return; }
    mydata.dtStart = $("#dtp_weeklycashflow_MainStartDate").val();
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}


function DownloadMonthlySubscription(Preview) {
    var myurl = "/Report/DownloadMonthlySubscription?";
    var mydata = new Object();
    mydata.year = 2019;
    mydata.SubsType = $(".ddlSubType").val();
    DownloadFiles(myurl, mydata);
}





function DownloadItemSalesProfitList(Preview) {
    var myurl = "/Report/DownloadItemWiseSales?";
    var mydata = new Object();
    mydata.dtStart = $("#dtp_itemSalesProfitList_MainStartDate").val();
    mydata.dtEnd = $("#dtp_itemSalesProfitList_MainEndDate").val();
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}


function DownloadAccountsPayable(Preview) {
    var myurl = "/Report/DownloadAccountsPayable?";
    var mydata = new Object();
    mydata.vendorID = $("#ddl_AccountsPayable_Vendor").val();
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}



function DownloadAccountsReceivable(Preview) {
    var myurl = "/Report/DownloadAccountsReceivable?";
    var mydata = new Object();
    mydata.custID = $("#ddl_AccountsReceivable_SubCustomer").val();
    mydata.typeID = $("#ddl_AccountsReceivable_Vendor").val();
    if (Preview) {
        PreviewReport(myurl, mydata);
    } else {
        DownloadFiles(myurl, mydata);
    }
}

function GetVendors(control) {
    var myurl = "/Vendor/GetVendorList";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            control.append('<option value="0">--All--</option>');
            for (var i = 0; i < result.length; i++) {
                control.append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            Vendorlist = result;
        }
    });
}

function GetItems(control) {
    var myurl = "/Item/GetItemList";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            console.log(result);
            control.append('<option value="0">--All--</option>');
            for (var i = 0; i < result.length; i++) {
                control.append('<option value="' + result[i].id + '">' + result[i].name + '</option>');
            }
        }
    });
}



function GetCustomers(typeID) {
    var myurl = "/Customer/GetCustomerListByType";
    var mydata = new Object();
    mydata.typeID = typeID;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_AccountsReceivable_SubCustomer').empty();
            //$('.ddl_AccountsReceivable_SubCustomer').val("ALL");
            $('.ddl_AccountsReceivable_SubCustomer').append('<option value="0">' + "All" + '</option>');
            for (var i = 0; i < result.length; i++) {
                $('.ddl_AccountsReceivable_SubCustomer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            //$('#ddl_AccountsReceivable_SubCustomer').val(0);
        }
    });
}





function LoadTrialBalance() {
    oTable = $("#tbltialbalance").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Report/GETtrialBalance",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "aoColumns": [
            { "data": 'account', "className": 'text-compressed  bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'debit', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'credit', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },



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
        order: [[0, "desc"]],

        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_trialbalance_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_trialbalance_MainEndDate').val()).format("YYYY-MM-DD") });

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

function PreviewReport(myurl, mydata) {
    mydata.Preview = true;
    myurl = myurl + $.param(mydata);
    ShowAjaxLoader();
    $("#ReportPreview").off("load");
    $("#ReportPreview").on("load", function () {
        HideAjaxLoader();
    });
    $("#ReportPreview").attr("src", myurl);
}

