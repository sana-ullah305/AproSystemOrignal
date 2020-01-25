function RepoConfigTrigger() {
    LoadReportConfig();
    $("#btnSave").click(function () {
        SaveRepoConfig();
    })
    $("#btnClear").click(function () {
        Clear();
    })
}

function Clear() {
    $("#txtTitle").val('');
    $("#txtAddress").val('');
    $("#txtDetailsTitle").val('');
    $("#txtDetails").val('');
}
function LoadReportConfig()
{
    Clear();
    var myurl = "/ReportConfig/GetReportConfig";
    var mydata = new Object();
    XHRPOSTRequest(myurl,mydata, function (result) {
        if(result!=null)
        {
            $("#txtTitle").val(result.title);
            $("#txtAddress").val(result.address);
            $("#txtDetailsTitle").val(result.detailTitle);
            $("#txtDetails").val(result.detail);
        }
    });
}
function SaveRepoConfig() {
    if ($("#txtTitle").val().trim().length == 0) { toastr.warning("Please Enter Title"); return; }
    if ($("#txtAddress").val().trim().length == 0) { toastr.warning("Please Enter Address"); return; }
    if ($("#txtDetailsTitle").val().trim().length == 0) { toastr.warning("Please Enter Detail Title"); return; }
    if ($("#txtDetails").val().trim().length == 0) { toastr.warning("Please Enter Details"); return; }
    var myurl = "/ReportConfig/SaveReportConfig";
    var mydata = new Object();
    var objx = {
        title: $("#txtTitle").val(),
        address: $("#txtAddress").val(),
        detailTitle: $("#txtDetailsTitle").val(),
        detail: $("#txtDetails").val(),

    };
    ShowAjaxLoader();
    mydata.repoParam = JSON.stringify(objx);
  
    XHRPOSTRequest(myurl, mydata, function (result) {
        if (result == "success") {
            showNotification("Saved", "success");
         //   Clear();
        }
        HideAjaxLoader();

    });
}