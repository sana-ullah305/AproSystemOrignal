

var BankID = 0;

function BankDetail()
{
   
    FillBankTable();
    CashAmount();
    //GetOutStandingCheque();
    $(document).on('click', '.Deposit', function () {
     
        $('.DepositAmount').val("");
        $("#dtp_bank_depositDate").val(FormatDateTimeToDisplay(new Date()));
        $('#DepositModal').modal('show');
        clearAll();
        BankID = $(this).attr('rel');
        GetOutStandingCheque();
    });

    $(document).on('click', '.Withdraw', function () {
        $('#WithdrawModal').modal('show');
      
        $(".WithDrawAmount").val("");
        $("#dtp_bank_withDrawDate").val(FormatDateTimeToDisplay(new Date()));
        BankID = $(this).attr('rel');
    });

    $(document).on('click', '.DepositAmount_Save', function () {
        
        var Amount = $('.DepositAmount').val() * 1;
        var BankID_ = BankID;
        if (Amount == "" || Amount == null) {
            showNotification("Enter Amount", "warning");
            return;
        }
        var comment = $("#inp_DepositComment").val();
        if (!comment) {
            if (!window.confirm("Do you want to save without comment", "warning")) {
                return;
            }
            
        }
        var TranDatetime = $("#dtp_bank_depositDate").val();
         var documentID = null;
        if ($("#ddl_bank_DocumentId Option:Selected").text().length > 0) {
            documentID = $("#ddl_bank_DocumentId Option:Selected").text();
        }
        SaveAmount(Amount, BankID_,TranDatetime,comment ,documentID);
        $('#DepositModal').modal('hide');
    });

    $(document).on('click', '.WithDrawAmount_Save', function () {

        var Amount = $('.WithDrawAmount').val() * -1;
        var BankID_ = BankID;
        if (Amount == "" || Amount == null) {
            showNotification("Enter Amount", "warning");
            return;
        }
        var comment = $("#inp_WithDrawComment").val();
        if (!comment) {
            if (!window.confirm("Do you want to save without comment?")) {
                return;
            }
        }
        var TranDatetime = $("#dtp_bank_withDrawDate").val();
        SaveAmount(Amount, BankID_,TranDatetime);

        $('#WithdrawModal').modal('hide');
    });

    $(document).on('change', '#ddl_bank_DocumentId', function () {
        if ($("#ddl_bank_DocumentId").val().length > 0) {
            $(".DepositAmount").val($("#ddl_bank_DocumentId").val());
            $(".DepositAmount").attr('disabled', true);
        }
        else {
            $(".DepositAmount").val("");
            $(".DepositAmount").attr('disabled', false);
        }
    });
}

function FillBankTable()
{
    $(".tblBankDetail_tblactive").empty();
    var myurl = "/Banks/GetBanksList";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        console.log(result);
        if (result.length != 0) {
            for (var i = 0; i < result.length; i++) {
                if (result[i].bankName != "Cash") {
                    $('#tblBankDetail').append('<tr><td class="text-center ">' + result[i].bankName + '</td><td class="text-center"><input type="button" class="btn btn-success Deposit" rel=' + result[i].bankID + ' style="width:22%;margin-right:5%" value="Deposit" /><input type="button" class="btn btn-success Withdraw" style="width:22%" rel=' + result[i].bankID + ' value="Withdraw" /> </td>');
                }
            }
        }
    });
}

function SaveAmount(Amount,BankID,TranDatetime,Comment,documentId)
{
    var data = { amount: Amount, bankID: BankID, transDate: TranDatetime, comment: Comment, documentId: documentId };
    var myurl = "/Banks/InsertBankTransfer";
        var mydata = new Object();
        mydata.param = JSON.stringify(data);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            FillBankTable();
            CashAmount();
            $("#inp_WithDrawComment").val("");
        }
    });
}

function CashAmount() {
    
    $('.CurrentCash').val("");
    var myurl = "/Banks/GetCurrentCash";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        $('.CurrentCash').val(result.amount)
    });
}



function GetOutStandingCheque() {
    var myurl = "/ChequeManagement/GetUnSubmitDocumentList";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('#ddl_bank_DocumentId').html('');
            for (var i = 0; i < result.length; i++) {
                if (i == 0) { $('#ddl_bank_DocumentId').append('<option></option>'); }
                $('#ddl_bank_DocumentId').append('<option value="' + result[i].chequeReceivedAmount + '">' + result[i].documentNo + '</option>');
            }
            $('#ddl_bank_DocumentId').val("select");
          
        }
        else { $('#ddl_bank_DocumentId').val(""); }
    });
}
function clearAll()
{
    $(".DepositAmount").val("");
    $("#inp_DepositComment").val("");
    $("#ddl_bank_DocumentId").empty();
    $(".DepositAmount").attr('disabled', false);
}
