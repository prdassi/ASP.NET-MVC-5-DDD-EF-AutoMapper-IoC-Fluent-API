using System.Web;
using System.Web.Optimization;

namespace HHT.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/javascripts").Include(
                        "~/Scripts/jquery-1.12.4.js",
                        "~/Scripts/perfect-scrollbar.js",
                        "~/Scripts/main.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/jquery.gritter.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/dataTables.bootstrap.min.js",
                        "~/Scripts/bootstrap-multiselect.js",
                        "~/Scripts/jquery.multi-select.js",
                        "~/Scripts/moment.min.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js",
                        "~/Scripts/daterangepicker.js",
                        "~/Scripts/select2.min.js",
                        "~/Scripts/dataTables.buttons.js",
                        "~/Scripts/buttons.html5.js",
                        "~/Scripts/buttons.flash.js",
                        "~/Scripts/buttons.print.js",
                        "~/Scripts/buttons.colVis.js",
                        "~/Scripts/buttons.bootstrap.js",                        
                        "~/Scripts/jquery.mask.min.js",
                        "~/Scripts/validator.min.js",
                        "~/Scripts/jquery.table2excel.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/empresa").Include(
                        "~/Scripts/Controllers/Empresa.js",                        
                        "~/Scripts/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/contratado").Include(
                        "~/Scripts/Controllers/Contratado.js",
                        "~/Scripts/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/documentoGeral").Include(
                        "~/Scripts/Controllers/DocumentoGeral.js"));

            bundles.Add(new ScriptBundle("~/bundles/hht").Include(
                       "~/Scripts/Controllers/HHT.js"));

            bundles.Add(new ScriptBundle("~/bundles/inconsistenciaHorario").Include(
                      "~/Scripts/Controllers/InconsistenciaHorario.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajustesPonto").Include(
                      "~/Scripts/Controllers/AjustePonto.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/perfect-scrollbar.css",
                        "~/Content/material-design-iconic-font.min.css",
                        "~/Content/dataTables.bootstrap.min.css",
                        "~/Content/font-awesome.min.css",
                        "~/Content/bootstrap-multiselect.css",
                        "~/Content/multi-select.css",
                        "~/Content/jquery.gritter.css",
                        "~/Content/style.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
