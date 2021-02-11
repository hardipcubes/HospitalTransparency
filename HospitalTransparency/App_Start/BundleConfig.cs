using System.Web;
using System.Web.Optimization;

namespace HospitalTransparency
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.UseCdn = true;

            //var jqueryCdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js";

            //bundles.Add(new ScriptBundle("~/bundles/jquery",
            //            jqueryCdnPath).Include(
            //            "~/js/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/js/jquery.js",
            //          "~/Content/bs3/js/bootstrap.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //         "~/js/jquery-ui/jquery-ui-1.10.1.custom.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/accordion").Include(
            //         "~/js/jquery.dcjqaccordion.2.7.js"));

            //bundles.Add(new ScriptBundle("~/bundles/scroll").Include(
            //         "~/js/jquery.scrollTo.min.js",
            //         "~/js/jQuery-slimScroll-1.3.0/jquery.slimscroll.js",
            //         "~/js/jquery.nicescroll.js",
            //         "~/js/jquery.scrollTo/jquery.scrollTo.js"));

            //bundles.Add(new ScriptBundle("~/bundles/skycons").Include(
            //         "~/js/skycons/skycons.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryeasing", "http://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/clndr").Include(
            //         "~/js/calendar/clndr.js",
            //         "~/js/calendar/moment-2.2.1.js",
            //         "~/js/evnt.calendar.init.js"));

            //bundles.Add(new ScriptBundle("~/bundles/underscore", "http://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.5.2/underscore-min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jvectormap").Include(
            //         "~/js/jvector-map/jquery-jvectormap-1.2.2.min.js",
            //         "~/js/jvector-map/jquery-jvectormap-us-lcc-en.js"));

            //bundles.Add(new ScriptBundle("~/bundles/gauge").Include(
            //         "~/js/gauge/gauge.js"));

            //bundles.Add(new ScriptBundle("~/bundles/css3clock").Include(
            //         "~/js/css3clock/js/css3clock.js"));

            //bundles.Add(new ScriptBundle("~/bundles/easypiechart").Include(
            //         "~/js/easypiechart/jquery.easypiechart.js"));

            //bundles.Add(new ScriptBundle("~/bundles/sparklinechart").Include(
            //         "~/js/sparkline/jquery.sparkline.js"));

            //bundles.Add(new ScriptBundle("~/bundles/morrisChart").Include(
            //         "~/js/morris-chart/morris.js",
            //         "~/js/morris-chart/raphael-min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/flotChart").Include(
            //         "~/js/flot-chart/jquery.flot.js",
            //         "~/js/flot-chart/jquery.flot.tooltip.min.js",
            //         "~/js/flot-chart/jquery.flot.resize.js",
            //         "~/js/flot-chart/jquery.flot.pie.resize.js"));

            //bundles.Add(new ScriptBundle("~/bundles/flotChartag").Include(
            //         "~/js/flot-chart/jquery.flot.animator.min.js",
            //         "~/js/flot-chart/jquery.flot.growraf.js"));

            //bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
            //         "~/js/dashboard.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jQuerycustomSelect").Include(
            //         "~/js/jquery.customSelect.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/commonScript").Include(
            //         "~/js/scripts.js"));

            //bundles.Add(new ScriptBundle("~/bundles/validate").Include(
            //         "~/js/jquery.validate.min.js",
            //         "~/js/validation-init.js"));

            //bundles.Add(new ScriptBundle("~/bundles/icheck").Include(
            //         "~/js/iCheck/jquery.icheck.js"));

            //bundles.Add(new ScriptBundle("~/bundles/icheckini").Include(
            //         "~/js/icheck-init.js"));

            //bundles.Add(new ScriptBundle("~/bundles/wysihtml").Include(
            //         "~/js/bootstrap-wysihtml5/wysihtml5-0.3.0.js",
            //         "~/js/bootstrap-wysihtml5/bootstrap-wysihtml5.js"));

            //bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
            //         "~/js/advanced-datatable/js/jquery.dataTables.js",
            //         "~/js/data-tables/DT_bootstrap.js"));

            //bundles.Add(new ScriptBundle("~/bundles/tableIni").Include(
            //         "~/js/dynamic_table_init.js"));

            ////Styles Start

            //bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
            //          "~/Content/bs3/css/bootstrap.min.css",
            //          "~/Content/css/bootstrap-reset.css"));

            //bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
            //          "~/Content/font-awesome/css/font-awesome.css"));

            //bundles.Add(new StyleBundle("~/Content/style").Include(
            //          "~/Content/css/style.css",
            //          "~/Content/css/style-responsive.css"));

            //bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
            //          "~/js/jquery-ui/jquery-ui-1.10.1.custom.min.css"));

            //bundles.Add(new StyleBundle("~/Content/jvectormap").Include(
            //          "~/js/jvector-map/jquery-jvectormap-1.2.2.css"));

            //bundles.Add(new StyleBundle("~/Content/clndr").Include(
            //          "~/Content/css/clndr.css"));

            //bundles.Add(new StyleBundle("~/Content/css3clock").Include(
            //          "~/js/css3clock/css/style.css"));

            //bundles.Add(new StyleBundle("~/Content/morrisChart").Include(
            //          "~/js/morris-chart/morris.css"));

            //bundles.Add(new StyleBundle("~/Content/icheck").Include(
            //          "~/js/iCheck/skins/minimal/minimal.css"));

            //bundles.Add(new StyleBundle("~/Content/wysihtml").Include(
            //          "~/js/bootstrap-wysihtml5/bootstrap-wysihtml5.css"));

            //bundles.Add(new StyleBundle("~/Content/datatable").Include(
            //         "~/js/advanced-datatable/css/demo_page.css",
            //         "~/js/advanced-datatable/css/demo_table.css",
            //         "~/js/data-tables/DT_bootstrap.css"));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
