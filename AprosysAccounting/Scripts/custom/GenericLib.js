function ShowAjaxLoader() {
	if ($("#AjaxLoader").length == 0) {
        $("body").append('<div id="AjaxLoader" style="display: none"><div><span></span></div></div>');
    }
    $("#AjaxLoader").css("display", "block");
}

function HideAjaxLoader() {
    $("#AjaxLoader").css("display", "none");
}

$(document).ready(function(){
	window.onbeforeunload = function(event) {
		ShowAjaxLoader();
	};
	$(document).ajaxError(function (e,request) {
	    if (request.status == 403) {
	        window.alert("Session has expired. You will be redirected to login page");
	        window.location.href = "/Admin/Index?" + $.param({ "redirecturl": window.location.href });
	    } else {
	        window.alert("Please Reload Page , And contact Administrator"); HideAjaxLoader();
	    }
	   //console.log(e);
	});
	//SetSmallFooter();
	//$( window ).resize(function(){
	//	SetSmallFooter();
	//});
	//$.datepicker.setDefaults( $.datepicker.regional[ "it" ] );	
});

function SetSmallFooter(){
	$("#body").css("min-height","");
	var footerTop=$("footer").offset().top;
	var winHeight=$(window).height();
	if(footerTop<winHeight){
		var bdHeight=$("#body").height();
		bdHeight=bdHeight+(winHeight-footerTop)-40;
		$("#body").css("min-height",bdHeight+"px");
	}
	
}

function UpdateTableHeader(table) {
    var maxlength = [];
    $("tbody tr:first td", table).each(function (i) {
        maxlength[i] = $(this).width();
    });
    if (maxlength.length == 0) {
        return;
    }
    $("thead tr:first td", table).each(function (i) {
        $(this).css("display", "inlne-block")
        $(this).css("max-width", maxlength[i] + "px");
        $(this).css("width", maxlength[i] + "px");
    });

}

function ISHighLevelUser() {
    return $("#Role_ID").val()*1==2;
}

function StringToJSDate(val) {
    if (!val) {
        return null;
    }
    var dtto = val.split("/");
    if (dtto.length > 1) {
        dtto = dtto[2] + dtto[1] + dtto[0];
        val = dtto.substr(2);
        var y = val.substr(0, 2);
        var m = val.substr(2, 2);
        var d = val.substr(4, 2);
        return new Date("20" + y * 1, m * 1-1, d * 1);
    }
    return null;
}

function CurrencyFormat(number,_decimal)
{

    var decimalplaces = 2;
    if (_decimal!=undefined && _decimal!=null) {
        decimalplaces = _decimal;
    }
    var decimalcharacter = ".";
    var thousandseparater = ",";
    number = parseFloat(number);
    var sign = number < 0 ? "-" : "";
    var formatted = new String(number.toFixed(decimalplaces));
    if( decimalcharacter.length && decimalcharacter != "." ) { formatted = formatted.replace(/\./,decimalcharacter); }
    var integer = "";
    var fraction = "";
    var strnumber = new String(formatted);
    var dotpos = decimalcharacter.length ? strnumber.indexOf(decimalcharacter) : -1;
    if( dotpos > -1 )
    {
        if( dotpos ) { integer = strnumber.substr(0,dotpos); }
        fraction = strnumber.substr(dotpos+1);
    }
    else { integer = strnumber; }
    if( integer ) { integer = String(Math.abs(integer)); }
    while( fraction.length < decimalplaces ) { fraction += "0"; }
    temparray = new Array();
    while( integer.length > 3 )
    {
        temparray.unshift(integer.substr(-3));
        integer = integer.substr(0,integer.length-3);
    }
    temparray.unshift(integer);
    integer = temparray.join(thousandseparater);
    if (decimalplaces == 0) {
        return sign + integer
    } else {
        return "PKR "+ sign + integer + decimalcharacter + fraction;
    }
}


function PercentFormat(number) {

    var decimalplaces = 1;
  
    var decimalcharacter = ",";
    var thousandseparater = ".";
    number = parseFloat(number);
    var sign = number < 0 ? "-" : "";
    var formatted = new String(number.toFixed(decimalplaces));
    if (decimalcharacter.length && decimalcharacter != ".") { formatted = formatted.replace(/\./, decimalcharacter); }
    var integer = "";
    var fraction = "";
    var strnumber = new String(formatted);
    var dotpos = decimalcharacter.length ? strnumber.indexOf(decimalcharacter) : -1;
    if (dotpos > -1) {
        if (dotpos) { integer = strnumber.substr(0, dotpos); }
        fraction = strnumber.substr(dotpos + 1);
		
    }
    else {
        integer = strnumber;
        
    }
	if(fraction*1==0){
		decimalplaces=0;
	}
    if (integer) { integer = String(Math.abs(integer)); }
    while (fraction.length < decimalplaces) { fraction += "0"; }
    temparray = new Array();
    while (integer.length > 3) {
        temparray.unshift(integer.substr(-3));
        integer = integer.substr(0, integer.length - 3);
    }
    temparray.unshift(integer);
    integer = temparray.join(thousandseparater);
    if (decimalplaces == 0) {
        return sign + integer
    } else {
        return sign + integer + decimalcharacter + fraction;
    }
}


function FormatDateTimeToDisplay(date) {
  return  moment(date).format('YYYY-MM-DD hh:mm:ss A');

}


function GetJSDate(par) {
    return new Date(parseInt(par.replace(/\/Date\((-?\d+)\)\//, '$1')));
}


$(document).on('keypress', '.nonNum', function (e) {
    return NotAllowDecimalAndNegative(e);
})


function NotAllowDecimalAndNegative(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 189) {
        return false;
    }
    else if (charCode == 189 || charCode == 46 || (charCode > 31 && (charCode < 48 || charCode > 57))) {
        return false;
    }

}