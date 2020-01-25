function BonusRevenueInformationTrigger() {
    $("#dtp_evenue_account_MainStartDate").val(moment().subtract('days', 90).format('YYYY-MM-DD'));
    $("#dtp_revenue_account_MainEndDate").val(moment().format('YYYY-MM-DD'));
    LoadBonusRevenueInfo();
    GetRevenueAccountData();
    GetBankData();
}

$('#btn_AddNewBonusRevenue').click(function () {
    $('.modal-title').text('Add New Bonus Revenue');
    clearControls();
    $('#dtp_RevenueAccount_Date').val(FormatDateTimeToDisplay(new Date()));
    $('#NewBonusRevenueModal').modal('show');
});

$(document).on('click', '#btn_bonus_revenue_Edit', function () {
    clearControls();
    $('.modal-title').text('Edit Bonus Revenue');
    $('#NewBonusRevenueModal').modal('show');
    $('#btnSaveBonusRevenue').attr('rel', $(this).attr('rel'));
    getBonusDataByTransId($(this).attr('rel'));

});
$(document).on('click', '#btn_bonus_revenue_Delete', function () {
    if (confirm('Are You Sure To Delete Bonus Revenue Record ??'))
        deleteBonusDataByTransId($(this).attr('rel'));

});
function deleteBonusDataByTransId(transId) {
    var myurl = "/BonusRevenue/DeleteBonus";
    var mydata = new Object();
    mydata.transId = transId;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result === "Success") {
            oTable.ajax.reload();
            showNotification("Saved", "success");
        }
    });
}
$('#btnSaveBonusRevenue').click(function () {
    validateBonusFormData($(this).attr('rel') != '' ? $(this).attr('rel') : null);
});

$('#btn_bonusrevenue_Search').click(function () {
    oTable.ajax.reload();
});
function getBonusDataByTransId(transId) {
    var myurl = "/BonusRevenue/GetBonusRecordById";
    var mydata = new Object();
    mydata.transId = transId;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $('#dtp_RevenueAccount_Date').val(FormatDateTimeToDisplay(GetJSDate(result.activityDate)));
            $('#txt_bonus_amount').val(result.BonusAmount);
            $('#dd_revenue_account').val(result.RevenueAccount);
            $('#dd_bonus_revenue_bank').val(result.BankAccount);
            $('#txt_bonus_revenue_Misc').val(result.Misc);
        }

    });
}

function validateBonusFormData(TransactionId) {
    if ($('#dtp_RevenueAccount_Date').val() == null || $("#dtp_RevenueAccount_Date").val().trim().length == 0) { $("#btnSaveBonusRevenue").attr("disabled", false); toastr.warning("Please Select Date"); return; }
    if ($('#txt_bonus_amount').val() * 1 == 0 || $('#txt_bonus_amount').val() == null || $("#txt_bonus_amount").val().trim().length == 0) { $("#btnSaveBonusRevenue").attr("disabled", false); toastr.warning("Please Enter Bonus Amount"); return; }
    if ($('#dd_revenue_account').val() * 1 == 0 || $('#dd_revenue_account').val() == null || $("#dd_revenue_account").val().trim().length == 0) { $("#btnSaveBonusRevenue").attr("disabled", false); toastr.warning("Please Select Revenue Account"); return; }
    if ($('#dd_bonus_revenue_bank').val() * 1 == 0 || $('#dd_bonus_revenue_bank').val() == null || $("#dd_bonus_revenue_bank").val().trim().length == 0) { $("#btnSaveBonusRevenue").attr("disabled", false); toastr.warning("Please Enter Bank"); return; }
    var mydata = createObject(TransactionId);
    saveBonus(mydata);
};
function createObject(TransactionId) {
    var objData = new Object();
    objData = {
        "TransactionId": TransactionId != null ? TransactionId : null,
        "activityDate": $('#dtp_RevenueAccount_Date').val(),
        "BonusAmount": $('#txt_bonus_amount').val(),
        "RevenueAccount": $('#dd_revenue_account').val(),
        "BankAccount": $('#dd_bonus_revenue_bank').val(),
        "Misc": $('#txt_bonus_revenue_Misc').val()
    };
    return objData;
};
function saveBonus(mydata) {
    var myurl = "/BonusRevenue/SaveBonus";
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result === "Success") {
            showNotification("Saved", "success");
            $('#NewBonusRevenueModal').modal('hide');
            oTable.ajax.reload();
        }
        else {
            toastr.warning("Bonus Revenue Already Exists");
        }
    });
};
function LoadBonusRevenueInfo() {
    oTable = $("#tblBonusRevenueInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/BonusRevenue/GetBonusRevenueList",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            {
                "data": 'activityDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    if (obj["activityDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                    else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["activityDate"])) + '</td>' }
                }
            },
            {
                "data": 'BonusAmount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            { "data": 'RevenueAccount', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'BankAccount', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'TransactionId', "className": 'text-compressed TransactionId bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";

                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.TransactionId + ' id="btn_bonus_revenue_Edit" class="form-control" />'
                        + ' <input type="button"  value="Delete" rel=' + obj.TransactionId + ' id="btn_bonus_revenue_Delete" class="form-control" />';
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

            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_evenue_account_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_revenue_account_MainEndDate').val()).format("YYYY-MM-DD") });

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
function GetRevenueAccountData() {
    var myurl = "/BonusRevenue/GetRevenueAccountList";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.dd_revenue_account').append(cloneDom);
        }
        //$('.dd_revenue_account').chosen();
        //$(".chosen-select").chosen({ width: '100%' });
    });
}


function GetBankData() {
    var myurl = "/BonusRevenue/GetBankList";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.dd_bonus_revenue_bank').append(cloneDom);
        }
        //$('.dd_revenue_account').chosen();
        //$(".chosen-select").chosen({ width: '100%' });
    });
}

function clearControls() {
    $('#dtp_RevenueAccount_Date').val('');
    $('#txt_bonus_amount').val('');
    $('#dd_revenue_account').val('0');
    $('#dd_bonus_revenue_bank').val('0');
    $('#txt_bonus_revenue_Misc').val('');
    $('#btnSaveBonusRevenue').removeAttr('rel');
}