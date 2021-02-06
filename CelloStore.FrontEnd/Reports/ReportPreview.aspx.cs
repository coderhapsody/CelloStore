using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using CelloStore.FrontEnd.Base;
using CelloStore.Providers;
using Microsoft.Reporting.WebForms;
using Ninject;

namespace CelloStore.FrontEnd.Reports
{
    public partial class ReportPreview : BaseForm
    {
        [Inject]
        public ReportProvider ReportService { get; set; }

        [Inject]
        public ConfigurationProvider ConfigurationService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptReport.InteractivityPostBackMode = InteractivityPostBackMode.AlwaysAsynchronous;
                string reportName = Request.QueryString["ReportName"];
                var keys = Request.QueryString.AllKeys.Where(param => param != "ReportName");

                var parameters = keys.ToDictionary(key => key, key => Request.QueryString[key]);


                var action = Delegate.CreateDelegate(
                        typeof(Action<string, Dictionary<string, string>>),
                        this,
                        reportName);

                action.DynamicInvoke(reportName, parameters);                
            }
        }

        public void DeliveryOrder(string reportName, Dictionary<string, string> parameters)
        {
            rptReport.LocalReport.ReportPath = String.Format(@"{0}\{1}", Server.MapPath(ConfigurationService[ConfigurationKeys.ReportFolder]), reportName + ".rdlc");
            var orderId = Convert.ToInt32(parameters["OrderId"]);
            ReportService.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var deliveryOrderHeader = ReportService.ReportDeliveryOrder(orderId);
            var deliveryOrderDetail = ReportService.ReportDeliveryOrderDetail(orderId);
            var deliveryOrderDataSource = new ReportDataSource("DeliveryOrder", deliveryOrderHeader);
            var deliveryOrderDetailDataSource = new ReportDataSource("DeliveryOrderDetail", deliveryOrderDetail);

            //var reportParameters = new List<ReportParameter>();
            //reportParameters.Add(new ReportParameter("FromDate", fromDate.ToString("dddd, dd MMMM yyyy")));
            //reportParameters.Add(new ReportParameter("ToDate", toDate.ToString("dddd, dd MMMM yyyy")));

            rptReport.LocalReport.DataSources.Add(deliveryOrderDataSource);
            rptReport.LocalReport.DataSources.Add(deliveryOrderDetailDataSource);
            //rptReport.LocalReport.SetParameters(reportParameters);
            rptReport.LocalReport.Refresh();
        }
    }
}