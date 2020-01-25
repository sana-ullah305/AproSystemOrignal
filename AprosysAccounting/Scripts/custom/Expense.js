
function ExpenseTrigger() {
    LoadExpenseTable();
    $("#btn_expense_Add").on("click", function () {
        if ($("#txt_expense_Name").val() != null && $("#txt_expense_Name").val().length > 0)
        { SaveExpense($("#txt_expense_Name").val()); }
        else { toastr.warning("Please Enter Expense Name");}
    });


    $(document).on('click', '#btn_expense_Delete', function () {

        var id = $(this).attr('rel');
        if (confirm("Are you sure to Delete")) {
            DeleteAdministrativeExpense(id);
        }
    

    });
    $(document).on('click', '#btn_expense_Edit', function () {
        var id = $(this).attr('rel');
        GetExpenseByID(id);
    });
    $(document).on('click', '#btn_pvoucher_SaveExpense', function () {
        var name = $("#txt_pvoucher_AdministrativeExpense").val();
        id = $("#lbl_pvoucher_AdministrativeExpenseId").text();
        if (id > 0)
        { EditExpense(name,id); }
    });
}


function EditExpense(name,id) {
    var myurl = "/Expense/EDITAdministrativeExpense";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.name = name;
    mydata.id = id;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            $('#AddNewAdministrativeModal').modal('hide');
            LoadExpenseTable();
          //  $("#txt_expense_Name").val("");
        }
        if (result == "Expense Already Exists") {

            toastr.warning(result);

        }
    });
}


function SaveExpense(name) {
    var myurl = "/Expense/SaveAdministrativeExpense";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.name = name;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            LoadExpenseTable();
            $("#txt_expense_Name").val("");
        }
        if (result == "Expense Already Exists") {
            
            toastr.warning(result);

        }
    });
}


function LoadExpenseTable() {
    oTable = $("#tblExpenseInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Expense/LoadExpenseTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'id', "className": 'text-compressed TextAlignCenter bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'name', "className": 'text-compressed TextAlignCenter bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'id', "className": 'text-compressed InvoiceNo bold NumbersAlign', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.id + ' id="btn_expense_Edit" class="form-control" />'
                                            + ' <input type="button"  value="Delete" rel=' + obj.id + ' id="btn_expense_Delete" class="form-control" />';
                    //  html2 += '<input type="button"  value="Delete" rel=' + obj.InvoiceNo + ' id="btn_pvoucher_Delete" class="form-control" />';
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


function DeleteAdministrativeExpense(id) {
    var myurl = "/Expense/DeleteAdministrativeExpense";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.id = id;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Deleted", "success");

            LoadExpenseTable(); return;
        }
        if (result == "Expense Already Exists") {

            toastr.warning(result); return;

        }
        else { toastr.warning(result); return; }

    });
}



function GetExpenseByID(id) {
    $("#lbl_pvoucher_AdministrativeExpenseId").text(0);
    var myurl = "/Expense/GetExpenseByID";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.id = id;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $('#AddNewAdministrativeModal').modal('show');
            $("#txt_pvoucher_AdministrativeExpense").val(result);
            $("#lbl_pvoucher_AdministrativeExpenseId").text(id);
        }

    });
}
