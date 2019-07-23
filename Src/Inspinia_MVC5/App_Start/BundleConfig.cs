using System.Web;
using System.Web.Optimization;

namespace WebCartera
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/animate.css",
                      "~/Content/style.css",
                      "~/Content/Custom.css"));

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
                       "~/Scripts/app/custom.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"));

            // Footable Styless
            bundles.Add(new StyleBundle("~/plugins/dynatableStyles").Include(
                      "~/Scripts/plugins/dynatable/jquery.dynatable.css", new CssRewriteUrlTransform()));

            // Footable alert
            bundles.Add(new ScriptBundle("~/plugins/dynatable").Include(
                      "~/Scripts/plugins/dynatable/jquery.dynatable.js"));

            // dataPicker 
            bundles.Add(new ScriptBundle("~/plugins/dataPicker").Include(
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.js"));
            // dataPicker styles
            bundles.Add(new StyleBundle("~/plugins/dataPickerStyles").Include(
                      "~/Content/plugins/datapicker/datepicker3.css"));

        }
    }
}
