function outStandingChequeTrigger()
{
    $("#dtp_outStandingCheque_MainStartDate").val(moment().subtract('days', 7).format('YYYY-MM-DD'));
    $("#dtp_outStandingCheque_MainEndDate").val(moment().format('YYYY-MM-DD'));
    LoadOutStandingChequeTable();

    $("#btn_outStandingCheque_Search").on("click", function () {
        if ($("#dtp_outStandingCheque_MainStartDate").val() != "" && $("#dtp_outStandingCheque_MainEndDate").val() != "") {
            LoadOutStandingChequeTable();
        }
        else { toastr.warning("Please Select Dates"); }
    });
}



function LoadOutStandingChequeTable() {
    oTable = $("#tbloutStandingChequeInformation").DataTable({
        "bDestroy": true,
        "bAutoWidth": false,
        "bScrollCollapse": true,
        "bServerSide": true,
        "sAjaxSource": "/ChequeManagement/LoadOutStandingChequeTable",
        "bProcessing": true,
        "searchDelay": 1000,
        "scrollX": false,
        "bSortable": false,
        "aoColumns": [
            { "data": 'invoiceNo', "className": 'text-compressed  bold ', "bSortable": false, "orderSequence": ["desc", "asc"], },
            { "data": 'customerName', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },
            
            { "data": 'documentNo', "className": 'text-compressed  bold', "bSortable": false, "orderSequence": ["desc", "asc"], },


           {
               "data": 'chequeReceivedDate', "className": 'text-compressed bold NoWrap', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                   if (obj["chequeReceivedDate"] == null) { return '<td class=" text-compressed bold"></td>' }
                   else { return '<td class=" text-compressed bold">' + FormatDateTimeToDisplay(GetJSDate(obj["chequeReceivedDate"])) + '</td>' }
               }
           },
            {
                "data": 'chequeReceivedAmount', "className": 'text-compressed  bold NumbersAlign', "bSortable": false, "orderSequence": ["desc", "asc"], "mRender": function (data, type, obj) {
                    return CurrencyFormat(data);
                }
            },
          //{
          //    "data": 'invoiceNo', "className": 'text-compressed id bold', "bSortable": false,
          //    "render": function (full, type, obj) {

          //        //var obj_ = { InvoiveNo: obj.invoiceNo, customer: obj.customerName }
          //        //console.log(obj_);
          //        var html2 = "";
          //        html2 += ' <input type="button"  value="PAID" rel=' + obj.invoiceNo + ' cust="' + obj.customerName + '" custID=' + obj.customerID + ' amount=' + obj.netAmount + ' selldate="' + FormatDateTimeToDisplay(GetJSDate(obj.sellDate)) + '" id="btn_creditSales_Paid" class="form-control" />'
          //                                + ' <input type="button"  value="View" rel=' + obj.invoiceNo + ' cust="' + obj.customerName + '" id="btn_creditSales_View" class="form-control"/>'
          //                                 + ' <input type="button"  value="Print" rel=' + obj.invoiceNo + ' cust="' + obj.customerName + '" id="btn_creditSales_Print" class="form-control"/>'
          //                                  + ' <input type="button"  value="Download" rel=' + obj.invoiceNo + ' id="btn_creditsales_Download" class="form-control" />';
          //        return html2;
          //    }
          //},


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
            aoData.push({ "name": "Start_Date", "value": moment($('#dtp_outStandingCheque_MainStartDate').val()).format("YYYY-MM-DD") });
            aoData.push({ "name": "End_Date", "value": moment($('#dtp_outStandingCheque_MainEndDate').val()).format("YYYY-MM-DD") });
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
