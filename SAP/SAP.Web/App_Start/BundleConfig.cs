using System.Web.Optimization;

namespace SAP.Web
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

            //obsługa zegara serwerowego
            bundles.Add(new ScriptBundle("~/bundles/time").Include(
                "~/Scripts/server-time.js"));

            bundles.Add(new ScriptBundle("~/bundles/compilerArea").Include(
                "~/Content/edit_area/edit_area_full.js"));

            bundles.Add(new ScriptBundle("~/bundles/data_tables").Include(
                "~/Content/data_tables/js/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/data_tables").Include(
                "~/Content/data_tables/css/jquery.dataTables.min.css"));
        }
    }
}