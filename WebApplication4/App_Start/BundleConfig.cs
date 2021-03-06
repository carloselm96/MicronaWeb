﻿using System;
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
                "~/Content/bower_components/select2/dist/css/select2.min.css",
                "~/Content/dist/css/AdminLTE.min.css",                
                "~/Content/dist/css/skins/_all-skins.min.css"
                ).Include("~/Content/bower_components/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/bower_components/jquery/dist/jquery.min.js",
                "~/Content/bower_components/jquery-ui/jquery-ui.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/loginquery").Include(
                "~/Content/Login_v15/vendor/jquery/jquery-3.2.1.min.js",
                "~/Content/Login_v15/vendor/animsition/js/animsition.min.js",
                "~/Content/Login_v15/vendor/bootstrap/js/popper.js",
                "~/Content/Login_v15/vendor/bootstrap/js/bootstrap.min.js",
                "~/Content/Login_v15/vendor/select2/select2.min.js",
                "~/Content/Login_v15/vendor/daterangepicker/moment.min.js",
                "~/Content/Login_v15/vendor/daterangepicker/daterangepicker.js",
                "~/Content/Login_v15/vendor/countdowntime/countdowntime.js",
                "~/Content/Login_v15/js/main.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/login").Include(
                "~/Content/Login_v15/vendor/bootstrap/css/bootstrap.min.css",
                "~/Content/Login_v15/vendor/animate/animate.css",
                "~/Content/Login_v15/vendor/css-hamburgers/hamburgers.min.css",
                "~/Content/Login_v15/vendor/animsition/css/animsition.min.css",
                "~/Content/Login_v15/vendor/select2/select2.min.css",
                "~/Content/Login_v15/vendor/daterangepicker/daterangepicker.css",
                "~/Content/Login_v15/css/util.css",
                "~/Content/Login_v15/css/main.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/editorstyle").Include(
                "~/Conent/AdminLTE/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/editorscript").Include(
                "~/Conent/AdminLTE/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bowercomponents").Include(
                "~/Content/bower_components/jquery/dist/jquery.min.js",
                "~/Content/bower_components/bootstrap/dist/js/bootstrap.min.js",
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
                "~/Content/dist/js/demo.js",
                "~/Scripts/form-format1.js"
                ));
        }
    }
}