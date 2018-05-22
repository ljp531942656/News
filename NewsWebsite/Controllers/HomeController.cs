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

            string sql = "  select top 5 * from PIC where STATUS = '是' and NEWSTYPE is null and TYPENUM is null order by CREATETIME desc";
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

            for (int i = 1; i <= 10; i++)
            {
                sqlcommand = new SqlCommand(("select * from dbo.PIC where NEWSTYPE = '" + trans(i) + "' and TYPENUM <> 'null' order by TYPENUM"), sqlcon);
                adapter = new SqlDataAdapter(sqlcommand);
                ds = new DataSet();
                adapter.Fill(ds, "piclist");
                dt = ds.Tables["piclist"];
                ViewData[trans(i)] = dt;
                sqlcommand = null;
            }

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
            string newsid = Request.QueryString["newsid"];
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = string.Format("  select top 3 * from COMMENT where newsid = '{0}' and ZANNUM > 10 order by ZANNUM desc ", newsid);
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "TOP3Comment");
            ViewData["TOP3Comment"] = ds.Tables["TOP3Comment"];
            string top3id = "";
            foreach (DataRow dr in (ViewData["TOP3Comment"] as DataTable).Rows)
            {
                if (top3id == "")
                {
                    top3id = dr["id"].ToString();
                }
                else
                {
                    top3id = top3id + "," + dr["id"].ToString();
                }
            }
            sql = string.Format(" select * from COMMENT where id not in({0}) order by createtime desc", top3id);
            sqlcommand = new SqlCommand(sql, sqlcon);
            adapter = new SqlDataAdapter(sqlcommand);
            ds = new DataSet();
            adapter.Fill(ds, "Comment");
            ViewData["Comment"] = ds.Tables["Comment"];
            return View();
        }
        public ActionResult Zan()
        {
            try
            {
                string commentid = Request["commentid"];
                string ipaddress = Request["ipaddress"];
                string type = Request["type"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                if (type == "set")
                {
                    string sql = "IPADDRESS";
                    SqlConnection sqlcon = new SqlConnection(constring);
                    sqlcon.Open();
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.CommandType = CommandType.StoredProcedure;
                    sqlcommand.Parameters.Add("RETURN_VALUE", SqlDbType.Int, 10).Direction = ParameterDirection.ReturnValue;
                    sqlcommand.Parameters.Add("@id", SqlDbType.NVarChar, 10).Value = commentid;
                    sqlcommand.Parameters.Add("@zannum", SqlDbType.NVarChar, 10).Value = "";
                    sqlcommand.Parameters.Add("@hasip", SqlDbType.NVarChar, 10).Value = "";
                    sqlcommand.Parameters.Add("@OldIpaddress", SqlDbType.NVarChar, 10000).Value = "";
                    sqlcommand.Parameters.Add("@NewIpaddress", SqlDbType.NVarChar, 10000).Value = ipaddress;
                    sqlcommand.Parameters.Add("@Result", SqlDbType.NVarChar, 10000).Value = "";
                    sqlcommand.ExecuteNonQuery();
                    int result = int.Parse(sqlcommand.Parameters["RETURN_VALUE"].Value.ToString());
                    return Json(new { msg = "success", result = result });
                }
                else
                {
                    string sql = string.Format("select case when (select 1 from [News].[dbo].[COMMENT] where IPADDRESS LIKE '%' + '{0}' + '%' and id = {1}) = 1 then 1 else 0 end", ipaddress,commentid);
                    SqlConnection sqlcon = new SqlConnection(constring);
                    sqlcon.Open();
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "NewsPage");
                    string resutlt = ds.Tables["NewsPage"].Rows[0][0].ToString();
                    return Json(new { msg = "success", result = resutlt });
                }
            }
            catch (Exception ex)
            {
                
                return Json(new { msg = ex });
            }
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

        public string trans(int i)
        {
            string str = "";
            switch (i)
            {
                case 1: str = "SHDT"; break;
                case 2: str = "JRCJ"; break;
                case 3: str = "JQTY"; break;
                case 4: str = "KJQY"; break;
                case 5: str = "QCZX"; break;
                case 6: str = "FC"; break;
                case 7: str = "JS"; break;
                case 8: str = "YL"; break;
                case 9: str = "JK"; break;
                case 0: str = "QT"; break;
            }
            return str;
        }
    }
}