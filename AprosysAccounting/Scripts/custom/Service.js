
function ServiceTrigger() {
    LoadServiceTable();
    $("#btn_service_Add").on("click", function () {
        //if ($("#txt_service_Name").val() != null && $("#txt_service_Name").val().length > 0)
        //{ SaveService($("#txt_service_Name").val()); }
        Clear();
        $('#AddNewRevnueServiceModal').modal('show');
        $(".modal-title").text("Add New Service");
    });

    $(document).on('click', '#btn_service_Edit', function () {
        var id = $(this).attr('rel');
        if (id > 0) {
            GetServiceByID(id);
        }
    });
    $(document).on('click', '#btn_service_Delete', function () {

        var id = $(this).attr('rel');
        if (id > 0) {
         
            if (confirm("Are you sure to Delete")) {
                DeleteService(id);
            }
        }
    });
    $(document).on('click', '#btn_service_SaveService', function () {
        var name = $("#txt_mdlservice_name").val();
        var cost = $("#txt_mdlservice_cost").val() * 1;
        var code = $("#txt_mdlservice_code").val();
        if (name.length == 0) { toastr.warning("Please Enter Service Name"); return; }
        var id = $("#lbl_service_RevenueServiceId").text();
        if (id > 0)
        { EditRevenueSales(id, name, cost,code); }
        else
        { SaveService(name, cost,code); }
    });
}

function SaveService(name, cost, code) {
    var myurl = "/Service/SaveRevenueSales";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.name = name;
    mydata.cost = cost;
    mydata.code = code;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            $('#AddNewRevnueServiceModal').modal('hide');
            showNotification("Saved", "success");
            LoadServiceTable();

        }
        if (result == "Exists") {
            toastr.warning("Service Name Already Exists");
        }
        if (result == "Code Already Exists") {
            toastr.warning("Service Code Already Exists");
        }
    });
}


function LoadServiceTable() {
    oTable = $("#tblServiceInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/Service/LoadCustomerTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'id', "className": 'text-compressed TextAlignCenter bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
               { "data": 'code', "className": 'text-compressed TextAlignCenter bold', "bSortable": false, "orderSequence": ["desc", "asc"], },

            { "data": 'name', "className": 'text-compressed TextAlignCenter bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'cost', "className": 'text-compressed TextAlignCenter bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
              {
                  "data": 'id', "className": 'text-compressed InvoiceNo bold NumbersAlign', "bSortable": false,
                  "render": function (full, type, obj) {
                      var html2 = "";
                      html2 += ' <input type="button"  value="EDIT" rel=' + obj.id + ' id="btn_service_Edit" class="form-control" />'
                                              + ' <input type="button"  value="Delete" rel=' + obj.id + ' id="btn_service_Delete" class="form-control" />';
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



function GetServiceByID(id) {
    $("#lbl_service_RevenueServiceId").text(0);
    var myurl = "/Service/GetServiceByID";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.id = id;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {
            $('#AddNewRevnueServiceModal').modal('show'); $(".modal-title").text("Edit Service");
            $("#txt_mdlservice_name").val(result.name);
            $("#txt_mdlservice_cost").val(result.cost);
                $("#txt_mdlservice_code").val(result.code);
            $("#lbl_service_RevenueServiceId").text(result.id);
        }

    });
}

function DeleteService(id) {
    var myurl = "/Service/DeleteRevenueService";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);
    mydata.id = id;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Deleted", "success");

            LoadServiceTable();
        }
        else { toastr.warning(result); return; }

    });
}


function EditRevenueSales(id, name, cost,code) {
    var myurl = "/Service/EditRevenueSales";
    var mydata = new Object();
    //mydata.pvoucher = JSON.stringify(obj);

    mydata.id = id
    mydata.name = name;
    mydata.cost = cost;
    mydata.code = code;
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null && result != "Exists" && result != "Code Already Exists") {
            showNotification(result, "success");
            $('#AddNewRevnueServiceModal').modal('hide');
            LoadServiceTable();
            //  $("#txt_expense_Name").val("");
        }
        if (result == "Exists") {
            toastr.warning("Service Name Already Exists");
        }
        if (result == "Code Already Exists") {
            toastr.warning("Service Code Already Exists");
        }
    });
}

function Clear() {
    $("#txt_mdlservice_name").val("");
    $("#lbl_service_RevenueServiceId").text(0);
    $("#txt_mdlservice_cost").val("");
    $("#txt_mdlservice_code").val("");
}