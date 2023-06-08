using System.Web;
using System.Web.Optimization;

namespace MMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        //"~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-1.12.4.js",
                        //"~/Scripts/bootstrap-datepicker.js", 
                          "~/Scripts/jquery.ui.timepicker.js",
                          "~/Scripts/jquery.validate.min.js",
                           "~/Scripts/jquery.validate.unobtrusive.js",
                        //"~/Scripts/jquery-ui.js"
                        "~/Scripts/jquery.latest.ui.js"

                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/customVal").Include(
                          "~/Scripts/custom.js")
                       );

            //added new bundle for including 'customScript.js'
            bundles.Add(new ScriptBundle("~/bundles/customValScript").Include(
                         "~/Scripts/customScript.js")
                      );

            bundles.Add(new ScriptBundle("~/bundles/multiselect").Include(
                                "~/Scripts/bootstrap-multiselect.js",
                                "~/Scripts/jquery.simplePagination.js"
                                )
                         );

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                         "~/Scripts/tinymce/js/tinymce/tinymce.min.js")
                      );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/SideMenu.css",
                      "~/Content/mms.css",
                      "~/Content/datepicker.css",
                      "~/Content/jquery.ui.timepicker.css"
                      ));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
