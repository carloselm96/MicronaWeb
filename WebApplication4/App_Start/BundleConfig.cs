using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebApplication4.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/bower_components/jquery/dist/jquery.min.js",
                        "~/Content/bower_components/jquery-ui/jquery-ui.min.js",
                        "~/Content/DataTables/datatables.min.js",
                        "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js",
                        "~/Content/bower_components/raphael/raphael.min.js",
                        "~/Content/bower_components/morris.js/morris.min.js",
                        "~/Content/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js",
                        "~/Content/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                        "~/Content/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                        "~/Content/bower_components/jquery-knob/dist/jquery.knob.min.js",
                        "~/Content/bower_components/moment/min/moment.min.js",
                        "~/Content/bower_components/bootstrap-daterangepicker/daterangepicker.js",
                        "~/Content/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                        "~/Content/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                        "~/Content/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/bower_components/fastclick/lib/fastclick.js",
                        "~/Content/dist/js/adminlte.min.js",
                        "~/Content/bower_components/Chart.js-2.7.2/dist/Chart.js",
                        "~/Content/dist/js/pages/dashboard.js",
                        "~/Content/dist/js/demo.js",
                        "~/Content/bower_components/Chart.js-2.7.2/dist/Chart.bundle.js",
                        "~/Content/ChartScript.js",
                        "~/Content/DataTables/Buttons-1.5.2/js/buttons.bootstrap.min.js",
                        "~/Content/DataTables/Buttons-1.5.2/js/buttons.html5.min.js",
                        "~/Content/DataTables/Buttons-1.5.2/js/dataTables.buttons.min.js",
                        "~/Content/ConfirmationBoostrap/bootstrap-confirmation.min.js",
                        "~/Content/bower_components/select2/dist/js/select2.full.min.js",
                        "~/Content/bower_components/select2/dist/js/select2.min.js",
                        "~/Content/site.js",
                        "~/Content/modalScript.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"
                        ));
            BundleTable.EnableOptimizations = false;

            /*
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));*/
        }
    }
}