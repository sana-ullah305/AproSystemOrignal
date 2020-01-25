
function VendorTrigger() {

    LoadVendorTable();


    $("#btn_vendor_AddNewVendor").on("click", function () {
        ClearVendor();
        $('#AddNewVendorModal').modal('show'); $(".modal-title").text("Add New Vendor")
      //  $("#dtp_customer_StartDate").val(moment().format('YYYY-MM-DD'));
    });

    $("#btn_vendor_Save").on("click", function () {
        $("#btn_vendor_Save").attr("disabled", true);
        SaveVendor();
    });


    $(document).on('click', '.close', function () {
        ClearVendor();
    });

    $(document).on('click', '#btn_vendor_Delete', function () {

        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            if (confirm("Are you sure to Delete ?")) {
                DeleteVendor(id);
            }
        }
    });
    $(document).on('click', '#btn_vendor_Edit', function () {
        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            GetVendorByID(id);
        }

    });

    $(document).on('click', '#btn_vendor_Search', function () {
        LoadVendorTable();
    });


}



function ClearVendor() {
    $('#txt_vendor_vendorId').val("");
    $('#txt_vendor_LastName').val("");
    $('#txt_vendor_FirstName').val("");
    $('#txt_vendor_Phone').val("");
    $('#txt_vendor_Email').val("");
    $('#txt_vendor_CreditLimit').val("");
    $('#txt_vendor_Balance').val("");
    $('#txt_vendor_Terms').val("");
    $('#txt_vendor_Misc').val("");
    $('#txt_vendor_cnic').val("");
    $('#txt_vendor_ntn').val("");
    $("#btn_vendor_Save").attr("disabled", false);
}



function SaveVendor() {
    if ($('#txt_vendor_LastName').val() == null || $("#txt_vendor_LastName").val().trim().length == 0) { $("#btn_vendor_Save").attr("disabled", false); toastr.warning("Please Enter LastName"); return; }
    if ($('#txt_vendor_FirstName').val() == null || $("#txt_vendor_FirstName").val().trim().length == 0) { $("#btn_vendor_Save").attr("disabled", false); toastr.warning("Please Enter FirsttName"); return; }
    if ($('#txt_vendor_Phone').val() == null || $("#txt_vendor_Phone").val().trim().length == 0) { $("#btn_vendor_Save").attr("disabled", false); toastr.warning("Please Enter Phone"); return; }
    //if ($('#txt_vendor_Email').val() == null || $("#txt_vendor_Email").val().trim().length == 0) { $("#btn_vendor_Save").attr("disabled", false);  toastr.warning("Please Enter EmailID"); return; }
   // if ($('#txt_vendor_CreditLimit').val() == null || $("#txt_vendor_CreditLimit").val().trim().length == 0) { $("#btn_vendor_Save").attr("disabled", false);  toastr.warning("Please Enter Credit Limit"); return; }
    //if ($('#txt_vendor_Balance').val() == null || $("#txt_vendor_Balance").val().trim().length == 0) { $("#btn_vendor_Save").attr("disabled", false);  toastr.warning("Please Enter Balance"); return; }
    //if ($('#txt_vendor_Terms').val() == null || $("#txt_vendor_Terms").val().trim().length == 0) { $("#btn_vendor_Save").attr("disabled", false); toastr.warning("Please Enter Terms"); return; }


    var re = /^[a-z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)?@[a-z][a-zA-Z-0-9]*\.[a-z]+(\.[a-z]+)?$/;
    var preemail = $('#txt_vendor_Email').val().toLowerCase().trim();
    if (preemail.length > 0 && !re.test(preemail)) {
        $("#btn_vendor_Save").attr("disabled", false); toastr.warning("Please Enter Correct Email"); return;
    }
    var obj = {
        id: $('#txt_vendor_vendorId').val()*1,
        lastName: $('#txt_vendor_LastName').val(),
        firstName: $("#txt_vendor_FirstName").val(),
        phone: $("#txt_vendor_Phone").val(),
        email:  $('#txt_vendor_Email').val().toLowerCase().trim(),
        creditLimit: $('#txt_vendor_CreditLimit').val() * 1,
        balance: $('#txt_vendor_Balance').val()*1,
        terms: $('#txt_vendor_Terms').val()*1,
        misc: $('#txt_vendor_Misc').val(),
        cnic: $('#txt_vendor_cnic').val(),
        ntn: $('#txt_vendor_ntn').val()

    };
    var myurl = "/Vendor/SaveVendor";
    var mydata = new Object();
    mydata.paramVendor = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            ClearVendor();
            $('#AddNewVendorModal').modal('hide');
            LoadVendorTable();
        }
        if (result == "Vendor Already Exists") {
            $("#btn_vendor_Save").attr("disabled", false);
            toastr.warning(result);

        }
    });
}

function LoadVendorTable() {
    oTable = $("#tblVendorInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Vendor/LoadVendorTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "aoColumns": [
            { "data": 'firstName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'lastname', "className": 'text-compressed  bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },

            { "data": 'phone', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'email', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'cnic', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{ "data": 'terms', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{ "data": 'creditLimit', "className": 'text-compressed NumbersAlign bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'balance', "className": 'text-compressed NumbersAlign  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'id', "className": 'text-compressed id bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.id + ' id="btn_vendor_Edit" class="form-control" />'
                                            + ' <input type="button"  value="Delete" rel=' + obj.id + ' id="btn_vendor_Delete" class="form-control" />';
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
      //  order: [[0, "desc"]],
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




function DeleteVendor(id) {
    var myurl = "/Vendor/DeleteVendor";
    var mydata = new Object();
    mydata.vendorID = id;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadVendorTable();

        }
        else { toastr.warning(result); return; }

    });
}

function GetVendorByID(_vendorId) {
    var myurl = "/Vendor/GetVendorByID";
    var mydata = new Object();
    mydata.vendorID = _vendorId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {

            $('#AddNewVendorModal').modal('show'); $(".modal-title").text("Edit Vendor");
            $('#txt_vendor_vendorId').val(result.id);
            $('#txt_vendor_LastName').val(result.lastName);
            $('#txt_vendor_FirstName').val(result.firstName);
            $('#txt_vendor_Phone').val(result.phone);
            $('#txt_vendor_Email').val(result.email);
            $('#txt_vendor_CreditLimit').val(result.creditLimit);
            $('#txt_vendor_Balance').val(result.balance);
            $('#txt_vendor_Terms').val(result.terms);
            $('#txt_vendor_Misc').val(result.misc);
            $('#txt_vendor_cnic').val(result.cnic);
            $('#txt_vendor_ntn').val(result.ntn);

        }

    });
}



$('#txt_vendor_cnic').on('keydown', function (e) {
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
