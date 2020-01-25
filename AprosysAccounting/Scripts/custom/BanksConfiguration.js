function InitBankConfiguration() {
    FillBankTable();
    $(document).on('click', '.btnedit', function () {
        $('#EditAccountModal .txtAccountName').val($(this).closest('tr').find('.bankName').text());
        $('#EditAccountModal .txtAccountName').attr('rel', $(this).attr('rel'));
        $('#EditAccountModal').modal('show');
    });

    $(document).on('click', '.btnDelete', function () {
        if (confirm("Are you sure to Delete ?"))
            deleteBankAccount($(this).attr('rel'));
    });
    $('#btn_add_newBankAccount').on('click', function () {
        clearAddNewBankModelControls();
    });

    $('#btn_bank_SaveAccount').click(function () {
        saveBankAccountDetails();
    });
    function clearAddNewBankModelControls() {
        $("#txt_mdlbank_name").val('');
        $("#txt_mdlaccount_no").val('');
        $("#txt_mdlaccount_deposit").val('');
        $('#AddNewBankAccountModal').modal('show');
    }
    function saveBankAccountDetails() {
        //if ($("#txt_mdlbank_name").val().trim().length == 0) { alert('Bank name cannot be empty'); return; }
        if ($("#txt_mdlaccount_no").val().trim().length == 0) { alert('Bank account no cannot be empty'); return; }
        //if ($("#txt_mdlaccount_deposit").val().trim().length == 0) { alert('Bank deposit cannot be empty'); return; }
        var myurl = "/BanksConfiguration/AddNewBankAccoount";
        var mydata = new Object();
        mydata.bankName = $("#txt_mdlbank_name").val();
        mydata.bankAccount = $("#txt_mdlaccount_no").val();
        mydata.depositAmmount = $("#txt_mdlaccount_deposit").val();
        XHRPOSTRequest(myurl, mydata, function (result) {
            if (result == "success") {
                FillBankTable();
                $('#AddNewBankAccountModal').modal('hide');
                toastr.success('saved');
            }
            else {
                toastr.warning(result);
            }
        });

    }
    $(".btnRenameBank").click(function () {
        if ($(".txtAccountName").val().trim().length == 0) { alert('Bank name cannot be empty'); return; }
        var myurl = "/BanksConfiguration/RenameBank";
        var mydata = new Object();
        mydata.bankID = $(".txtAccountName").attr('rel');
        mydata.name = $(".txtAccountName").val();
        XHRPOSTRequest(myurl, mydata, function (result) {
            if (result == "success") {
                FillBankTable();
                $('#EditAccountModal').modal('hide');
                toastr.success('saved');
            }

        });
    })
}


function FillBankTable() {
    $(".tblBankDetail_tblactive").empty();
    var myurl = "/Banks/GetBanksList";
    var mydata = new Object();
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length != 0) {
            $('.tblBankDetail_tblactive').html('');
            for (var i = 0; i < result.length; i++) {
                if (result[i].bankID != 11) {
                    $('.tblBankDetail_tblactive').append('<tr><td class="text-center bankName ">' + result[i].bankName + '</td><td class="text-center"> '
                        + '<input type="button" class="btn btn-success btnedit" rel=' + result[i].bankID + ' style="width:12%;margin-right:5%" value="Edit" />'
                        + '<input type="button" class="btn btn-danger btnDelete" rel=' + result[i].bankID + ' style="" value="Delete" />'
                        + '</td>');
                }
            }
        }
    });
}


function deleteBankAccount(bankID) {
    var myurl = "/BanksConfiguration/DeleteBankAccount";
    var mydata = new Object();
    mydata.CoaId = bankID;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            FillBankTable();
            $('#EditAccountModal').modal('hide');
            toastr.success('saved');
        }
        else { toastr.warning(result); }

    });
}