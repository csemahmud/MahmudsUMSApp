using System.Web;
using System.Web.Optimization;

namespace MahmudsUMSApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularui").Include(
                        "~/Scripts/angular-ui/ui-bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryhammer").Include(
                        "~/Scripts/jquery.hammer.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/ums_style.css"));
            bundles.Add(new StyleBundle("~/Content/site").Include("~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/core.css",
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/datepicker.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/theme.css"));

            bundles.Add(new StyleBundle("~/Content/themes/ui-darkness/css").Include(
                        "~/Content/themes/ui-darkness/jquery.ui.core.css",
                        "~/Content/themes/ui-darkness/jquery.ui.resizable.css",
                        "~/Content/themes/ui-darkness/jquery.ui.selectable.css",
                        "~/Content/themes/ui-darkness/jquery.ui.accordion.css",
                        "~/Content/themes/ui-darkness/jquery.ui.autocomplete.css",
                        "~/Content/themes/ui-darkness/jquery.ui.button.css",
                        "~/Content/themes/ui-darkness/jquery.ui.dialog.css",
                        "~/Content/themes/ui-darkness/jquery.ui.slider.css",
                        "~/Content/themes/ui-darkness/jquery.ui.tabs.css",
                        "~/Content/themes/ui-darkness/jquery.ui.datepicker.css",
                        "~/Content/themes/ui-darkness/jquery.ui.progressbar.css",
                        "~/Content/themes/ui-darkness/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/bootstrap-theme.css",
                        "~/Content/bootstrap-responsive.css"));

            bundles.Add(new StyleBundle("~/Content/uibootstrap").Include(
                        "~/Content/ui-bootstrap-csp.css"));

            bundles.Add(new StyleBundle("~/Content/font").Include(
                        "~/Content/font-awesome.css"));
        }
    }
}