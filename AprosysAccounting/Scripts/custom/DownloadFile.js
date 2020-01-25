
var downloadCallback = null;
function checktime(time) {
    if (time < 10)
        return "0" + time;
    return time;
}

function DownloadFiles(Url, data,callback) {

    if (callback) {
        downloadCallback = callback;
    }
    if ($("#DownloadHidden").length == 0) {
        $("body").append('<div id="DownloadHidden" style="display:none"></div>');
    }
    $("#DownloadHidden").html("");
    ShowAjaxLoader();
    var handleload = true; ;
    var downloadtimer = null;
    $("<iframe id='downloadiframe' />").load(function () {
       
        if (!handleload) {
            if (downloadtimer != null) {
                
                HideAjaxLoader();
                clearTimeout(downloadtimer);
                if (downloadCallback) {
                    downloadCallback();
                }
            }
            return;
        }

        var d = new Date();
        var fbody = $("#downloadiframe").contents().find('body');
        var cv = checktime(d.getHours()) + checktime(d.getMinutes()) + checktime(d.getSeconds());
        var downloadurl = Url + "cv=" + cv;
        var frmstr = "<form  action='" + downloadurl + "' method='post'>";
        for (var keys in data) {
            frmstr += "<input id='" + keys + "' name='" + keys + "' type='hidden' />";
        }
        frmstr += "</form>";
        fbody.append(frmstr).ready(function () {
            for (var keys in data) {
                fbody.find("#" + keys).val(data[keys]);
            }
            fbody.find('form').submit()
            handleload = false;

        });
        downloadtimer = setTimeout("CheckDownloadeded('" + cv + "',1)", 1000);

    }).appendTo("#DownloadHidden");
}


function CheckDownloadeded(val, iter) {
    if ($.cookie("ava_cv") == val) {
        HideAjaxLoader();
        if (downloadCallback) {
            downloadCallback();
        }
        return;
    }
    iter = iter + 1;
    if (iter < 200)
        setTimeout("CheckDownloadeded('" + val + "'," + iter + ")", 1000);
    else {
        HideAjaxLoader();
        if (downloadCallback) {
            downloadCallback();
        }
    }
}