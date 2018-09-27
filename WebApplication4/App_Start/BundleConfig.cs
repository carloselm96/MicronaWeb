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
            bundles.Add(new StyleBundle("~/bundles/styles").Include(            
                "~/Content/bower_components/Ionicons/css/ionicons.min.css",       
                "~/Content/bower_components/bootstrap-daterangepicker/daterangepicker.css",
                "~/Content/bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css",
                "~/Content/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css", 
                "~/Content/bower_components/select2/dist/css/select2.min.css",
                "~/Content/dist/css/AdminLTE.min.css",                
                "~/Content/dist/css/skins/_all-skins.min.css"
                ).Include("~/Content/bower_components/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/bower_components/jquery/dist/jquery.min.js",
                "~/Content/bower_components/jquery-ui/jquery-ui.min.js"
                ));

            
              bundles.Add(new ScriptBundle("~/bundles/bowercomponents").Include(
                "~/Content/bower_components/jquery/dist/jquery.min.js",
                "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js",
                "~/Content/bower_components/datatables.net/js/jquery.dataTables.min.js",
                "~/Content/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js",
                "~/Content/bower_components/fastclick/lib/fastclick.js",
                "~/Content/dist/js/adminlte.min.js",
                "~/Content/bower_components/jquery-sparkline/dist/jquery.sparkline.min.js",
                "~/Content/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                "~/Content/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                "~/Content/bower_components/select2/dist/js/select2.full.min.js",                
                "~/Content/plugins/input-mask/jquery.inputmask.js",
                "~/Content/plugins/input-mask/jquery.inputmask.date.extensions.js",
                "~/Content/plugins/input-mask/jquery.inputmask.extensions.js",                
                "~/Content/bower_components/moment/min/moment.min.js",
                "~/Content/bower_components/bootstrap-daterangepicker/daterangepicker.js",                
                "~/Content/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",                
                "~/Content/bower_components/bootstrap-colorpicker/dist/js/bootstrap-colorpicker.min.js",                
                "~/Content/plugins/timepicker/bootstrap-timepicker.min.js",
                "~/Content/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Content/bower_components/chart.js/Chart.js",
                "~/Content/sweetalert.min.js",
                "~/Content/plugins/iCheck/icheck.min.js",
                "~/Content/dist/js/pages/dashboard2.js",
                "~/Content/site.js",
                "~/Content/dist/js/demo.js"
                ));
        }
    }
}