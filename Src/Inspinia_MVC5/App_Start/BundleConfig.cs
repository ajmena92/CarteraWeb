using System.Web;
using System.Web.Optimization;

namespace WebCartera
{
    public static class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/animate.css",
                      "~/Content/style.css",
                      "~/Content/Custom.css",
                      "~/Content/toastr.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // jQuery
             bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                     "~/Scripts/jquery-{version}.js"));

            // jQuery Form Plugin  and Validate 
            bundles.Add(new ScriptBundle("~/bundles/jqueryForm").Include(
                    "~/Scripts/jquery.form.js",
                   "~/Scripts/jquery.unobtrusive*",
                   "~/Scripts/jquery.validate*"));
            // jQueryUI CSS
            bundles.Add(new ScriptBundle("~/Scripts/plugins/jquery-ui/jqueryuiStyles").Include(
                        "~/Scripts/plugins/jquery-ui/jquery-ui.min.css"));

            // jQueryUI 
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js"));

            // Bootstrap         
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/umd/popper.js",
                        "~/Scripts/bootstrap.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                      "~/Scripts/plugins/pace/pace.min.js",
                      "~/Scripts/app/inspinia.js",
                       "~/Scripts/app/custom.js",
                       "~/Scripts/toastr.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"));

            // Footable Styless
            bundles.Add(new StyleBundle("~/plugins/dynatableStyles").Include(
                      "~/Scripts/plugins/dynatable/jquery.dynatable.css", new CssRewriteUrlTransform()));

            // Footable alert
            bundles.Add(new ScriptBundle("~/plugins/dynatable").Include(
                      "~/Scripts/plugins/dynatable/jquery.dynatable.js"));

            //// dataPicker 
            //bundles.Add(new ScriptBundle("~/plugins/dataPicker").Include(
            //          "~/Scripts/plugins/datapicker/bootstrap-datepicker.js"));
            //// dataPicker styles
            //bundles.Add(new StyleBundle("~/plugins/dataPickerStyles").Include(
            //          "~/Content/plugins/datapicker/datepicker3.css"));



            // dataTables css styles
            bundles.Add(new StyleBundle("~/Content/plugins/dataTables/dataTablesStyles").Include(
                       "~/Content/DataTables/media/css/dataTables.bootstrap4.css",
                       "~/Content/DataTables/extensions/Buttons/css/buttons.bootstrap4.css",
                       "~/Content/DataTables/extensions/Responsive/css/responsive.bootstrap4.css",
                       "~/Content/DataTables/extensions/FixedHeader/css/fixedInspinia_MVC5Header.bootstrap4.css"));

            // dataTables 
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                        "~/Scripts/DataTables/media/js/jquery.dataTables.js",
                        "~/Scripts/DataTables/media/js/dataTables.bootstrap4.js",
                        "~/Scripts/DataTables/extensions/Buttons/js/dataTables.buttons.js",
                        "~/Scripts/DataTables/extensions/Buttons/js/buttons.bootstrap4.js",
                        "~/Scripts/DataTables/extensions/Responsive/js/dataTables.responsive.js",
                        "~/Scripts/DataTables/extensions/Responsive/js/responsive.bootstrap4.js",
                        "~/Scripts/DataTables/extensions/FixedHeader/js/dataTables.fixedHeader.js",
                        "~/Scripts/DataTables/extensions/FixedHeader/js/fixedHeader.bootstrap4.js"));
            // dataTables  addins
            bundles.Add(new ScriptBundle("~/plugins/dataTables_addings").Include(              
                        "~/Scripts/DataTables/extensions/JSZip/jszip.js",
                        "~/Scripts/DataTables/extensions/pdfmake/pdfmake.js",
                        "~/Scripts/DataTables/extensions/pdfmake/vfs_fonts.js",
                        "~/Scripts/DataTables/extensions/Buttons/js/buttons.html5.js",
                        "~/Scripts/DataTables/extensions/Buttons/js/buttons.print.js",
                        "~/Scripts/DataTables/extensions/Buttons/js/buttons.colVis.js"));


        }
    }
}
