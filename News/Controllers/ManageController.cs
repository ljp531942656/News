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
    public class ManageController : Controller
    {
        
        // GET: Manage
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Authority()
        {
            try
            {
                var username = Request["username"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format(" select authority from dbo.account where username = '{0}'", username);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "Authority");
                string Authority = ds.Tables["Authority"].Rows[0][0].ToString();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success", authority= Authority });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult submit()
        {
            try
            {
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("insert into dbo.NewsPage(TITLE,AUTHOR,ORIGINAL,NEWSTYPE,DATE,ISRELEASE,ISTOP,NEWSCONTENT) values('{0}','{1}','{2}','{3}',cast(nullif('{4}','') as DateTime),'{5}','{6}','{7}')", form["title"], form["author"], form["original"], form["newstype"], form["date"], form["release"], form["top"], form["newscontent"]);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });

            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        


        public ActionResult NewsEdit()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "  select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP] from dbo.NewsPage order by [ISRELEASE] DESC,[DATE] DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "NewsPage");
            DataTable dt = ds.Tables["NewsPage"];
            ViewData["NewsList"] = dt;
            sqlcon.Close();
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Edit()
        {
            try
            {
                string NEWSID = Request["NEWSID"];
                string STATUS = Request["STATUS"];
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                if (STATUS == "get")
                {
                    string sql = string.Format("select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT] from dbo.NewsPage where ID = '{0}'", NEWSID);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "CommontList");
                    DataTable dt = ds.Tables["CommontList"];
                    DataRow dr = dt.Rows[0];
                    List<string> newslist = new List<string>();
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        newslist.Add(dr[i].ToString());
                    }
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success", news = newslist });
                }
                else
                {
                    var sql = string.Format(" update dbo.NewsPage set [TITLE]='{0}',[AUTHOR]='{1}',[ORIGINAL]='{2}',[NEWSTYPE]='{3}',[DATE]=cast(nullif('{4}','') as DateTime),[ISRELEASE]='{5}',[ISTOP]='{6}',[NEWSCONTENT]='{7}' where [ID]='{8}'", form["title"], form["author"], form["original"], form["newstype"], form["date"], form["release"], form["top"], form["newscontent"], form["id"]);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.ExecuteNonQuery();
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult delete()
        {
            try
            {
                string newsid = Request["newsid"];
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("delete from dbo.NewsPage where id = '{0}'", newsid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }


        public ActionResult Comment()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[COMMONTNUM] from dbo.NewsPage where [ISRELEASE] = '是' order by DATE DESC,COMMONTNUM DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "CommontList");
            DataTable dt = ds.Tables["CommontList"];
            ViewData["CommontList"] = dt;
            sqlcon.Close();
            return View();
        }


        public ActionResult Account()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "select [ID],[USERNAME],[PASSWORD],[AUTHORITY],[LOGABLE] from dbo.ACCOUNT order by [LOGABLE] DESC,[AUTHORITY] DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "ACCOUNT");
            DataTable dt = ds.Tables["ACCOUNT"];
            ViewData["ACCOUNT"] = dt;
            sqlcon.Close();
            return View();
        }
        public ActionResult CreateAccount()
        {
            return View();
        }
        public ActionResult Accountcreate()
        {
            try
            {
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("insert into dbo.ACCOUNT(USERNAME,PASSWORD,AUTHORITY,LOGABLE) values('{0}','{1}','{2}','{3}')", form["username"], form["password"], form["authority"], form["logable"]);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        public ActionResult AccountEdit()
        {
            try
            {
                string ACCOUNTID = Request["ACCOUNTID"];
                string STATUS = Request["STATUS"];
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                if (STATUS == "get")
                {
                    string sql = string.Format(" select [ID],[USERNAME],[PASSWORD],[AUTHORITY],[LOGABLE] from dbo.ACCOUNT where [ID]='{0}'", ACCOUNTID);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "CommontList");
                    DataTable dt = ds.Tables["CommontList"];
                    DataRow dr = dt.Rows[0];
                    List<string> accountlist = new List<string>();
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        accountlist.Add(dr[i].ToString());
                    }
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success", news = accountlist });
                }
                else
                {
                    string sql = string.Format(" update dbo.ACCOUNT set [USERNAME]='{0}',[PASSWORD]='{1}',[AUTHORITY]='{2}',[LOGABLE]='{3}' where [ID]='{4}'",form["username"],form["password"],form["authority"],form["logable"],form["accountid"]);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.ExecuteNonQuery();
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult Accountdelete()
        {
            try
            {
                string accountid = Request["accountid"];
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("delete from dbo.ACCOUNT where id = '{0}'", accountid);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }


        public ActionResult Newscontent()
        {
            return View();
        }


        public ActionResult search(string[] str)
        {
            try
            {
                string table = Request["table"];
                string column = Request["column"];
                string str1 = Request["str"];
                string which = Request["which"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = "";
                var isrelease = "";
                var ordertype = "";
                if(which == "comment")
                {
                    isrelease = "and [ISRELEASE] = '是'";
                    ordertype = "[DATE] DESC,COMMONTNUM DESC";
                }
                else
                {
                    ordertype = "ISRELEASE DESC,[DATE] DESC";
                }
                if (column == "newstype")
                {
                    var newstypestr = "";
                    
                    if (str[0] == "")
                    {
                        sql = string.Format("select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT],[COMMONTNUM] from {0} where 1=1 " + isrelease + " order by " + ordertype, table);
                    }
                    else
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (i == 0)
                            {
                                newstypestr = "'" + str[i] + "'";
                            }
                            else
                            {
                                newstypestr = newstypestr + "," + "'" + str[i] + "'";
                            }
                        }
                        sql = string.Format("select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT],[COMMONTNUM] from {0} where newstype in ({1})" + isrelease + "order by " + ordertype, table, newstypestr);
                    }
                }
                else
                {
                    sql = string.Format("  select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP],[NEWSCONTENT],[COMMONTNUM] from {0} where {1} LIKE '%'+'{2}'+'%' and [ISRELEASE] = '是' order by [DATE] DESC,COMMONTNUM DESC", table, column, str1);
                }
                
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "SearchList");
                DataTable dt = ds.Tables["SearchList"];
                List<List<string>> searchlist = new List<List<string>>();
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    List<string> searchlist1 = new List<string>();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        searchlist1.Add(dr[j].ToString());
                    }
                    searchlist.Add(searchlist1);
                }
                return Json(new { msg = "success", rs = searchlist });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }

        public ActionResult sort()
        {
            try
            {
                string sort = Request["sort"];
                string type = Request["type"];
                string column = Request["column"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = "";
                if(type == "comment")
                {
                    sql = string.Format(" select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[COMMONTNUM] from dbo.NewsPage where [ISRELEASE] = '是' order by {0} {1} ",column,sort);
                }
                else
                {
                    sql = string.Format(" select [ID],[TITLE],[AUTHOR],[ORIGINAL],[NEWSTYPE],convert(varchar(16),[DATE],120) [DATE],[ISRELEASE],[ISTOP] from dbo.NewsPage order by {0} {1} ", column, sort);
                }
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                DataSet ds = new DataSet();
                adapter.Fill(ds, "SearchList");
                DataTable dt = ds.Tables["SearchList"];
                List<List<string>> sortlist = new List<List<string>>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    List<string> sortlist1 = new List<string>();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sortlist1.Add(dr[j].ToString());
                    }
                    sortlist.Add(sortlist1);
                }
                return Json(new { msg = "success", sortlist = sortlist });
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }

        public ActionResult Picauto()
        {
            var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);
            sqlcon.Open();
            string sql = "  select * from dbo.PIC order by STATUS DESC,CREATETIME DESC";
            SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
            SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "PICLIST");
            DataTable dt = ds.Tables["PICLIST"];
            ViewData["PICLIST"] = dt;
            sqlcon.Close();
            return View();
        }
        public ActionResult Picupload()
        {
            try
            {
                HttpPostedFileBase imgFile = Request.Files[0];
                if (System.IO.File.Exists(Server.MapPath("~/imglist/") + imgFile.FileName))
                {
                    string fail = "名为" + imgFile.FileName + "的文件已存在，请确认后重新上传";
                    return Json(new { code = 0, msg = fail });
                }
                else
                {
                    imgFile.SaveAs(Server.MapPath("~/IMGlist/") + imgFile.FileName);
                }
                var url = "/imglist/" + imgFile.FileName;
                return Json(new { msg = "success", url = url });
            }
            catch (Exception ex)
            {

                return Json(new { msg = ex });
            }
        }
        public ActionResult setPic()
        {
            try
            {
                var form = Request.Form;
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = string.Format("insert into dbo.PIC(TITLE,PICORIGIN,TOURL,STATUS,CREATETIME) values('{0}','{1}','{2}','{3}','{4}')", form["pictitle"], form["picorigin"], form["tourl"], form["status"], DateTime.Now);
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });

            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult editPic()
        {
            try
            {
                var id = Request["id"];
                var type = Request["type"];
                string sql = "";
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                if (type == "get")
                {
                    sql = string.Format("select * from dbo.PIC where id = '{0}'",id);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    SqlDataAdapter adapter = new SqlDataAdapter(sqlcommand);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "piclist");
                    DataTable dt = ds.Tables["piclist"];
                    DataRow dr = dt.Rows[0];
                    List<string> piclist = new List<string>();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        piclist.Add(dr[i].ToString());
                    }
                    sqlcommand = null;
                    return Json(new { msg = "success", piclist = piclist });
                }
                else
                {
                    var form = Request.Form;
                    sql = string.Format(" update dbo.PIC set [TITLE]='{0}',[PICORIGIN]='{1}',[TOURL]='{2}',[STATUS]='{3}' where [ID]='{4}'", form["pictitle"], form["picorigin"], form["tourl"], form["status"], form["picid"]);
                    SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                    sqlcommand.ExecuteNonQuery();
                    sqlcommand = null;
                    sqlcon.Close();
                    sqlcon = null;
                    return Json(new { msg = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
        public ActionResult deletePic()
        {
            try
            {
                var type = Request["type"];
                var constring = ConfigurationManager.ConnectionStrings["NEWS"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);
                sqlcon.Open();
                string sql = "";
                if (type == "pic")
                {
                    var filename = Request["filename"];
                    if (System.IO.File.Exists(Server.MapPath("~/imglist/") + filename))
                    {
                        System.IO.File.Delete(Server.MapPath("~/imglist/") + filename);
                    }
                }
                else
                {
                    var id = Request["id"];
                    sql = string.Format("delete from dbo.PIC where id = '{0}'", id);
                }
                SqlCommand sqlcommand = new SqlCommand(sql, sqlcon);
                sqlcommand.ExecuteNonQuery();
                sqlcommand = null;
                sqlcon.Close();
                sqlcon = null;
                return Json(new { msg = "success" });

            }
            catch (Exception ex)
            {
                return Json(new { msg = ex });
            }
        }
    }
}