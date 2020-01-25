
function SalesPersonManagementTrigger() {
    //$('#ddl_salesPerson_Customers').chosen();
    LoadSalesPersonTable();


    $("#btn_person_AddNewSalesPerson").on("click", function () {
        ClearSalesPerson();
        $('#chosen_add_new').show();
        GetCustomersList();
        $('#chosen_sales_person').hide();
        $('#AddNewSalesPersonModal').modal('show'); $(".modal-title").text('Add New Sales Person');
        $("#dtp_salesPerson_StartDate").val(moment().format('YYYY-MM-DD'));
    });

    $("#btn_salesPerson_Save").on("click", function () {
        //    $("#btn_salesPerson_Save").attr("disabled", true);
        SaveSalesPerson();
    });


    $(document).on('click', '.close', function () {
        ClearSalesPerson();
    });


    $(document).on('click', '#btn_salesPerson_Delete', function () {

        var id = $(this).attr('rel') * 1;
        if (confirm("Are you sure to Delete ?")) {
            DeleteSalesPerson(id);
        }

    });
    $(document).on('click', '#btn_salesPerson_Edit', function () {
        $('#chosen_add_new').hide();
        $('#chosen_sales_person').show();
        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            GetCustomersListBySalePersonID(id);
        }
    });

    $(document).on('click', '#btn_salesPerson_Search', function () {
        LoadSalesPersonTable();
    });


}

function ClearSalesPerson() {
    $("#txt_salesPerson_Id").val("");
    $('#txt_selesPerson_LastName').val("");
    $('#txt_selesPerson_FirstName').val("");
    $('#txt_salesPerson_Phone').val("");
    $('#txt_salesPerson_Email').val("");
    $('#txt_sales_person_cnic').val("");
    $('#txt_sales_person_ntn').val("");
    $('#txt_salesPerson_OpeningBalance').val("");
    $('#dtp_salesPerson_StartDate').val("");
    $('#openingBalance').val("");
    //$('#dtp_salesPerson_DueDate').val("");
    $('#txt_salesPerson_Misc').val("");
    $("#btn_salesPerson_Save").attr("disabled", false);
    //$("#ddlStatus").trigger("chosen:updated");
    $('#ddl_salesPerson_Customers').empty();
    $('#ddl_salesPerson_Customer_edit').empty();
    //$('.ddl_salesPerson_Customers').trigger("chosen:updated")
}

function SaveSalesPerson() {
    if ($('#txt_selesPerson_LastName').val() == null || $("#txt_selesPerson_LastName").val().trim().length == 0) { $("#btn_salesPerson_Save").attr("disabled", false); toastr.warning("Please Enter LastName"); return; }
    if ($('#txt_selesPerson_FirstName').val() == null || $("#txt_selesPerson_FirstName").val().trim().length == 0) { $("#btn_salesPerson_Save").attr("disabled", false); toastr.warning("Please Enter FirsttName"); return; }
    if ($('#txt_salesPerson_Phone').val() == null || $("#txt_salesPerson_Phone").val().trim().length == 0) { $("#btn_salesPerson_Save").attr("disabled", false); toastr.warning("Please Enter Phone"); return; }
    if ($('#txt_sales_person_cnic').val() == null || $("#txt_sales_person_cnic").val().trim().length == 0) { $("#txt_sales_person_cnic").attr("disabled", false); toastr.warning("Please Enter CNIC."); return; }
   
    //if ($('#txt_salesPerson_Email').val() == null || $("#txt_salesPerson_Email").val().trim().length == 0) {
    //    $("#btn_salesPerson_Save").attr("disabled", false); toastr.warning("Please Enter Email"); return;
    //   }
    //if ($('#txt_salesPerson_OpeningBalance').val() == null || $("#txt_salesPerson_OpeningBalance").val().trim().length == 0) { toastr.warning("Please Enter OpeningBalance"); return; }
    if ($('#dtp_salesPerson_StartDate').val() == null || $("#dtp_salesPerson_StartDate").val().trim().length == 0) { $("#btn_salesPerson_Save").attr("disabled", false); toastr.warning("Please Enter StartDate"); return; }
    // if ($('#dtp_salesPerson_DueDate').val() == null || $("#dtp_salesPerson_DueDate").val().trim().length == 0) { toastr.warning("Please Enter DueDate"); return; }
    var re = /^[a-z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)?@[a-z][a-zA-Z-0-9]*\.[a-z]+(\.[a-z]+)?$/;
    var preemail = $('#txt_salesPerson_Email').val().toLowerCase().trim();
    if (preemail.length > 0 && !re.test(preemail)) {
        $("#btn_salesPerson_Save").attr("disabled", false); toastr.warning("Please Enter Correct Email"); return;
    }
    var customerIds = $("#txt_salesPerson_Id").val() * 1 > 0 ? $('#ddl_salesPerson_Customer_edit').val() : $('#ddl_salesPerson_Customers').val();
    var obj = {
        Id: $("#txt_salesPerson_Id").val() * 1,
        lastName: $('#txt_selesPerson_LastName').val(),
        firstName: $("#txt_selesPerson_FirstName").val(),
        phone: $("#txt_salesPerson_Phone").val(),
        cnic: $("#txt_sales_person_cnic").val(),
        ntn: $("#txt_sales_person_ntn").val(),
        customersIDs: customerIds,
        email: $('#txt_salesPerson_Email').val().toLowerCase().trim(),
        openingBalance: $('#txt_salesPerson_OpeningBalance').val() * 1,
        startDate: $('#dtp_salesPerson_StartDate').val(),
        // dueDate: $('#dtp_salesPerson_DueDate').val(),
        misc: $('#txt_salesPerson_Misc').val()
    };
    ShowAjaxLoader();
    var myurl = "/SalesPersonManagement/SaveSalesPerson";
    var mydata = new Object();
    mydata.paramcustomer = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != "") {

            if (result != "Sales Person Already Exists") {
                HideAjaxLoader();
                showNotification("Saved", "success");
                ClearSalesPerson();
                $('#AddNewSalesPersonModal').modal('hide');
                LoadSalesPersonTable();
            }
        }
        if (result == "Sales Person Already Exists") {
            HideAjaxLoader();
            toastr.warning(result);

        }
    });
}

function LoadSalesPersonTable() {
    oTable = $("#tblSalePersonInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/SalesPersonManagement/LoadSalesPersonTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": true,
        "bSortable": false,
        "aoColumns": [
            { "data": 'firstName', "className": 'text-compressed purchaseDate bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'lastName', "className": 'text-compressed invoiceNo bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },

            { "data": 'phone', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'email', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{ "data": 'openingBalance', "className": 'text-compressed NumbersAlign bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'cnic', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{
            //    "data": 'startDate', "className": 'text-compressed startDate bold', "bSortable": true, "orderSequence": ["desc", "asc"],
            //    "mRender": function (data, type, obj) {
            //        if (obj["startDate"] == null) {
            //            return '<td class=" text-compressed bold"></td>'
            //        }
            //        else {
            //            return '<td class=" text-compressed bold">' + moment(obj["startDate"]).format('YYYY-MM-DD') + '</td>'
            //        }
            //    }
            //},
            //{
            //    "data": 'createdOn', "className": 'text-compressed startDate bold', "bSortable": true, "orderSequence": ["desc", "asc"],
            //    "mRender": function (data, type, obj) {
            //        if (obj["createdOn"] == null) {
            //            return '<td class=" text-compressed bold"></td>'
            //        }
            //        else {
            //            return '<td class=" text-compressed bold">' + moment(obj["createdOn"]).format('YYYY-MM-DD') + '</td>'
            //        }
            //    }
            //} ,
            //{ "data": 'createdBy', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{ "data": 'isActive', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{
            //    "data": 'modifiedOn', "className": 'text-compressed startDate bold', "bSortable": true, "orderSequence": ["desc", "asc"],
            //    "mRender": function (data, type, obj) {
            //        if (obj["modifiedOn"] == null) {
            //            return '<td class=" text-compressed bold"></td>'
            //        }
            //        else {
            //            return '<td class=" text-compressed bold">' + moment(obj["modifiedOn"]).format('YYYY-MM-DD') + '</td>'
            //        }
            //    }
            //},
            //{ "data": 'modifiedBy', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            //{ "data": 'misc', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'id', "className": 'text-compressed id bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.Id + ' id="btn_salesPerson_Edit" class="form-control" />'
                        + ' <input type="button"  value="Delete" rel=' + obj.Id + ' id="btn_salesPerson_Delete" class="form-control" />';
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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_saleperson_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_saleperson_MainEndDate').val()).format("YYYY-MM-DD") });
            // aoData.push({ "name": "SearchType", "value": $('#ddl_salesPerson_SearchBy option:selected').val() });


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

function DeleteSalesPerson(_userId) {
    var myurl = "/SalesPersonManagement/DeleteSalesPerson";
    var mydata = new Object();
    mydata.salesPersonID = _userId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadSalesPersonTable();

        }
        else { toastr.warning(result); return; }

    });
}

function GetSalesPersonByID(salePersonID) {
    var myurl = "/SalesPersonManagement/GetSalesPersonByID";
    var mydata = new Object();
    mydata.salePersonID = salePersonID;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $('#AddNewSalesPersonModal').modal('show'); $(".modal-title").text('Edit Sales Person');
            $('#txt_salesPerson_Id').val(result.Id);
            $('#txt_selesPerson_LastName').val(result.lastName);
            $('#txt_selesPerson_FirstName').val(result.firstName);
            $('#txt_salesPerson_Phone').val(result.phone);
            $('#txt_salesPerson_Email').val(result.email);
            $('#txt_sales_person_cnic').val(result.cnic);
            $('#txt_sales_person_ntn').val(result.ntn);
            if (result.customersIDs.length !== 0) {
                var customersIDs = [];
                for (var i = 0; i < result.customersIDs.length; i++) {
                    customersIDs.push(result.customersIDs[i].toString());
                }
                if (customersIDs.length > 0)
                    $('#ddl_salesPerson_Customer_edit').val(customersIDs).trigger('chosen:updated');
            }
            //$('#txt_salesPerson_OpeningBalance').val(result.openingBalance);
            if (result.startDate != null && result.startDate != '') { $('#dtp_salesPerson_StartDate').val(moment(result.startDate).format('YYYY-MM-DD')); }
            $('#txt_salesPerson_Misc').val(result.misc);


        }

    });
}
$('#txt_sales_person_cnic').on('keydown', function (e) {
    var key = e.keyCode | e.which;
    if (key >= 48 && key <= 57 || key === 45 || key === 8 || key === 46) {
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
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
function GetCustomersList() {
    var myurl = "/SalesPersonManagement/GetCustomersList";
    var mydata = new Object();
    var dom = '<option value=[ID]>[Name]</option>';
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.ddl_salesPerson_Customers').append(cloneDom);
        }
        $('.ddl_salesPerson_Customers').chosen();
        $(".chosen-select").chosen({ width: '100%' });
        $('#ddl_salesPerson_Customers').val('').trigger("chosen:updated");
    });
}


function GetCustomersListBySalePersonID(salePersonID) {
    var myurl = "/SalesPersonManagement/GetCustomersListBySalePersonID";
    var mydata = new Object();
    mydata.salePersonID = salePersonID;
    var dom = '<option value=[ID]>[Name]</option>';
    ClearSalesPerson();
    XHRPOSTRequest(myurl, mydata, function (result) {
        for (var i = 0; i < result.length; i++) {
            var cloneDom = dom;
            cloneDom = cloneDom.replace('[ID]', result[i].Key);
            cloneDom = cloneDom.replace('[Name]', result[i].Value);
            $('.ddl_salesPerson_Customer_edit').append(cloneDom);
        }
        $('.ddl_salesPerson_Customer_edit').chosen();
        $(".chosen-select").chosen({ width: '100%' });
        $('#ddl_salesPerson_Customer_edit').val('').trigger("chosen:updated");
        GetSalesPersonByID(salePersonID);
    });
}