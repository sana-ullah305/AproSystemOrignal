
function CustomerTrigger() {

    LoadCustomerTable();
    GetSalesPersonWithNewCustomerData();


    $("#btn_customer_AddNewCustomer").on("click", function () {
        ClearCustomer();
        $('#AddNewCustomerModal').modal('show'); $(".modal-title").text('Add New Customer');
        $("#dtp_customer_StartDate").val(moment().format('YYYY-MM-DD'));
    });

    $("#btn_customer_Save").on("click", function () {
        //    $("#btn_customer_Save").attr("disabled", true);
        SaveCustomer();
    });


    $(document).on('click', '.close', function () {
        ClearCustomer();
    });


    $(document).on('click', '#btn_customer_Delete', function () {

        var id = $(this).attr('rel') * 1;
        if (confirm("Are you sure to Delete ?")) {
            DeleteCustomer(id);
        }

    });
    $(document).on('click', '#btn_customer_Edit', function () {
        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            GetCustomerByID(id);
        }
    });

    $(document).on('click', '#btn_customer_Search', function () {
        LoadCustomerTable();
    });


}

function ClearCustomer() {
    $("#txt_customer_Id").val("");
    $('#txt_customer_LastName').val("");
    $('#txt_customer_FirstName').val("");
    $('#txt_customer_Phone').val("");
    $('#txt_customer_Email').val("");
    $('#txt_customer_OpeningBalance').val("");
    $('#dtp_customer_StartDate').val("");
    $('#txt_customer_cnic').val("");
    $('#txt_customer_ntn').val("");
    $(".dd_customer_sales_person").val('0');
    //$('#dtp_customer_DueDate').val("");
    $('#txt_customer_Misc').val("");
    $("#btn_customer_Save").attr("disabled", false);
}

function SaveCustomer() {
    if ($('#txt_customer_LastName').val() == null || $("#txt_customer_LastName").val().trim().length == 0) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter LastName"); return; }
    if ($('#txt_customer_FirstName').val() == null || $("#txt_customer_FirstName").val().trim().length == 0) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter FirsttName"); return; }
    if ($('#txt_customer_Phone').val() == null || $("#txt_customer_Phone").val().trim().length == 0) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter Phone"); return; }
    //if ($('#txt_customer_Email').val() == null || $("#txt_customer_Email").val().trim().length == 0) {
    //    $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter Email"); return;
    //   }
    //if ($('#txt_customer_OpeningBalance').val() == null || $("#txt_customer_OpeningBalance").val().trim().length == 0) { toastr.warning("Please Enter OpeningBalance"); return; }
    if ($('#dtp_customer_StartDate').val() == null || $("#dtp_customer_StartDate").val().trim().length == 0) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter StartDate"); return; }
    if ($('#txt_customer_cnic').val() == null || $("#txt_customer_cnic").val().trim().length == 0) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter CNIC."); return; }
    if ($('#dd_customer_sales_person').val() == "0" || $("#dd_customer_sales_person").val().trim().length == 0) { $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Select Sales Person"); return; }
    // if ($('#dtp_customer_DueDate').val() == null || $("#dtp_customer_DueDate").val().trim().length == 0) { toastr.warning("Please Enter DueDate"); return; }
    var re = /^[a-z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)?@[a-z][a-zA-Z-0-9]*\.[a-z]+(\.[a-z]+)?$/;
    var preemail = $('#txt_customer_Email').val().toLowerCase().trim();
    if (preemail.length > 0 && !re.test(preemail)) {
        $("#btn_customer_Save").attr("disabled", false); toastr.warning("Please Enter Correct Email"); return;
    }

    var obj = {
        id: $("#txt_customer_Id").val() * 1,
        lastName: $('#txt_customer_LastName').val(),
        firstName: $("#txt_customer_FirstName").val(),
        phone: $("#txt_customer_Phone").val(),
        cnic: $("#txt_customer_cnic").val(),
        email: $('#txt_customer_Email').val().toLowerCase().trim(),
        openingBalance: $('#txt_customer_OpeningBalance').val() * 1,
        startDate: $('#dtp_customer_StartDate').val(),
        salesPerson: $('#dd_customer_sales_person').val(),
        // dueDate: $('#dtp_customer_DueDate').val(),
        ntn: $("#txt_customer_ntn").val(),
        misc: $('#txt_customer_Misc').val()
    };
    ShowAjaxLoader();
    var myurl = "/Customer/SaveCustomer";
    var mydata = new Object();
    mydata.paramcustomer = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != "") {

            if (result != "Customer Already Exists") {
                HideAjaxLoader();
                showNotification("Saved", "success");
                ClearCustomer();
                $('#AddNewCustomerModal').modal('hide');
                oTable.ajax.reload(null, false);
            }
        }
        if (result == "Customer Already Exists") {
            HideAjaxLoader();
            toastr.warning(result);

        }
    });
}

function LoadCustomerTable() {
    oTable = $("#tblCustomerInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Customer/LoadCustomerTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'firstName', "className": 'text-compressed purchaseDate bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'lastname', "className": 'text-compressed invoiceNo bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },

           { "data": 'phone', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'email', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'cnic', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'salesPersonName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //  { "data": 'openingBalance', "className": 'text-compressed NumbersAlign bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
                //{ "data": 'balance', "className": 'text-compressed  bold', "bSortable": true, "orderSequence": ["desc", "asc"], },
            {
                "data": 'id', "className": 'text-compressed id bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.id + ' id="btn_customer_Edit" class="form-control" />'
                                            + ' <input type="button"  value="Delete" rel=' + obj.id + ' id="btn_customer_Delete" class="form-control" />';
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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_customer_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_customer_MainEndDate').val()).format("YYYY-MM-DD") });
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

function DeleteCustomer(_userId) {
    var myurl = "/Customer/DeleteCustomer";
    var mydata = new Object();
    mydata.customerID = _userId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadCustomerTable();

        }
        else { toastr.warning(result); return; }

    });
}

function GetCustomerByID(customerID) {
    var myurl = "/Customer/GetCustomerByID";
    var mydata = new Object();
    mydata.customerID = customerID;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {

            $('#AddNewCustomerModal').modal('show'); $(".modal-title").text('Edit Customer');
            $('#txt_customer_Id').val(result.id);
            $('#txt_customer_LastName').val(result.lastName);
            $('#txt_customer_FirstName').val(result.firstName);
            $('#txt_customer_Phone').val(result.phone);
            $('#txt_customer_Email').val(result.email);
            $('#txt_customer_cnic').val(result.cnic);
            $('#txt_customer_ntn').val(result.ntn);
            $('#dd_customer_sales_person').val(result.salesPerson);
            //$('#txt_customer_OpeningBalance').val(result.openingBalance);
            if (result.startDate != null && result.startDate != '')
            { $('#dtp_customer_StartDate').val(moment(result.startDate).format('YYYY-MM-DD')); }
            $('#txt_customer_Misc').val(result.misc);


        }

    });
}

$('#txt_customer_cnic').on('keydown', function (e) {
    var key = e.keyCode | e.which;
    if (key >= 48 && key <= 57 || key === 45 || key === 189 || key === 8 || key === 46) {
        if (key !== 8 && key !== 46) {
            var cnic = $(this).val().length; // get character length
            switch (cnic) {
                case 5:
                    var cnicVal = $(this).val();
                    var cnicNewVal = cnicVal + '-';
                    $(this).val(cnicNewVal);
                    break;
                case 13:
                    var cnicVal = $(this).val();
                    var cnicNewVal = cnicVal + '-';
                    $(this).val(cnicNewVal);
                    break;
                default:
                    break;
            }
        } return true;
    } else return false;
});


function GetSalesPersonWithNewCustomerData() {
    var myurl = "/Sales/GetSalesPersonList";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.dd_customer_sales_person').append(cloneDom);
        }
        //$('.dd_sell_sales_person').chosen();
        //$(".chosen-select").chosen({ width: '100%' });
    });
}