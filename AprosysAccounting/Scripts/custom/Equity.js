
function EquityTrigger() {

    GetShareHoldersName();
    LoadBankList();
    FillInvestorTable();
    LoadEquityInfoTable();
    //LoadServiceTable();
    $("#btn_equity_Deposit").on("click", function () {
        //if ($("#txt_service_Name").val() != null && $("#txt_service_Name").val().length > 0)
        //{ SaveService($("#txt_service_Name").val()); }
        Clear();
        $('#hdn_equity_deposit_or_withdraw').val(1);
        $('#AddNewEquityModal').modal('show');
        $(".modal-title").text("Add New Deposit");

    });
    $("#btn_equity_WithDraw").on("click", function () {
        //if ($("#txt_service_Name").val() != null && $("#txt_service_Name").val().length > 0)
        //{ SaveService($("#txt_service_Name").val()); }
        Clear();
        $('#hdn_equity_deposit_or_withdraw').val(0);
        $('#AddNewEquityModal').modal('show');
        $(".modal-title").text("Add New WithDraw");

    });

    $('#dd_equity_tranMode').on('change', function () {
        $(this).val() == "1" ? $('#dd_equity_bankNames').attr('disabled', 'disabled') : $('#dd_equity_bankNames').removeAttr('disabled')
    });

    $(document).on('click', '#btn_equity_Delete', function () {


        var id = $(this).attr('rel');
        if (id != null) {
            if (confirm("Are you sure to Delete  ? ")) {

                DeleteEquityInvoice(id);
            }
        }
    });

}
function Clear() {
    $('#dd_equity_investor').val('0');
    $('#dd_equity_tranMode').val('1');
    $('#txt_equity_amount').val('');
    $('#dtp_equity_activityTime').val();
    $('#dd_equity_bankNames').val('0');
    $('#dd_equity_bankNames').attr('disabled', 'disabled');
    $('#hdn_equity_deposit_or_withdraw').val('');
    $('#new_invstr_name').val('');
    $('#txt_investor_comments').val('');
    $('#txt_Investor_equity_amount').val('');
    $('#investor_id').val('');

}

function GetShareHoldersName() {
    var myurl = "/Equity/GetShareHoldersName";
    var mydata = new Object();

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.dd_equity_investor').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.dd_equity_investor').append('<option value="' + result[i].coaId + '">' + result[i].treeName + '</option>');
            }

        }
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
            $("#dd_equity_bankNames").append(tmp);
        }
    });

}

$('#btn_equity_SaveEquity').click(function () {
    $('#dtp_equity_activityTime').val(FormatDateTimeToDisplay(new Date()));
    validateSend();
    LoadEquityInfoTable();
});


function validateSend() {

    if ($('#dd_equity_investor').val() == '0' || $("#dd_equity_investor").val().trim().length == 0) { $("#btn_equity_SaveEquity").attr("disabled", false); toastr.warning("Please Select The Investor "); return; }
    if ($('#dd_equity_tranMode').val() == "1" && ($('#txt_equity_amount').val() == '' || $("#txt_equity_amount").val().trim().length == 0)) { $("#btn_equity_SaveEquity").attr("disabled", false); toastr.warning("Please Enter Amount "); return; }
    if ($('#dd_equity_tranMode').val() == "2" && ($('#dd_equity_bankNames').val() == '0' || $("#dd_equity_bankNames").val().trim().length == 0)) { $("#btn_equity_SaveEquity").attr("disabled", false); toastr.warning("Please Select The Bank "); return; }

    var data =
    {
        investorId: $('#dd_equity_investor').val(),
        amount: $('#txt_equity_amount').val(),
        accountId: $('#dd_equity_bankNames').val() * 1,
        activityTime: $('#dtp_equity_activityTime').val(),
        comments: $('#txt_equity_comments').val(),
        isdeposit: $('#hdn_equity_deposit_or_withdraw').val() == "1" ? true : false
    }
    var myurl = "/Equity/InsertEquityInfo";
    var mydata = new Object();
    mydata.param = JSON.stringify(data);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            $('#AddNewEquityModal').modal('hide');
            toastr.success('saved');
        }
    });
}


function FillInvestorTable() {
    //$("#tblInvestorDetail").DataTable();
    $(".tblInvestorDetails_tblactive").empty();
    var myurl = "/Equity/GetInvestorList";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length != 0) {
            $('.tblInvestorDetails_tblactive').html('');
            for (var i = 0; i < result.length; i++) {
                $('.tblInvestorDetails_tblactive').append('<tr><td class="text-center investorName ">' + result[i].investorName + '</td> '
                    + '<td class="text-center amount ">' + CurrencyFormat((result[i].amount != null) ? result[i].amount : 0) + '</td>'
                    + '<td class="text-center"> '
                    + '<input type="button" class="btn btn-success btnedit" rel=' + result[i].investorId + ' style="width:16%;margin-right:5%" value="Edit" />'
                    + '<input type="button" class="btn btn-danger btnDelete" rel=' + result[i].investorId + ' style="" value="Delete" />'
                    + '</td>');
            }
        }
    });
}

$('#btn_add_newInvestor').on('click', function () {
    Clear();
    $(".modal-title").text("Add New Investor");
    $('#AddInvestorModal').modal('show');
});


$(document).on('click', '.btnedit', function () {
    Clear();
    $('#investor_id').val($(this).attr('rel'));
    $(".modal-title").text("Edit Investor");
    $('#AddInvestorModal').modal('show');
    var values = [];
    var $row = $(this).closest("tr");
    $row.find('td').each(function () {
        values.push($(this).text());
    });
    $('#new_invstr_name').val(values[0]);
    $('#txt_Investor_equity_amount').val(values[1]);
    //$('#dd_equity_tranMode').attr('disabled', 'disabled');

});


//$('#dd_Investor_tranMode').on('change', function () {
//    $(this).val() == "1" ? $('#dd_equity_bankNames').attr('disabled', 'disabled') : $('#dd_equity_bankNames').removeAttr('disabled')
//});


$('#btn_Investor_SaveInvestor').on('click', function () {
    investorId = $('#investor_id').val() == "" ? "" : $('#investor_id').val();
    validateAndSaveInvestor(investorId);
    FillInvestorTable();
});


$(document).on('click', '.btnDelete', function () {
    if (confirm('Are you sure to delete investor record ??'))
        deleteInvestorRecord($(this).attr('rel'));
    //$('#dd_equity_tranMode').attr('disabled', 'disabled');

});

function deleteInvestorRecord(investorId) {
    Clear();
    var myurl = "/Equity/DeleteInvestor";
    var mydata = new Object();
    mydata.CoaId = investorId;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            $('#AddInvestorModal').modal('hide');
            toastr.success('saved');
            FillInvestorTable();
        }
        else {
            toastr.warning(result)
        }
    });

}


function validateAndSaveInvestor(investorId) {
    if ($('#new_invstr_name').val() == "" || $("#new_invstr_name").val().trim().length == 0) { $("#btn_Investor_SaveInvestor").attr("disabled", false); toastr.warning("Please Enter Investor Name "); return; }
    //if ($('#txt_Investor_equity_amount').val() == '' || $("#txt_Investor_equity_amount").val().trim().length == 0) { $("#btn_Investor_SaveInvestor").attr("disabled", false); toastr.warning("Please Enter Amount "); return; }
    //if ($('#dd_Investor_tranMode').val() == "2" && ($('#new_invstr_bank_account').val() == '' || $("#dd_Investor_tranMode").val().trim().length == 0)) { $("#btn_equity_SaveEquity").attr("disabled", false); toastr.warning("Please Select The Bank "); return; }
    var myurl = "/Equity/SaveInvestor";
    var obj = {
        investorId: investorId,
        investorName: $('#new_invstr_name').val(),
        //amount: $('#txt_Investor_equity_amount').val()
    };
    var mydata = new Object();
    mydata.paraminvestor = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            $('#AddInvestorModal').modal('hide');
            Clear();
            toastr.success('saved');
            FillInvestorTable();
        }
        else {
            toastr.warning(result);
        }
    });
}


function LoadEquityInfoTable() {
    oTable = $("#tblEquityInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Equity/LoadEquityInfoTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "aoColumns": [
             { "data": 'InvoiceNo', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
              { "data": 'ActivityType', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'Equity', "className": 'text-compressed bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
             { "data": 'BankAccount', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
             {
                 "data": 'Amount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                     return CurrencyFormat(data);
                 }
             },


            

            {
                "data": 'ActivityDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    if (obj["ActivityDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                    else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["ActivityDate"])) + '</td>' }
                }
            },
            {
                "data": 'InvoiceNo', "className": 'text-compressed InvoiceNo bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += '<input type="button"  value="Delete" rel=' + obj.InvoiceNo + ' id="btn_equity_Delete" class="form-control" />';
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
            //aoData.push({ "name": "Start_Date", "value": moment($('#dtp_pvoucher_MainStartDate').val()).format("YYYY-MM-DD") });
            //aoData.push({ "name": "End_Date", "value": moment($('#dtp_pvoucher_MainEndDate').val()).format("YYYY-MM-DD") });
            //// aoData.push({ "name": "SearchType", "value": $('#ddl_customer_SearchBy option:selected').val() });


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





function DeleteEquityInvoice(_voucherId) {
    var myurl = "/Equity/DeleteEquityInvoice";
    var mydata = new Object();
    mydata.invoiceNo = _voucherId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Deleted", "success");
            LoadEquityInfoTable();
        }
    });
}