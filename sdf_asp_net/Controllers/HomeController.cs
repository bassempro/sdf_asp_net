
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;

namespace sdf_asp_net.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult MyCalendar()
        {
            ViewBag.Message = "Calendar info";

            return View();
        }

        string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=aspnet-sdf_asp_net-20181114091107;Integrated Security=True";
        public JsonResult GetEvents()
        {

            DataTable events = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                var sqlDa = new SqlDataAdapter("SELECT * FROM Events", sqlCon);
                sqlDa.Fill(events);
            //}

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in events.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in events.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }

                rows.Add(row);
            }

            return new JsonResult {Data = rows.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            }
        }

        public void AddEvents(Event e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                string query = "INSERT INTO Events VALUES(@Subject, @Description, @Start, @End, @ThemeColor, @IsFullDay)";
                SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Subject", e.Subject);
                sqlCmd.Parameters.AddWithValue("@Description", e.Description);
                sqlCmd.Parameters.AddWithValue("@Start", e.Start);
                sqlCmd.Parameters.AddWithValue("@End", e.End);
                sqlCmd.Parameters.AddWithValue("@ThemeColor", DBNull.Value);
                sqlCmd.Parameters.AddWithValue("@IsFullDay", false);
                sqlCmd.ExecuteNonQuery();
            }
        }
    }
}