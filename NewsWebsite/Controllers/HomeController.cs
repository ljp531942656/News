using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace NewsWebsite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "select top 5 * from PIC where STATUS = '是' order by CREATETIME desc";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "PICList");
            DataTable dt = ds.Tables["PICList"];
            ViewData["PICList"] = dt;
            sqlcon.Close();
            return View();
        }
        public ActionResult NewsList()
        {
            return View();
        }
        public ActionResult NewsPage()
        {
            return View();
        }
        public ActionResult TypeDetail()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            string type = Request["type"];
            sqlcon.Open();
            string sql = string.Format("select * from NewsPage where newstype = '{0}'",type);
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "PICList");
            DataTable dt = ds.Tables["PICList"];
            sqlcon.Close();
            return View();
        }
    }
}