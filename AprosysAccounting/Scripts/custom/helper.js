

function showNotification(_message, _type) {

    switch (_type) {
        case 'success':
            toastr.success(_message);
            break;
        case 'error':
            toastr.error(_message)
            break;
        case 'warning':
            toastr.warning(_message)
            break;
    }
}
function XHRPOSTRequestForFormData(_url, _data, _onsuccess) {
    return $.ajax({
        type: "POST",
        url: _url,
        processData: false,
        contentType: false,
        data: _data,
        dataType: "json",
        beforeSend: function () {
            // uiBlock();
        },
        success: _onsuccess,
        error: function (error) {
            //uiUnBlock();
            if (error.status == 403) {
               // RedirectLogin();
            }
        }
    });
}

function XHRGETRequest(_url, _data, _onsuccess) {
    $.ajax({
        type: "GET",
        url: _url,
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: _data,
        beforeSend: function () {
            // uiBlock();
        },
        success: _onsuccess,
        error: function (error) { uiUnBlock(); }
    });
}

function XHRPOSTRequest(_url, _data, _onsuccess) {
    return $.ajax({
        type: "POST",
        url: _url,
        data: _data,
        dataType: "json",
        beforeSend: function () {
            // uiBlock();
        },
        success: _onsuccess,
        error: function (error) {
            //uiUnBlock();
            if (error.status == 403) {
                //RedirectLogin();
            }
        }
    });
}

function XHRPOSTRequestAsync(_url, _data, _onsuccess) {
    $.ajax({
        type: "POST",
        url: _url,
        data: _data,
        dataType: "json",
        success: _onsuccess,
        error: function (error) {

            //  uiUnBlock();
            if (error.status == 403) {
                //RedirectLogin();

            }

        }
    });
}

function NotAllowDecimalAndNegative(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 189) {
        return false;
    }
    else if (charCode == 189 || charCode == 46 || (charCode > 31 && (charCode < 48 || charCode > 57))) {
        return false;
    }

}


function validateFloatKeyPress(el, evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    var number = el.value.split('.');
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    //just one dot (thanks ddlab)
    if (number.length > 1 && charCode == 46) {
        return false;
    }
    //get the carat position
    var caratPos = getSelectionStart(el);
    var dotPos = el.value.indexOf(".");
    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        return false;
    }
    return true;
}

function getSelectionStart(o) {
    if (o.createTextRange) {
        var r = document.selection.createRange().duplicate()
        r.moveEnd('character', o.value.length)
        if (r.text == '') return o.value.length
        return o.value.lastIndexOf(r.text)
    } else return o.selectionStart
}

function onlyAlphabet(event) {

   // var inputValue = event.charCode;
    var regex = new RegExp("^[a-zA-Z ]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }

    

       

}

function trim(el) {
    el.value = el.value.trim();
    return;
}

