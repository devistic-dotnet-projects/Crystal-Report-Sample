using CReport.Models;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrystalReportMVC.Controllers
{
    public class CustomerController : Controller
    {
        //DbContext  
        private CustomerDBEntities context = new CustomerDBEntities();
        // GET: Customer  
        public ActionResult Index()
        {
            var customerList = context.Customers.ToList();
            return View(customerList);
        }

        public ActionResult ExportCustomers()
        {
            List<Customer> allCustomer = new List<Customer>();
            allCustomer = context.Customers.ToList();

            ReportDocument rd = new ReportDocument();
            string path= Path.Combine(Server.MapPath("~/CrystalReport"), "ReportCustomer.rpt");

            rd.Load(path);

            rd.SetDataSource(allCustomer.ToList());

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "CustomerList.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}