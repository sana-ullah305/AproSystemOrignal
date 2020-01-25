var removedCount = -1;
var removedDates = [];
var listItem = new Array();
var dat_picker_count = -1;
var dropDownCutDta = "";
var shy = 0;
function SubscriptionTrigger() {

    GetCustomers(2);
    LoadSubscriptionTable();
    GetCustomersData();

    $("#btn_sub_AddNewSubscriber").on("click", function () {
        $('#AddNewSubscriberModal').modal('show');
        $('.date').datepicker({
            format: 'yyy/mm/dd',
            show: 'none',
            autoclose: true,

        });
        ClearSubCustomer();
        $("#dtp_sub_StartDate").val(moment().format('YYYY-MM-DD'));

    });

    //$("#ddl_sub_Subscriber").on("click", function () {
    //    SaveSubscriptionCustomer();
    //});

    $("#btn_sub_Save").on("click", function () {
        $("#btn_sub_Save").attr("disabled", true);
        SaveSubscriptionCustomer();

    });

    $("#btn_sub_EnableSubscription").on("click", function () {
        $("#btn_sub_Save").attr("disabled", true);
        

    });
    $("#btn_sub_disableSubscription").on("click", function () {
        $("#btn_sub_Save").attr("disabled", true);
      

    });

    $(document).on('click', '.close', function () {
        ClearSubCustomer();
    });

    $(document).on('click', '#btn_sub_PiadDues', function () {

        //var id = $(this).attr('custid') * 1;
        var id = $(this).attr('rel') * 1;
        if (id > 0) {
        if (confirm("Are you sure to Clear all Dues  ? ")) {
           // SaveSuspendedDuesofCustomer(id);
            ClearSuspendedDuesofCustomer(id);
            }
        }
    });
    $(document).on('click', '#btn_sub_Delete', function () {

        var custid = $(this).attr('rel') * 1;
        if (id > 0) {
            if (confirm("Are you sure to Delete  ? ")) {
                DeleteSubscriptionCustomer(id);
            }
        }
    });
    $(document).on('click', '#btn_sub_Edit', function () {
        $('#btn_AddCustomer').attr('disabled', true);
        $("#add_clone_date").attr("disabled", true);

        //$('.valDate').attr("disabled", true);
        $(".clone-date-wrapper").empty();
        dat_picker_count = -1;

        removedCount = -1;
        removedDates = [];
        listItem = [];
        $('#btn_AddCustomer').attr('disabled', true);
        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            GetSubCustByID(id);

        }
        if ($('#chk_sub_subStatus').prop('checked') == true) {
            $('.valDate').attr("disabled", false);
        }
    });
    $(document).on('click', '#btn_sub_ShowLog', function () {
        var id = $(this).attr('rel') * 1;
        ShowSubReminderLog(id);

    });

    $(document).on('click', '#btn_customer_Search', function () {
        LoadSubscriptionTable();
    });


}



function ClearSubCustomer() {


    $('#dd_sell_Customer').val("");
    $('#txt_sub_LastName').val("");
    $('#txt_sub_FirstName').val("");
    $('#txt_sub_Phone').val("");
    $('#txt_sub_Email').val("");
    $('#txt_sub_OpeningBalance').val("");
    $('#ddl_sub_DueDate').val("");
    $('#dtp_sub_StartDate').val("");
    $('#txt_sub_Comments').val("");
    $('#txt_sub_SubscriptionAmount').val("");
    $("#lbl_sub_subId").val("");
    $("#btn_sub_Save").attr("disabled", false);
    $('#chk_sub_subStatus').prop('checked', false);
    $('#dtp_sub_subDisableStartDate').val("");
    $('#dtp_sub_subDisableEndDate').val("");
    removedCount = -1;
    removedDates = [];
    listItem = [];
    dat_picker_count = -1
    $("#add_clone_date").attr("disabled", true);
    $('.valDate').attr("disabled", true);
    $(".clone-date-wrapper").empty();
    $('#btn_AddCustomer').attr('disabled', false);
}


function GetCustomers(typeID) {
    var myurl = "/Customer/GetCustomerList";
    var mydata = new Object();
    mydata.typeID = typeID;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result.length !== 0) {
            $('.ddl_sub_Subscriber').empty();
            $('#ddl_sub_Subscriber').val("select");
            for (var i = 0; i < result.length; i++) {
                $('.ddl_sub_Subscriber').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            $('#ddl_sub_Subscriber').val(0);
        }
    });
}


function SaveSubscriptionCustomer() {
    if ($('#txt_sub_LastName').val() == null || $("#txt_sub_LastName").val().trim().length == 0) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Please Enter LastName"); return; }
    if ($('#txt_sub_FirstName').val() == null || $("#txt_sub_FirstName").val().trim().length == 0) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Please Enter FirsttName"); return; }
    if ($('#txt_sub_Phone').val() == null || $("#txt_sub_Phone").val().trim().length == 0) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Please Enter Phone"); return; }
    if ($('#ddl_sub_DueDate').val() == null || $("#ddl_sub_DueDate").val().trim().length == 0) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Please Select Due Date"); return; }
    if ($('#dtp_sub_StartDate').val() == null || $("#dtp_sub_StartDate").val().trim().length == 0) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Please Select Start Date"); return; }

    if ($('#txt_sub_SubscriptionAmount').val() == null || $("#txt_sub_SubscriptionAmount").val().trim().length == 0) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Please Enter Amount"); return; }
  //  if ($('#chk_sub_subStatus').prop('checked') == true && (($('#dtp_sub_subDisableStartDate').val() == null) || ($('#dtp_sub_subDisableStartDate').val().trim().length == 0) || ($('#dtp_sub_subDisableEndDate').val() == null) || ($('#dtp_sub_subDisableEndDate').val().trim().length == 0))) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Enter Disable Start and End Date"); return; }
    //if ($('#chk_sub_subStatus').prop('checked') == true) { }
    //if (dat_picker_count > -1) {
    //    removedCount = 0;
    //    removedDates.sort();
    //    for (var i = 0; i <= dat_picker_count; i++) {
    //        if (removedDates[removedCount] != i) {
    //            if ($('#chk_sub_subStatus').prop('checked') == true && (($('#dtp_sub_subDisableStartDate_' + i).val() == null) || ($('#dtp_sub_subDisableStartDate_' + i).val() == "") || ($('#dtp_sub_subDisableStartDate_' + i).val().trim().length == 0) || ($('#dtp_sub_subDisableEndDate_' + i).val() == null) || ($('#dtp_sub_subDisableEndDate_' + i).val() == "") || ($('#dtp_sub_subDisableEndDate_' + i).val().trim().length == 0))) { $("#btn_sub_Save").attr("disabled", false); toastr.warning("Enter Start and End Date"); return; }
    //        }
    //        else {
    //            removedCount++;
    //        }
    //    }
    //}


    var re = /^[a-z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)?@[a-z][a-zA-Z-0-9]*\.[a-z]+(\.[a-z]+)?$/;
    var preemail = $('#txt_sub_Email').val().toLowerCase().trim();
    if (preemail.length > 0 && !re.test(preemail)) {
        $("#btn_sub_Save").attr("disabled", false); toastr.warning("Please Enter Correct Email"); return;
    }
    var _id = 0;
    if ($("#txt_sub_CustId").val() != "") {
        _id = $("#txt_sub_CustId").val();
    } else {
        _id = $("#lbl_sub_subId").val();
    }
    //if (dat_picker_count > -1) {
    //    removedCount = 0;
    //    listItem.push({ subDisableStartDate: moment($('#dtp_sub_subDisableStartDate').val()).format('YYYY-MM-DD'), subDisableEndDate: moment($('#dtp_sub_subDisableEndDate').val()).format('YYYY-MM-DD') });
    //    for (var i = 0; i <= dat_picker_count; i++) {
    //        if (removedDates[removedCount] != i) {
    //            if ($('#dtp_sub_subDisableStartDate_' + i).val() != "" && $('#dtp_sub_subDisableEndDate_' + i).val() != "") {
    //                listItem.push({ subDisableStartDate: moment($('#dtp_sub_subDisableStartDate_' + i).val()).format('YYYY-MM-DD'), subDisableEndDate: moment($('#dtp_sub_subDisableEndDate_' + i).val()).format('YYYY-MM-DD') });
    //            }
    //        }
    //        else {
    //            removedCount++;
    //        }

    //    }
    //}
    //else {
    //    if ($('#dtp_sub_subDisableStartDate').val() != "" && $('#dtp_sub_subDisableEndDate').val() != "") {
    //        listItem.push({ subDisableStartDate: moment($('#dtp_sub_subDisableStartDate').val()).format('YYYY-MM-DD'), subDisableEndDate: moment($('#dtp_sub_subDisableEndDate').val()).format('YYYY-MM-DD') });
    //    }
    //}
    var obj = {
        id: _id,
        lastName: $('#txt_sub_LastName').val(),
        firstName: $("#txt_sub_FirstName").val(),
        phone: $("#txt_sub_Phone").val(),
        email: $('#txt_sub_Email').val().toLowerCase().trim(),
        openingBalance: $('#txt_sub_OpeningBalance').val() * 1,
        startDate: $('#dtp_sub_StartDate').val(),
        dueDate: $('#ddl_sub_DueDate').val(),

        misc: $('#txt_sub_Comments').val(),
        subscriptionAmount: $('#txt_sub_SubscriptionAmount').val(),
        subStatus: $('#chk_sub_subStatus').prop('checked'),
        sed: listItem,
        // id:$("#lbl_sub_subId").val()*1
    };
    var myurl = ($("#lbl_sub_subId").val() * 1 == 0) ? "/SubscriptionManagment/SaveSubscriptionCustomer" : "/SubscriptionManagment/EditSubscriptionCustomer";
    var mydata = new Object();
    mydata.paramcustomer = JSON.stringify(obj); chk_sub_subStatus
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            ClearSubCustomer();
            $('#AddNewSubscriberModal').modal('hide');
            $(".clone-date-wrapper").empty();
            dat_picker_count = -1
            LoadSubscriptionTable();
        }
        if (result == "Customer Already Exists") {
            toastr.warning(result);
            $("#btn_sub_Save").attr("disabled", false);

        }
    });
}

function LoadSubscriptionTable() {
    oTable = $("#tblSubsciptionInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/SubscriptionManagment/LoadSubscriptionTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "order": [[3, "desc"]],
        "aoColumns": [
            { "data": 'firstName', "className": 'text-compressed purchaseDate bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'lastname', "className": 'text-compressed invoiceNo bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'phone', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'subscriptionAmount', "className": 'text-compressed NumbersAlign bold', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
            { "data": 'dueDate', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'startDate', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"],
                "mRender": function (data, type, obj) {
                    if (obj["startDate"] == null) {
                        return '<td class=" text-compressed bold"></td>'
                    }
                    else {
                        return '<td class=" text-compressed bold">' + moment(obj["startDate"]).format('YYYY-MM-DD') + '</td>'
                    }
                }
            },
             {
                 "data": 'lastReminderSentDate', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"],
                 "mRender": function (data, type, obj) {
                     if (data == null) {
                         return '<td class=" text-compressed bold"></td>'
                     }
                     else {
                         return '<td class=" text-compressed bold">' + moment(data).format('YYYY-MM-DD') + '</td>'
                     }
                 }
             },
               { "data": 'subscriptionStatus', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
          //{
          //    "data": 'subStatus', "className": 'text-compressed Active bold', "bSortable": false, "orderSequence": ["desc", "asc"],
          //    "render": function (full, type, obj) {
          //        var html2 = "";
          //        if (obj.subStatus == true) { html2 += '<input type="checkbox"  class="form-control" disabled checked />'; }
          //        else { html2 += '<input type="checkbox"  class="form-control " disabled />'; }
          //        return html2;
          //    }
          //},
            {
                "data": 'subId', "className": 'text-compressed subId bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.subId + ' id="btn_sub_Edit" class="form-control" />'
                        + ' <input type="button"  value="ShowLog" rel=' + obj.subId + ' id="btn_sub_ShowLog" class="form-control" />'
                   
                     + ' <input type="button"  value="Delete" rel=' + obj.subId + ' id="btn_sub_Delete" class="form-control" />'
                    if (obj.subscriptionStatus == "Suspended") { html2 += ' <input type="button"  value="Clear Dues" rel=' + obj.subId + ' id="btn_sub_PiadDues" class="form-control" />' }

                    
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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_sub_StartDateMain').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_sub_EndDateMain').val()).format("YYYY-MM-DD") });
            // aoData.push({ "name": "SearchType", "value": $('#ddl_customer_SearchBy option:selected').val() });


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




function DeleteSubscriptionCustomer(subcustomerID) {
    var myurl = "/SubscriptionManagment/DeleteSubscriptionCustomer";
    var mydata = new Object();
    mydata.subcustomerID = subcustomerID;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadSubscriptionTable();

        }
        else { toastr.warning(result); return; }

    });
}

function ClearSuspendedDuesofCustomer(SubCustomerId) {
    var myurl = "/SubscriptionManagment/SaveSuspendedDuesofCustomer";
    var mydata = new Object();
    mydata.SubCustomerId = SubCustomerId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("All Suspended Subscription Paid", "success");
            LoadSubscriptionTable();

        }
        else { toastr.warning(result); return; }

    });
}


function GetSubCustByID(_subCustId) {
    var myurl = "/SubscriptionManagment/GetSubCustByID";
    var mydata = new Object();
    mydata.subCustId = _subCustId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $("#txt_sub_CustId").val("")
            $('#AddNewSubscriberModal').modal('show');
            $('#lbl_sub_subId').val(result.subid);
            $('#txt_sub_LastName').val(result.lastName);
            $('#txt_sub_FirstName').val(result.firstName);
            $('#txt_sub_Phone').val(result.phone);
            $('#txt_sub_Email').val(result.email);
            // $('#txt_sub_OpeningBalance').val(result.openingBalance);
            $('#ddl_sub_DueDate').val(result.dueDate);
            $('#dtp_sub_StartDate').val(moment(result.startDate).format('YYYY-MM-DD'));
            $('#txt_sub_Comments').val(result.misc);
            $('#txt_sub_SubscriptionAmount').val(result.subscriptionAmount);
            $('#chk_sub_subStatus').prop('checked', result.subStatus);
            if (result.subStatus == true) {

                if (result.sed.length > 0) {
                    for (var i = 0; i < result.sed.length; i++) {
                        if (i == 0) {
                            $('#dtp_sub_subDisableStartDate').val(moment(result.sed[i].subDisableStartDate).format('YYYY-MM-DD'));
                            $('#dtp_sub_subDisableEndDate').val(moment(result.sed[i].subDisableEndDate).format('YYYY-MM-DD'));
                            if (($('#chk_sub_subStatus').prop('checked') == true && (($('#dtp_sub_subDisableStartDate').val() != null) || ($('#dtp_sub_subDisableStartDate').val() != "")) || ($('#dtp_sub_subDisableStartDate').val().trim().length != 0) && (($('#dtp_sub_subDisableEndDate').val() != null) || ($('#dtp_sub_subDisableEndDate').val() != "") || ($('#dtp_sub_subDisableEndDate').val().trim().length != 0)))) {
                                $('#add_clone_date').prop('disabled', false);
                            }
                        }

                        if (i > 0) {
                            var edit_cloned = '<div class="row" id="edit_clone_' + (i - 1) + '"><br /><div class="col-sm-3"></div><div class="col-sm-4"><div class="input-group date" id="startDatepicker_' + (i - 1) + '" data-provide="datepicker"><input type="text" class="form-control date valDate_clone" rel="' + (i - 1) + '" id="dtp_sub_subDisableStartDate_' + (i - 1) + '" readonly><div class="input-group-addon"><span class="glyphicon glyphicon-th"></span></div></div></div><div class="col-sm-4"><div class="input-group date" data-provide="datepicker" id="endDatepicker_' + (i - 1) + '"><input type="text" class="form-control date valDate_clone" rel="' + (i - 1) + '" id="dtp_sub_subDisableEndDate_' + (i - 1) + '" readonly><div class="input-group-addon"><span class="glyphicon glyphicon-th"></span></div></div> </div><div class="col-sm-1"><input type="button" class="btn btn-danger remove_clone_date" id="remove_edit_clone_date" value="-" rel="' + (i - 1) + '" style="margin-left: -73%;" /></div></div>';
                            $(edit_cloned).find('.date').datepicker({
                                format: "yyyy-mm-dd",
                                language: "en",
                                autoclose: true,
                                todayHighlight: true
                            });
                            $(".clone-date-wrapper").append(edit_cloned);
                            $('.date').datepicker({
                                format: "yyyy-mm-dd",
                                language: "en",
                                clearBtn: true,
                                todayHighlight: true,
                                close: true,
                            });
                            $('#dtp_sub_subDisableStartDate_' + (i - 1)).val(moment(result.sed[i].subDisableStartDate).format('YYYY-MM-DD'));
                            $('#dtp_sub_subDisableEndDate_' + (i - 1)).val(moment(result.sed[i].subDisableEndDate).format('YYYY-MM-DD'));
                            dat_picker_count = dat_picker_count + 1;
                            if (($('#chk_sub_subStatus').prop('checked') == true && (($('#dtp_sub_subDisableStartDate_' + (i - 1)).val() != null) || ($('#dtp_sub_subDisableStartDate_' + (i - 1)).val() != "")) || ($('#dtp_sub_subDisableStartDate_' + (i - 1)).val().trim().length != 0) && (($('#dtp_sub_subDisableEndDate_' + (i - 1)).val() != null) || ($('#dtp_sub_subDisableEndDate_' + (i - 1)).val() != "") || ($('#dtp_sub_subDisableEndDate_' + (i - 1)).val().trim().length != 0)))) {
                                $('#add_clone_date').prop('disabled', false);
                            }
                        }
                    }

                }
            }
            else {
                $('#dtp_sub_subDisableStartDate').val();
                $('#dtp_sub_subDisableEndDate').val();
            }

        }

    });
}

function GetCustomersData(select) {
    var myurl = "/Customer/GetCustomerList";
    var mydata = new Object();

    mydata.typeID = 1;
    XHRPOSTRequest(myurl, mydata, function (result) {
        console.log(result);
        dropDownCutDta = result;

        if (result.length !== 0) {
            $('#dd_sell_Customer').html('');
            $('#dd_sell_Customer').val("select");

            for (var i = 0; i < result.length; i++) {
                $('#dd_sell_Customer').append('<option value="' + result[i].Id + '">' + result[i].Name + '</option>');
            }
            $("#dd_sell_Customer").prepend("<option value='' selected='selected'></option>");

            if (shy != 0 || shy != "") {
                $("#dd_sell_Customer").val(shy);
                $("#dd_sell_Customer").trigger("change");
            }
        }
    });



}

$("#dd_sell_Customer").change(function () {

    for (var i = 0; i < dropDownCutDta.length; i++) {


        if (dropDownCutDta[i].Name == $("#dd_sell_Customer option:selected").text()) {

            $("#txt_sub_CustId").val(dropDownCutDta[i].Id);
            $("#txt_sub_LastName").val(dropDownCutDta[i].LastName);
            $("#txt_sub_FirstName").val(dropDownCutDta[i].FirstName);
            $("#txt_sub_Phone").val(dropDownCutDta[i].PhoneNo);
            $("#txt_sub_Email").val(dropDownCutDta[i].Email);
            // $("#dtp_sub_StartDate").val(moment(dropDownCutDta[i].StartDate, "MMM-dd-yyyy").format("YYYY-MM-DD"));


        }

        else if ($("#dd_sell_Customer").val() == "") {
            $("#txt_sub_CustId").val('');
            $("#txt_sub_LastName").val('');
            $("#txt_sub_FirstName").val('');
            $("#txt_sub_Phone").val('');
            $("#txt_sub_Email").val('');
            // $("#dtp_sub_StartDate").val('');
        }
    }


});

$('#btn_AddCustomer').click(function () {
    $('.txtCstFname').val('');
    $(".txtCstLname").val('');
    $(".txtCstPhone").val(''),
     $(".txtCstEmail").val(''),
    $("#NewCustomerModal").modal('show');
    $("#title_sales_customer").text("Add New Customer");
})


$("#btnSaveCustomer").click(function () {
    if ($('.txtCstFname').val().length == 0 || $('.txtCstLname').val().length == 0) { toastr.warning("Please Enter Customer Name"); return; }
    SaveNewCustomer();
    listItem = [];

})

function SaveNewCustomer() {
    var re = /^[a-z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)?@[a-z][a-zA-Z-0-9]*\.[a-z]+(\.[a-z]+)?$/;
    var preemail = $('.txtCstEmail ').val().toLowerCase().trim();
    if ((preemail.length > 0 && !re.test(preemail))) {
        toastr.warning("Please Enter Correct Email"); return;
    }
    var obj = {

        firstName: $('.txtCstFname').val(),
        lastName: $(".txtCstLname").val(),
        phone: $(".txtCstPhone").val(),
        email: $(".txtCstEmail").val(),

    };
    var myurl = "/Customer/SaveCustomer";
    var mydata = new Object();

    mydata.paramcustomer = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {

        if (result != "") {
            GetCustomersData(result);
            $("#NewCustomerModal").modal('hide');
            shy = result;


        }
        if (result == "Customer Already Exists") {
            toastr.warning(result);
        }
    });

}


function ShowSubReminderLog(id) {
    var myurl = "/SubscriptionManagment/GetReminderLog";
    var mydata = new Object();

    mydata.subCustId = id;
    XHRPOSTRequest(myurl, mydata, function (result) {
        var container = $("#reminderLogDiv");
        container.html("");
        if (result.length == 0) {
            swal("No Log for this customer");
            return;
        }
        for (var i = 0; i < result.length; i++) {
            var tmp = $("<div class='logrow'></div>");
            tmp.text(result[i].Log);
            container.append(tmp);

        }
        $("#ReminderLogModal").modal('show');
    });
}

$("#chk_sub_subStatus").change(function () {
    if (this.checked) {
        alert('Are You Sure to Disable Subscription ? ');
    }
    else { alert('Are You Sure to Enable Subscription ? '); }
    /*  if (this.checked) {
        $('.valDate').attr("disabled", false);
    }
    else {
        if ($('#lbl_sub_subId').val() > 0) {
            if (!confirm('Are you sure to remove all enter dates?')) {
                $('#chk_sub_subStatus').prop('checked', true);
                return false;
            }
            else {
                $("#add_clone_date").attr("disabled", true);
                $('.valDate').attr("disabled", true);
                $('#dtp_sub_subDisableStartDate').val("");
                $('#dtp_sub_subDisableEndDate').val("");
                $(".clone-date-wrapper").empty();
                dat_picker_count = -1;
            }
        }
        $("#add_clone_date").attr("disabled", true);
        $('.valDate').attr("disabled", true);
        $('#dtp_sub_subDisableStartDate').val("");
        $('#dtp_sub_subDisableEndDate').val("");
        $(".clone-date-wrapper").empty();
        dat_picker_count = -1;
    }*/
});

$("#add_clone_date").click(function () {
    if (dat_picker_count - removedCount != 4) {
        dat_picker_count = dat_picker_count + 1;
        var new_cloned = '<div class="row" id="clone_' + dat_picker_count + '"><br /><div class="col-sm-3"></div><div class="col-sm-4"><div class="input-group date" id="startDatepicker_' + dat_picker_count + '" data-provide="datepicker"><input type="text" class="form-control date valDate_clone" rel="' + dat_picker_count + '" id="dtp_sub_subDisableStartDate_' + dat_picker_count + '" readonly><div class="input-group-addon"><span class="glyphicon glyphicon-th"></span></div></div></div><div class="col-sm-4"><div class="input-group date" data-provide="datepicker" id="endDatepicker_' + dat_picker_count + '"><input type="text" class="form-control date valDate_clone" rel="' + dat_picker_count + '" id="dtp_sub_subDisableEndDate_' + dat_picker_count + '" readonly><div class="input-group-addon"><span class="glyphicon glyphicon-th"></span></div></div> </div><div class="col-sm-1"><input type="button" class="btn btn-danger remove_clone_date" id="remove_clone_date" rel="' + dat_picker_count + '" value="-" rel="' + dat_picker_count + '" style="margin-left: -73%;" /></div></div>';
        $(new_cloned).find('.date').datepicker({
            format: "yyyy-mm-dd",
            language: "en",
            autoclose: true,
            todayHighlight: true
        });

        $(".clone-date-wrapper").append(new_cloned);
        $('.date').datepicker({
            format: "yyyy-mm-dd",
            language: "en",
            autoclose: true,
            clearBtn: true,
            //todayBtn: true,
            todayHighlight: true,
            update: '',
            close: true,
            clear: 'glyphicon glyphicon-trash',
            close: 'glyphicon glyphicon-remove'
        });
        $('#dtp_sub_subDisableStartDate_' + dat_picker_count).val('');
        $('#dtp_sub_subDisableEndDate_' + dat_picker_count).val('');

        if (dat_picker_count > -1) {
            $("#add_clone_date").attr("disabled", true);
            $(".valDate_clone").change(function () {
                if ($('#dtp_sub_subDisableStartDate_' + $(this).attr('rel')).val() != "" && $('#dtp_sub_subDisableEndDate_' + $(this).attr('rel')).val() != "") {
                    //validation logic to see if the date is valid
                    $('#add_clone_date').attr("disabled", false);
                }
                else {
                    $("#add_clone_date").attr("disabled", true);
                }
            });
        }
    }
    else {
        alert('You have not allowed to add more then 5 dates.');
    }

});

$(".valDate").change(function () {
    if ($('#dtp_sub_subDisableStartDate').val() != "" && $('#dtp_sub_subDisableEndDate').val() != "") {
        //validation logic to see if the date is valid
        $('#add_clone_date').attr("disabled", false);
    }
    else {
        $("#add_clone_date").attr("disabled", true);
    }
});



$(document).on("click", "#remove_edit_clone_date", function () {
    removedCount++;
    if (confirm('Are you sure to remove the date.??') == true) {
        $('.clone-date-wrapper').find($('#edit_clone_' + $(this).attr('rel'))).remove();
        removedDates[removedCount] = $(this).attr('rel');
        if (($('#chk_sub_subStatus').prop('checked') == true && (($('#dtp_sub_subDisableStartDate').val() != null) || ($('#dtp_sub_subDisableStartDate').val() != "")) || ($('#dtp_sub_subDisableStartDate').val().trim().length != 0) && (($('#dtp_sub_subDisableEndDate').val() != null) || ($('#dtp_sub_subDisableEndDate').val() != "") || ($('#dtp_sub_subDisableEndDate').val().trim().length != 0)))) {
            $('#add_clone_date').prop('disabled', false);
        }
        if (($('#chk_sub_subStatus').prop('checked') == true && (($('#dtp_sub_subDisableStartDate').val() != null) || ($('#dtp_sub_subDisableStartDate').val() != "")) || ($('#dtp_sub_subDisableStartDate').val().trim().length != 0) && (($('#dtp_sub_subDisableEndDate').val() != null) || ($('#dtp_sub_subDisableEndDate').val() != "") || ($('#dtp_sub_subDisableEndDate').val().trim().length != 0)))) {
            $('#add_clone_date').prop('disabled', false);
        }
    }
});

$(document).on('click', '#remove_clone_date', function () {
    if (confirm('Are you sure to remove the date.??') == true) {
        $('.clone-date-wrapper').find($('#clone_' + $(this).attr('rel'))).remove();
        removedCount++;
        removedDates[removedCount] = $(this).attr('rel');
        if (($('#chk_sub_subStatus').prop('checked') == true && (($('#dtp_sub_subDisableStartDate').val() != null) || ($('#dtp_sub_subDisableStartDate').val() != "")) || ($('#dtp_sub_subDisableStartDate').val().trim().length != 0) && (($('#dtp_sub_subDisableEndDate').val() != null) || ($('#dtp_sub_subDisableEndDate').val() != "") || ($('#dtp_sub_subDisableEndDate').val().trim().length != 0)))) {
            $('#add_clone_date').prop('disabled', false);
        }

        for (var i = 0; i <= dat_picker_count; i++) {
            removedCount = 0;
            removedDates.sort();
            if (removedDates[removedCount] != i) {
                if ($('#chk_sub_subStatus').prop('checked') != true && (($('#dtp_sub_subDisableStartDate_' + i).val() != null) || ($('#dtp_sub_subDisableStartDate_' + i).val() != "") || ($('#dtp_sub_subDisableStartDate_' + i).val().trim().length != 0) || ($('#dtp_sub_subDisableEndDate_' + i).val() != null) || ($('#dtp_sub_subDisableEndDate_' + i).val() != "") || ($('#dtp_sub_subDisableEndDate_' + i).val().trim().length != 0))) {
                    $('#add_clone_date').prop('disabled', false);
                }
            }
            removedCount++;

        }

    }
});


