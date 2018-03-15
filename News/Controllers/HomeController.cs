using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace News.Controllers
{
    public class HomeController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckLogin()
        {
            try
            {
                var form = Request.Form;
                var customerid = "0";
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                var sql = string.Format("  select case when a.username='{0}' and a.password='{1}' and a.LOGABLE='是' then a.id when a.username!='{0}' or a.password!='{1}' and a.LOGABLE='是' then '0' when a.username='{0}' and a.password='{1}' and a.LOGABLE='否' then '-1' end as res from dbo.ACCOUNT a", form["username"],form["password"]);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "List");
                DataTable dt = ds.Tables["List"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = ds.Tables["List"].Rows[i];
                    if(dr[0].ToString() != "0")
                    {
                        customerid = dr[0].ToString();
                        break;
                    }

                }
                List<string> accountlist = new List<string>();
                return Json(new { msg = "success" ,customerid = customerid });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }

        public ActionResult GetUsername()
        {
            try
            {
                var customerid = Request["customerid"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                var sql = string.Format(" select USERNAME from dbo.ACCOUNT where id = '{0}' ", customerid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "List");
                string username = ds.Tables["List"].Rows[0][0].ToString();
                return Json(new { msg = "success", username = username });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
    }
}