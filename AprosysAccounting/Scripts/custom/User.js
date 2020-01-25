
function UserTrigger(GlobalPermission) {
    if (GlobalPermission.adminRights != true)
    {
        $('#btn_user_AddNewUser').attr("disabled", "disabled");
        //$("#btn_doc_Save").attr("disabled", "disabled");

    }
    LoadUserTable();


    $("#btn_user_AddNewUser").on("click", function () {
        ClearUser();
        $('#AddNewUserModal').modal('show'); $(".modal-title").text('Add New User');
    });

    $("#btn_user_Save").on("click", function () {
        SaveUser();
    });


    $(document).on('click', '.close', function () {
        ClearUser();
    });

    $(document).on('click', '#btn_user_Delete', function () {

        var id = $(this).attr('rel') * 1;
        if (id > 0) {
            if (confirm("Are you sure to Delete")) {
                DeleteUser(id);
            }
        }
    });
    $(document).on('click', '#btn_user_Edit', function () {
        var id = $(this).attr('rel') * 1;
        if (id!=null)
        { GetUserByID(id);}
    });

    //$(document).on('click', '#btn_user_Search', function () {
    //    LoadUserTable();
    //});


}

function ClearUser() {
    $("#txt_user_Id").val("");
    $('#txt_user_LastName').val("");
    $('#txt_user_FirstName').val("");
    $('#txt_user_UserId').val("");
    $('#txt_user_Password').val("");
    $('#txt_user_Phone').val("");
    $('#txt_user_Email').val("");
    $('#txt_user_Address').val("");
    $('#chk_user_isAdmin').prop('checked',false);

}

function SaveUser() {
    if ($('#txt_user_LastName').val() == null || $("#txt_user_LastName").val().trim().length == 0) { toastr.warning("Please Enter LastName"); return; }
    if ($('#txt_user_FirstName').val() == null || $("#txt_user_FirstName").val().trim().length == 0) { toastr.warning("Please Enter FirsttName"); return; }
    if ($('#txt_user_UserId').val() == null || $("#txt_user_UserId").val().trim().length == 0) { toastr.warning("Please Enter User Id"); return; }
    if ($('#txt_user_Password').val() == null || $("#txt_user_Password").val().trim().length == 0) { toastr.warning("Please Enter Password"); return; }

    var re = /^[a-z][a-zA-Z0-9_]*(\.[a-zA-Z][a-zA-Z0-9_]*)?@[a-z][a-zA-Z-0-9]*\.[a-z]+(\.[a-z]+)?$/;
    var preemail = $('#txt_user_Email').val().toLowerCase().trim();
    if (preemail.length > 0 && !re.test(preemail)) {
        $("#btn_user_Save").attr("disabled", false); toastr.warning("Please Enter Correct Email"); return;
    }

    var obj = {
        id:$("#txt_user_Id").val()*1,
        lastName: $('#txt_user_LastName').val(),
        firstName: $("#txt_user_FirstName").val(),
        userId: $("#txt_user_UserId").val(),
        password: $('#txt_user_Password').val(),
        phone: $('#txt_user_Phone').val(),
        email: $('#txt_user_Email').val(),
        address: $('#txt_user_Address').val(),
        adminRights: $('#chk_user_isAdmin').prop('checked')
    };
    var myurl = "/User/SaveUser";
    var mydata = new Object();
    mydata.paramuser = JSON.stringify(obj);
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
            ClearUser();
            $('#AddNewUserModal').modal('hide');
            LoadUserTable();
        }
        if (result == "UserID Already Exists") {
            toastr.warning(result);

        }
    });
}

function LoadUserTable() {
    oTable = $("#tblUserInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/User/LoadUserTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "sorting":false,
        "aoColumns": [
            { "data": 'firstName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'lastName', "className": 'bold ', "bSortable": false, "orderable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'userId', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'password', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"],
                "render": function (full, type, obj) {

                    var html2 = "";
                    //if (GlobalPermission.AdminRights == true) {
                    //    html2 += '<td class="text-center" >********</td>';
                        
                    //}
                    //else {
                        html2 += '<td class="text-center">********</td>';

                   // }

                    return html2;

                }
            },
          
            { "data": 'phone', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
              { "data": 'email', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            {
                "data": 'id', "className": 'text-compressed id bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    if (GlobalPermission.adminRights == true) {
                        html2 += ' <input type="button"  value="EDIT" rel=' + obj.id + ' id="btn_user_Edit" class="form-control" />'
                                                + ' <input type="button"  value="Delete" rel=' + obj.id + ' id="btn_user_Delete" class="form-control" />';
                    }
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
           // aoData.push({ "name": "Start_Date", "value": moment($('#dtp_user_MainStartDate').val()).format("MM-DD-YYYY") });
            //aoData.push({ "name": "End_Date", "value": moment($('#dtp_user_MainEndDate').val()).format("MM-DD-YYYY") });
            // aoData.push({ "name": "SearchType", "value": $('#ddl_item_SearchBy option:selected').val() });

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

function DeleteUser(_userId) {
    var myurl = "/User/DeleteUser";
    var mydata = new Object();
    mydata.id = _userId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadUserTable();

        }
        else { toastr.warning(result); return; }

    });
}

function GetUserByID(_userId) {
    var myurl = "/User/GetUserByID";
    var mydata = new Object();
    mydata._userId = _userId;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result != null) {

            $('#AddNewUserModal').modal('show'); $(".modal-title").text('Edit User');
            $("#txt_user_Id").val(result.id);
            $('#txt_user_LastName').val(result.lastName);
            $('#txt_user_FirstName').val(result.firstName);
            $('#txt_user_UserId').val(result.userId);
            $('#txt_user_Password').val(result.password);
            $('#txt_user_Phone').val(result.phone);
            $('#txt_user_Email').val(result.email);
            $('#txt_user_Address').val(result.address);
            $('#chk_user_isAdmin').prop('checked', result.adminRights);
          

        }

    });
}