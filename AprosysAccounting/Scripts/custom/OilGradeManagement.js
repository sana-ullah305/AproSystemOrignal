function OilGradeManagementTrigger() {
    LoadOilGradeTable();
}



function LoadOilGradeTable() {
    oTable = $("#tblOilGradeInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/OilGradeManagement/LoadOilGradeTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": true,
        "bSortable": false,
       
        "aoColumns": [
            { "data": 'OilGade', "sWidth": "80%", "className": 'text-compressed text-center OilGade bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
             //{
             //    "data": 'IsActive', "className": 'text-compressed text-center IsActive bold', "bSortable": false,
             //    "render": function (full, type, obj) {
             //        if (full == "1") {
             //            return '<input type=\"checkbox\" id="isActiveCheckBox" class="isActiveCheckBox" checked value="' + full + '">';
             //        } else {
             //            return '<input type=\"checkbox\" id="isActiveCheckBox" class="isActiveCheckBox" value="' + full + '">';
             //        }
             //    }
             //},
            {
                "data": 'Id',"sWidth": "20%", "className": 'text-compressed text-center Id bold', "bSortable": false,
                "render": function (full, type, obj) {
                    var html2 = "";
                    html2 += ' <input type="button"  value="EDIT" rel=' + obj.Id + ' id="btn_oilGrade_Edit" class="btn btn-primary" />  |  '
                        + ' <input type="button"  value="Delete" rel=' + obj.Id + ' id="btn_oilGrade_Delete" class="btn btn-danger" />';
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
        order: [[0, "desc"]],
        order: [],

        "fnServerData": function (sSource, aoData, fnCallback) {
            aoData.push({ "name": "Start_Date", "value": moment(new Date()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment(new Date()).format("YYYY-MM-DD") });


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

$(document).on('click', '#add_new_oilGrade', function () {
    clear();
    $('#AddNewGradeRecordModal').modal('show');
});

$(document).on('click', '#btn_oilGrade_Delete', function () {
    if (confirm('Are you sure to Delete Oil Grade??'))
        DeleteGradeRecord($(this).attr('rel'));
});


$(document).on('click', '#btn_oilGrade_Edit', function () {
    clear();
    $('#AddNewGradeRecordModal').modal('show');
    $('.modal-title').text('Edit Oil Grade');
    $('#top_hdn_id').val($(this).attr('rel'));
    $('#txt_oilgrade').val($(this).closest("tr").find("td:first-child").text());
});

$(document).on('click', '#btn_oilgrade_Save', function () {
    if ($('#txt_oilgrade').val() == "" || $('#txt_oilgrade').val().trim().length == 0) { toastr.warning('Please Enter Grade'); return; }
    SaveGrade($('#top_hdn_id').val() != "" ? $('#top_hdn_id').val() : null, $('#txt_oilgrade').val());
});

function SaveGrade(Id, Grade) {
    var myurl = "/OilGradeManagement/SaveGrade";
    var mydata = new Object();
    mydata.Id = Id;
    mydata.Grade = Grade;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadOilGradeTable();
            $('#AddNewGradeRecordModal').modal('hide');

        }
        else { toastr.warning(result); return; }

    });
}


function DeleteGradeRecord(Id) {
    var myurl = "/OilGradeManagement/DeleteGradeRecord";
    var mydata = new Object();
    mydata.Id = Id;

    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {

            showNotification("Deleted", "success");
            LoadOilGradeTable();

        }
        else { toastr.warning(result); return; }

    });
}

function clear() {
    $('.modal-title').text('Add New Oil Grade');
    $('#txt_oilgrade').val('');
    $('#top_hdn_id').val('');
}