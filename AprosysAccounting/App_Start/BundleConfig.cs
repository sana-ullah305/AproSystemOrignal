using System.Web;
using System.Web.Optimization;

namespace AprosysAccounting
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/generics").Include(
                      //"~/Scripts/jquery-1.10.2.min.js",
                      //"~/Theme/js/bootstrap.min.js",
                      "~/Theme/js/plugins/metisMenu/jquery.metisMenu.js",
                      "~/Theme/js/plugins/slimscroll/jquery.slimscroll.min.js",
                      "~/Theme/js/plugins/flot/jquery.flot.js",
                      "~/Theme/js/plugins/flot/jquery.flot.tooltip.min.js",
                      "~/Theme/js/plugins/flot/jquery.flot.spline.js",
                      "~/Theme/js/plugins/flot/jquery.flot.resize.js",
                      "~/Theme/js/plugins/flot/jquery.flot.pie.js",
                      "~/Theme/js/plugins/peity/jquery.peity.min.js",
                      "~/Theme/js/demo/peity-demo.js",
                      "~/Theme/js/inspinia.js",
                      "~/Theme/js/plugins/pace/pace.min.js",
                      "~/Theme/js/plugins/jquery-ui/jquery-ui.min.js",
                      "~/Theme/js/plugins/gritter/jquery.gritter.min.js",
                      "~/Theme/js/plugins/sparkline/jquery.sparkline.min.js",
                      "~/Theme/js/demo/sparkline-demo.js",
                      "~/Theme/js/plugins/chartJs/Chart.min.js",
                      "~/Theme/js/plugins/fullcalendar/moment.min.js",
                      "~/Theme/js/plugins/dataTables/datatables.min.js",
                      "~/Theme/js/plugins/dataTables/dataTables.bootstrap.js",
                      "~/Theme/js/plugins/dataTables/dataTables.fixedHeader.min.js",
                      "~/Theme/js/plugins/dataTables/dataTables.responsive.js",
                      "~/Theme/js/plugins/dataTables/dataTables.tableTools.min.js",
                      "~/Theme/js/plugins/dataTables/jquery.dataTables.js",
                      "~/Theme/js/plugins/sweetalert/sweetalert.min.js",
                      "~/Theme/js/plugins/datapicker/bootstrap-datepicker.js",
                      "~/Theme/js/plugins/Datetimepickerv3/bootstrap-datetimepicker.js",
                      "~/Scripts/jquery.cookie.js",
                      "~/Scripts/custom/DownloadFile.js",
                      "~/Scripts/custom/GenericLib.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",

                      "~/Theme/css/plugins/toastr/toastr.min.css",
                      "~/Theme/js/plugins/gritter/jquery.gritter.css",
                      "~/Theme/css/plugins/jQueryUI/jquery-ui.css",

                      "~/css/plugins/dataTables/dataTables.bootstrap.css",
                      "~/css/plugins/dataTables/dataTables.tableTools.min.css",
                      "~/css/plugins/dataTables/dataTables.responsive.css",
                      "~/css/plugins/dataTables/fixedHeader.dataTables.css",
                      "~/Theme/css/plugins/sweetalert/sweetalert.css",
                      "~/Theme/css/plugins/datapicker/datepicker3.css",
                      "~/Theme/css/plugins/Datetimepickerv3/bootstrap-datetimepicker.min.css"));
        }
    }
}
