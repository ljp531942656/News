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

            string sql2 = "select top 7 ID,TITLE,convert(varchar(10),DATE,23) DATE,ISTOP from [NewsPage] where ISRELEASE = '是' order by ISTOP desc , DATE desc";
            sqlcommand = new SqlCommand(sql2, sqlcon);
            adapter = new SqlDataAdapter(sqlcommand);
            ds = new DataSet();
            adapter.Fill(ds, "TOP7News");
            dt = ds.Tables["TOP7News"];
            ViewData["TOP7News"] = dt;

            sqlcon.Close();
            return View();
        }
        public ActionResult MobileIndex()
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

            string sql2 = "select top 7 ID,TITLE,convert(varchar(10),DATE,23) DATE,ISTOP from [NewsPage] where ISRELEASE = '是' order by ISTOP desc , DATE desc";
            sqlcommand = new SqlCommand(sql2, sqlcon);
            adapter = new SqlDataAdapter(sqlcommand);
            ds = new DataSet();
            adapter.Fill(ds, "TOP7News");
            dt = ds.Tables["TOP7News"];
            ViewData["TOP7News"] = dt;

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
        public ActionResult GetArticle()
        {
            try
            {
                var newsid = Request["newsid"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format(" select ID,TITLE,AUTHOR,ORIGINAL,NEWSTYPE,convert(varchar(16),DATE,20) DATE,NEWSCONTENT from NewsPage where id = '{0}'", newsid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "NewsPage");
                DataRow dr = ds.Tables["NewsPage"].Rows[0];
                List<string> NewsPage = new List<string>();
                for (int i = 0; i < ds.Tables["NewsPage"].Columns.Count; i++)
                {
                    NewsPage.Add(dr[i].ToString());
                }
                sqlcon.Close();
                return Json(new { msg = "success" ,NewsPage = NewsPage});
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }

        public ActionResult TypeDetail()
        {
            try
            {
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                string type = Request["type"];
                sqlcon.Open();
                string sql = string.Format("select * from NewsPage where newstype = '{0}' order by DATE DESC", type);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "PICList");
                DataTable dt = ds.Tables["PICList"];
                sqlcon.Close();
                return Json(new { msg = "success",type = type});
            }
            catch (Exception ex)
            {

                return Json(new { msg=ex });
            }
        }
    }
}